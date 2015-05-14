using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
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
        static Action<T, string[]> parser = CsvParser.GetParser<T>();

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
            int count = 0;

            Console.WriteLine("Writing to {0}", filePath);

            var ordered = from dp in OutputQueue
                          orderby dp.DateTime, dp.Timestamp
                          select dp;

            count = ordered.Count();

            using (var writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                foreach (var dp in ordered)
                {
                    write(dp, writer);
                }
            }

            Console.WriteLine("Consumer thread finished | {0} datapoints", count);
        }

        static void ProducerThread(object path)
        {
            if (!File.Exists(path as string))
            {
                throw new FileNotFoundException();
            }

            StreamReader reader = new StreamReader(path as string);
            Console.WriteLine("Producer is reading from {0}", path);

            bool AreThreadsSet = false;

            using (reader)
            {
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    InputQueue.Enqueue(reader.ReadLine());

                    if (!AreThreadsSet)
                    {
                        AreThreadsSet = true;
                        startBarrier.SignalAndWait();
                    }
                }
            }

            barrier.SignalAndWait();
            InputQueue = null;
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
            }

            while (!InputQueue.IsEmpty)
            {
                if (InputQueue.TryDequeue(out result))
                {
                    var value = result.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (value.Length == 30)
                    {
                        T dp = (T)BinaryDatapoint.Create<T>();
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
                }
            }

            barrier.SignalAndWait();
		}
	}
}