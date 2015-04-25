using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace XPlaneGenConsole
{
    public class DataWriter<T> : IDisposable 
		where T : BinaryDatapoint
    {
        private BinaryWriter writer;
        private MemoryStream stream;
        private readonly string outputPath;
		//private readonly int bytesPerDatapoint = 10;

        public DataWriter(string path = "")
        {
            // TODO: randomly generate a unique file name when no path is provided  

            outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "Data", string.IsNullOrEmpty(path) ? "data.bin" : path);

            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
        }

        public void Dispose()
        {
			using (var fs = new FileStream (outputPath, FileMode.OpenOrCreate)) {
				try {
					stream.WriteTo (fs);
				} catch (Exception ex) {
					Console.WriteLine ("Unable to write to file. Error: {0}", ex.Message);
				}
			}
        }
			
        public void Write(Stream stream)
        {
			stream.CopyTo (this.stream);
        }

        public void Write(T datapoint)
        {
            if (datapoint != null && datapoint.IsValid)
            {
                writer.Write(datapoint.Data);
            }
        }

        public void Write(IEnumerable<T> datapoints)
        {
			var start = DateTime.Now;

            //Console.WriteLine("Using {0} records", datapoints.Count(f => f.IsValid));

			var q = from d in datapoints
			        group d by d.Flight into g
			        select new FlightHeader
					{
						Flight = g.Key,
						Start = (from t2 in g select t2.DateTime).Min (),
						End = (from t3 in g select t3.DateTime).Max (),
						Count = (from t4 in g select t4).Count ()					
					};

            //Console.WriteLine("Writing {0} flights",q.Count());

			writer.Write (q.Count ()); // count of unique records

			foreach(var item in q){
				writer.Write (item.Flight);
				writer.Write (item.Count);			
				writer.Write (item.Start.ToBinary ());
				writer.Write (item.End.ToBinary ());
			}

            foreach (var dp in datapoints)
            {
                Write(dp);
            }

			Console.WriteLine (DateTime.Now.Subtract (start).TotalSeconds);
        }
    }

    /*internal sealed class Writer<T> where T : Datapoint<T>
    {
        private Writer() { }

        private static IEnumerable<T> AssignFlightIds(IEnumerable<T> datapoints, IEnumerable<DateTime> flightTimes)
        {
            Random r = new Random();
            int id;
            DateTime start = DateTime.MinValue,
                     end = DateTime.MaxValue;

            for (int i = 0; i < flightTimes.Count(); i++)
            {
                // Set start datetime
                start = flightTimes.ElementAt(i);

                // Set end datetime if there is another element, otherwise use DateTime.MaxValue
                end = i < flightTimes.Count() - 1 ? flightTimes.ElementAt(i + 1) : DateTime.MaxValue;

                // Generate random flight ID number
                id = r.Next();

                // Select datapoints between start and end datetimes
                var pts = from dp in datapoints
                          where dp.DateTime > start && dp.DateTime < end
                          select dp;

                //Debug.WriteLine(string.Format("{0}: {1}", start.ToString(), pts.Count()));

                // Assign Flight ID to selected datapoints
                foreach (var dp in pts)
                {
                    dp.Flight = id;
                }
            }

            return from dp in datapoints
                   orderby dp.Flight, dp.DateTime
                   select dp;
        }

        public static void WriteFile(IEnumerable<T> datapoints, string path, IEnumerable<DateTime> flightTimes = default(IEnumerable<DateTime>))
        {
            //TODO: add CancellationToken

            Debug.WriteLine("Writing Points: " + datapoints.Count());
            //Debug.WriteLine("Using Flight Times: " + flightTimes.Count());

            Debug.Assert(datapoints.Count() > 0, "Datapoints must be greater than zero");
            //Debug.Assert(flightTimes.Count() > 0, "There must be a positive number of flight times");

            // If FlightTimes has 1 or more elements, assign them to the datapoints otherwise assume flight ids are already set
            var flights = (flightTimes.Count() > 0) ?
                    AssignFlightIds(datapoints, flightTimes) :
                    from dp in datapoints
                    orderby dp.Flight, dp.DateTime
                    select dp;

            Write(flights, Datapoint<T>.BYTES_COUNT, new BinaryWriter(File.OpenWrite(path)));
        }

        private static void Write(IEnumerable<T> datapoints, int size, BinaryWriter writer)
        {
            using (writer)
            {
                try
                {
                    writer.BaseStream.Seek(0, SeekOrigin.Begin);

                    writer.Write(datapoints.Count());
                    writer.Write(size);

                    long pos = 0;

                    foreach (var dp in datapoints)
                    {
                        // store position
                        pos = writer.BaseStream.Position;

                        // write bytes
                        writer.Write(dp.GetBytes());

                        Debug.Assert(pos + size == writer.BaseStream.Position, "The incorrect amount of bytes was written");
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }*/
}
