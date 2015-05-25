using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using XPlaneGenConsole;

namespace XPlaneGenConsole
{
	public class CsvConverter<T>
        where T: BinaryDatapoint, new()
	{
        static Barrier barrier;
        static Barrier startBarrier;
		static ConcurrentQueue<string> InputQueue;
        static ConcurrentQueue<T> OutputQueue;
        static ConcurrentQueue<Tuple<DateTime,int>> FlightTimes;
        static int validLineCount = 0;
        static int Fields = typeof(T).GetCustomAttribute<CsvRecordAttribute>().Count;
        static Action<T, string[]> parser = CsvParser.GetParser<T>();


        public static async Task LoadAsync(string path, string outputPath)
        {
            await Task.Factory.StartNew(() => Load(path, outputPath));
        }

        public static void Load(string path, string outputPath)
        {
            outputPath = !string.IsNullOrWhiteSpace(outputPath) ? outputPath : path + ".output";

            barrier = new Barrier(Environment.ProcessorCount + 1);
            startBarrier = new Barrier(Environment.ProcessorCount + 1);

            InputQueue = new ConcurrentQueue<string>();
            OutputQueue = new ConcurrentQueue<T>();
            FlightTimes = new ConcurrentQueue<Tuple<DateTime, int>>();

            // Reader thread
            Thread producer = new Thread(ProducerThread);

            // Writer thread
            Thread consumer = new Thread(ConsumerThread);

            // Worker threads
            Thread[] threads = new Thread[Environment.ProcessorCount];

            var start = DateTime.Now;

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(ThreadRead);
                threads[i].Start();
            }

            // Start reading
            producer.Start(path);
            producer.Join(); // Producer has a barrier with the worker threads, so this blocks until all workers are done

            // Start writing
            consumer.Start(outputPath);
            consumer.Join();

            Console.WriteLine("Valid lines: {0}", validLineCount);
            Console.WriteLine("Unique Flights: {0}", FlightTimes.Count);
            Console.WriteLine("Process completed");
            Console.WriteLine(DateTime.Now.Subtract(start).TotalSeconds);

            OutputQueue = null;
            FlightTimes = null;
        }

        static void ConsumerThread(object path)
        {
            var write = BinaryDatapoint.GetWriteAction<T>();
            string filePath = path as string;

            Console.WriteLine("Writing to {0}", filePath);
            Console.WriteLine("Consumer found {0} datapoints", validLineCount);

            var ordered = from dp in OutputQueue
                          orderby dp.DateTime, dp.Timestamp
                          select dp;

            var ms = new MemoryStream();
            byte[] data = new byte[] { };
            long compressSize, normalSize;

            using (var writer = new BinaryWriter(ms))
            {
                foreach (var dp in ordered)
                {
                    write(dp, writer);
                }

                data = ms.ToArray();
                normalSize = ms.Length;
            }

            using (var file = File.Open(filePath, FileMode.Create))            
            using (var compress = new GZipStream(file, CompressionMode.Compress))
            {
                compress.Write(data, 0, data.Length);
                compressSize = compress.BaseStream.Length;
            }

            Console.WriteLine("Uncompressed Size: {0} bytes", normalSize);
            Console.WriteLine("Compressed Size: {0} bytes", compressSize);
            Console.WriteLine("Compression Ratio: {0:P}", 1.0 - (double)compressSize / normalSize);
            Console.WriteLine("Consumer thread finished | {0} datapoints", ordered.Count());
        }

        static void ProducerThread(object path)
        {
            if (!File.Exists(path as string))
            {
                throw new FileNotFoundException();
            }

            StreamReader reader = new StreamReader(path as string);
            Console.WriteLine("Producer is reading from {0}", path);
            int count = 0;

            bool AreThreadsSet = false;

            using (reader)
            {
                Console.WriteLine("File size: {0}", reader.BaseStream.Length);
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    InputQueue.Enqueue(reader.ReadLine());
                    count++;

                    if (!AreThreadsSet)
                    {
                        AreThreadsSet = true;
                        startBarrier.SignalAndWait();
                    }
                }
            }

            barrier.SignalAndWait();
            InputQueue = null;
            Console.WriteLine("Read In {0} lines", count);
        }

		static void ThreadRead()
		{
			string result;
			var tid = Thread.CurrentThread.ManagedThreadId;

            // Wait for the go signal
            try
            {
                startBarrier.SignalAndWait();
            }
            catch (EntryPointNotFoundException ex)
            {
                startBarrier.RemoveParticipant();
                Thread.CurrentThread.Abort();
                Console.WriteLine(ex.Message);
                return;
            }

            while (!InputQueue.IsEmpty)
            {
                if (InputQueue.TryDequeue(out result))
                {
                    var value = result.Split(new char[] { ',' }, StringSplitOptions.None);



                    if (value.Length == Fields)
                    {
                        T dp = Activator.CreateInstance<T>();
                        parser(dp, value);

                        OutputQueue.Enqueue(dp);
                        Interlocked.Increment(ref validLineCount);
                    }
                    else if (value.Length == 4 && value[3].Contains("POWER ON"))
                    {
                        FlightTimes.Enqueue(
                            new Tuple<DateTime, int>(
                                value[1].AsDateTime(value[2].AsTimeSpan()),
                                value[3].AsInt()
                                ));
                    }
                    else
                    {
                        Console.WriteLine("Skipping {0}", value.Length);
                    }
                }
            }

            barrier.SignalAndWait();
		}
	}
}