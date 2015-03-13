using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace XPlaneGenConsole
{
    internal sealed class Writer<T> where T : Datapoint<T>
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

            Write(flights, Datapoint<T>.LENGTH, new BinaryWriter(File.OpenWrite(path)));
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

                        // Make sure the correct amount of bytes was written 
                        if (pos + size != writer.BaseStream.Position)
                        {
                            // If this occurs, then the GetBytes method should be inspected
                            Debug.WriteLine("Incorrect Offset Warning");
                            continue;
                        }
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
