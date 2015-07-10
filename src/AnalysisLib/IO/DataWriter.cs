using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FDA
{
   [Obsolete()]
    public class DataWriter<T> : IDisposable
        where T : BinaryDatapoint, new()
    {
        private BinaryWriter writer;
        private MemoryStream stream;
        private readonly string outputPath;
        //private readonly int bytesPerDatapoint = 10;

        public DataWriter(string path) : this(File.Open(path,FileMode.Create))
        {
            outputPath = path; //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "Data", path);
        }

        public DataWriter(Stream stream)
        {
            this.stream = new MemoryStream();
            stream.CopyTo(this.stream);
            stream.Close();
            writer = new BinaryWriter(this.stream);
        }

        public void Dispose()
        {
            using (var fs = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                try
                {
                    stream.WriteTo(fs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to write to file. Error: {0}", ex.Message);
                }
            }
        }

        public void Write(Stream stream)
        {
            stream.CopyTo(this.stream);
        }

        public void Write(T datapoint)
        {
            if (datapoint != null && datapoint.IsValid)
            {
                //writer.Write(datapoint.Data);
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
                        Start = (from t2 in g select t2.DateTime).Min(),
                        End = (from t3 in g select t3.DateTime).Max(),
                        Count = (from t4 in g select t4).Count()
                    };

            //Console.WriteLine("Writing {0} flights",q.Count());

            if (q.Count() == 0)
            {
                Console.WriteLine("Nothing to write");
                return;
            }

            writer.Write(q.Count()); // count of unique records

            foreach (var item in q)
            {
                writer.Write(item.Flight);
                writer.Write(item.Count);
                writer.Write(item.Start.ToBinary());
                writer.Write(item.End.ToBinary());
            }

            foreach (var dp in datapoints)
            {
                Write(dp);
            }

            Console.WriteLine(DateTime.Now.Subtract(start).TotalSeconds);
        }
    }
}