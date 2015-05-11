using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
	public class PipedCsvReader
	{
		static Dictionary<int,AutoResetEvent> ConsumerEvents = new Dictionary<int, AutoResetEvent>();
		static bool IsDoneReading = false;
		static CountdownEvent Countdown;
		static ConcurrentQueue<string> Queue;
		static readonly int QueueBufferSize = 8;

		public static void Read(string path)
		{
			//ConsumerReadEvent = new AutoResetEvent (false);
			Countdown = new CountdownEvent (4);
		
			Queue = new ConcurrentQueue<string> ();

			Thread producer = new Thread (ProducerThread);
			producer.Start (path);

			Thread[] threads = new Thread[2];

			for (int i = 0; i < threads.Length; i++) {
				threads [i] = new Thread (ThreadRead);
				ConsumerEvents.Add (threads [i].ManagedThreadId, new AutoResetEvent (false));
				threads [i].Start ();
			}

			Countdown.Signal ();
			Countdown.Signal ();
			Countdown.Signal ();
			Countdown.Signal ();

			producer.Join ();

			// Wait for all threads to end
			foreach (var t in threads) {
				t.Join ();
			}

			Console.WriteLine ("Process completed");
		}

		static void ProducerThread(object path)
		{
			Console.WriteLine ("Producer Thread {0}", Thread.CurrentThread.ManagedThreadId);

			StreamReader reader = new StreamReader (path as string);

			Console.WriteLine ("Producer is reading from {0}", path);
			IsDoneReading = false;
			using (reader) {
				while (!reader.EndOfStream) {
					Console.WriteLine ("Producer {0} is waiting to load", Thread.CurrentThread.ManagedThreadId);

					Countdown.Wait (); // Wait for buffer to be half-way emptied

					while (Queue.Count < QueueBufferSize) {
						Queue.Enqueue (reader.ReadLine ());
					}

					Countdown.Reset ();

					Console.WriteLine ("{0}%", 100 * reader.BaseStream.Position / reader.BaseStream.Length);

					// Signal to all consumer threads
					foreach (var evt in ConsumerEvents.Values) {
						evt.Set ();
					}
				}
			}

			IsDoneReading = true;
			Console.WriteLine ("Producer finished");
		}

		static void ThreadRead()
		{
			//NamedPipeServerStream pipeServer = new NamedPipeServerStream ("CsvReader", PipeDirection.InOut, Environment.ProcessorCount);
			string result;
			var tid = Thread.CurrentThread.ManagedThreadId;

			//pipeServer.WaitForConnection ();

			while (!IsDoneReading) {
				Console.WriteLine ("Consumer {0} waiting to read", tid);
				// Wait for data to be read into buffer
				ConsumerEvents[tid].WaitOne();

				if (Queue.TryDequeue (out result)) {
					Console.WriteLine ("Consumer {0} successfully read", tid);
					Countdown.Signal ();
				} else {
					Console.WriteLine ("Consumer {0} unable to read", tid);
				}
			}

			Console.WriteLine ("Consumer {0} finished", tid);
		}

		static void FileRead()
		{

		}
	}
}