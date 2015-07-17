using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis; // SuppressMessageAttribute
using System.IO;
using System.IO.Compression;
using System.Linq; // OrderBy
using System.Reflection; // GetCustomAttribute<T>
using System.Threading; // Thread, Barrier, CancellationToken
using System.Threading.Tasks; // Task, TaskCreationOptions, TaskScheduler
using FDA.Attributes;
using FDA.IO; // Hash
using FDA.Extensions;

namespace FDA
{
    /// <summary>
    /// Converts CSV from text to object using threads and concurrent structure
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public static class CsvConverter<T>
        where T: BinaryDatapoint, new()
	{
        private static Barrier endBarrier;
        private static Barrier startBarrier;
        private static ConcurrentQueue<string> inputQueue;
        private static ConcurrentQueue<T> outputQueue;
        private static ConcurrentQueue<Tuple<DateTime, int>> flightTimes;

        private static int validLineCount = 0;
        private static int Fields = typeof(T).GetCustomAttribute<CsvRecordAttribute>().Count;

        private static Action<T, string[]> parser = CsvParser.GetParser<T>();

        private static readonly object _locker = new object();

        public static async Task LoadAsync(string path, string outputPath)
        {
            await Task.Factory.StartNew(() => Load(path, outputPath), CancellationToken.None, TaskCreationOptions.AttachedToParent, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private static void Initialize()
        {
            endBarrier = new Barrier(Environment.ProcessorCount + 1);
            startBarrier = new Barrier(Environment.ProcessorCount + 1);

            inputQueue = new ConcurrentQueue<string>();
            outputQueue = new ConcurrentQueue<T>();
            flightTimes = new ConcurrentQueue<Tuple<DateTime, int>>();
        }

        private static void VerifyPath(string path, ref string outputDirectory)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("path");
            }

            if (string.IsNullOrWhiteSpace(outputDirectory))
            {
                throw new ArgumentNullException("outputDirectory");
            }

            if (!Directory.Exists(outputDirectory))
            {
                throw new DirectoryNotFoundException();
            }

            var hash = Hash.ComputeHash(path);
            outputDirectory = Path.Combine(outputDirectory, BitConverter.ToString(hash).Replace("-", ""));
        }

        /// <summary>
        /// Loads and processes a CSV file
        /// </summary>
        /// <param name="path">Path of incoming file</param>
        /// <param name="outputDirectory">Output directory</param>
        public static void Load(string path, string outputDirectory)
        {
            Initialize();

            VerifyPath(path, ref outputDirectory);

            Thread producer = new Thread(ProducerThread);
            Thread consumer = new Thread(ConsumerThread);
            Thread[] workers = new Thread[Environment.ProcessorCount];

            for (int i = 0; i < workers.Length; i++)
            {
                workers[i] = new Thread(WorkerRead);
                workers[i].Start();
            }

            producer.Start(path);
            producer.Join(); // Producer has a barrier that is associated with the worker threads, so this blocks until all workers are done

            // TODO: Write output information in JSON format

            // TODO: allow the consumer to write to the output file while the other threads are still working
            consumer.Start(outputDirectory);
            consumer.Join();
        }

        /// <summary>
        /// Writes objects to a compressed filestream
        /// </summary>
        /// <param name="path">path of output</param>
        [SuppressMessage("Microsoft.Usage","CA2202:Do not dispose objects multiple times")]
        private static void ConsumerThread(object path)
        {
            var ms = new MemoryStream();
            byte[] data = new byte[] { };
            long compressSize, normalSize;

            Action<T, BinaryWriter> write = BinaryDatapoint.GetWriteAction<T>();

            string filePath = path as string;

            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException("path");
            }

            // Make sure elements are ordered by DateTime and Timestamp
            // DateTime is not guaranteed to be unique
            // TimeStamp is unique by value, but values eventually cycle. Thus the order cannot be trusted
            var ordered = from dp in outputQueue
                          //orderby /*dp.DateTime,*/ dp.Timestamp
                          select dp;

            // TODO: inheritance hierarchy was changed. Ordering is broken and needs to be fixed

            // TODO: Keep track of an index to improve lookup speeds.

            // Write datapoints to MemoryStream (not directly to FileStream)
            // Note: this cannot be done inside of a 'using(GZipStream)' block because the data will not be correctly written
            using (var writer = new BinaryWriter(ms))
            {
                foreach (var dp in ordered)
                {
                    write(dp, writer);
                }

                data = ms.ToArray();
                normalSize = ms.Length;
            }

            // TODO: Add switch for enabling/disabling compression

            // Compress to GZipStream (backed by FileStream)
            using (var file = File.Open(filePath, FileMode.Create))            
            using (var compress = new GZipStream(file, CompressionMode.Compress))
            {
                compress.Write(data, 0, data.Length);
                compressSize = compress.BaseStream.Length;  // keep track of size for writing info to a JSON object
            }

            /*lock (_locker)
            {
                // TODO: Write output information in JSON format, not flat string
            }*/
        }

        static void ProducerThread(object path)
        {
            var filePath = path as string;
            bool ReadySet = false;

            if (filePath == null)
            {
                throw new ArgumentNullException("path");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }
            
            using (var reader = new StreamReader(filePath))
            {
                reader.ReadLine(); // The first line is the name of fields

                while (!reader.EndOfStream)
                {
                    inputQueue.Enqueue(reader.ReadLine());

                    if (!ReadySet)
                    {
                        ReadySet = true; // GO!
                        startBarrier.SignalAndWait();
                    }
                }
            }

            endBarrier.SignalAndWait();
            inputQueue = null;
        }

        /// <summary>
        /// Thread reads and parses a single line
        /// </summary>
		static void WorkerRead()
		{
			string result;
			//var tid = Thread.CurrentThread.ManagedThreadId; // used only for debugging

            // Wait for the go signal
            try
            {
                startBarrier.SignalAndWait();
            }
            catch (EntryPointNotFoundException ex) // this is an infrequent exception that happens time to time
            {
                startBarrier.RemoveParticipant();
                endBarrier.RemoveParticipant();
                Thread.CurrentThread.Abort();
                Console.WriteLine(ex.Message);
                return;
            }
            catch (Exception)
            {
                throw; // preserve stack trace
            }

            while (!inputQueue.IsEmpty)
            {
                if (inputQueue.TryDequeue(out result))
                {
                    var value = result.Split(new char[] { ',' }, StringSplitOptions.None);

                    // Only parse if the number of elements is correct
                    if (value.Length == Fields)
                    {
                        T dp = Activator.CreateInstance<T>();
                        parser(dp, value); // This is the generated

                        outputQueue.Enqueue(dp);
                        Interlocked.Increment(ref validLineCount);
                    }
                    else if (value.Length == 4 && value[3].Contains("POWER ON"))
                    {
                        // A new flight is indicated by the presence of "POWER ON" in the 4th element
                        flightTimes.Enqueue(
                            new Tuple<DateTime, int>(
                                value[1].AsDateTime(value[2].AsTimeSpan()),
                                value[3].AsInt()
                                ));
                    }
                    else
                    {
                        // Skip meaningless data
                    }
                }
            }

            endBarrier.SignalAndWait();
		}
	}
}