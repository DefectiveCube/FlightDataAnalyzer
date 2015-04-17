using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    /// <summary>
    /// For reading binary types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FlightDataReader<T> where T : Datapoint<T>
    {
        private BinaryReader reader;
        private MemoryStream stream;
        private readonly int bytesToRead;

        public FlightDataReader(Stream stream)
        {
            this.stream = new MemoryStream();

            using (stream)
            {
                stream.CopyTo(this.stream);
            }

            this.stream.Seek(0, SeekOrigin.Begin);

            reader = new BinaryReader(this.stream);

            Type t = typeof(T);

            if (t == typeof(FlightDatapoint))
            {
                // TODO: fix this
                bytesToRead = 87;
            }
            else if (t == typeof(EngineDatapoint))
            {
                bytesToRead = EngineDatapoint.BYTES_COUNT;
            }
            else if (t == typeof(SystemDatapoint))
            {
                bytesToRead = SystemDatapoint.BYTES_COUNT;
            }
            else
            {
                throw new Exception();
            }
        }

        public T Read()
        {
            T datapoint = Activator.CreateInstance<T>();

            datapoint.Load(reader.ReadBytes(bytesToRead));

            return datapoint;
        }

        public object ReadField(int index) { return 0; }

        public short ReadInt16() { return reader.ReadInt16(); }

        public int ReadInt32() { return reader.ReadInt32(); }

        public long ReadInt64() { return reader.ReadInt64(); }

        public uint ReadUInt() { return reader.ReadUInt32(); }

        public IEnumerable<T> ReadToEnd()
        {
            var count = stream.Length / bytesToRead;

            Console.WriteLine(count);

            while(count > 0)
            {
                count--;

                yield return Read();
            }

            yield break;
        }
    }

    /// <summary>
    /// For reading CSV files
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FlightCSVReader<T> : TextReader where T : Datapoint<T>
    {
        private StreamReader reader;
        private MemoryStream stream;

        public FlightCSVReader(Stream stream)
        {
            this.stream = new MemoryStream();

            using (stream)
            {
                stream.CopyTo(this.stream);
            }

            this.stream.Seek(0, SeekOrigin.Begin);

            reader = new StreamReader(this.stream);
            reader.ReadLine();           
        }

        public new T ReadLine()
        {
            if (reader == null)
            {
                throw new EndOfStreamException();
            }

            T datapoint = Activator.CreateInstance<T>();

            datapoint.Load(reader.ReadLine());

            if (datapoint.IsValid)
            {
                datapoint.GetBytes();
            }

            return datapoint;
        }

        public new IEnumerable<T> ReadToEnd()
        {
            List<string> allLines = new List<string>();

            using (reader)
            {
                while (!reader.EndOfStream)
                {
                    allLines.Add(reader.ReadLine());
                }
            }

            for(int i = 0; i < allLines.Count; i++)
            {
                T datapoint = Activator.CreateInstance<T>();

                datapoint.Load(allLines.ElementAt(i));

                if (datapoint.IsValid)
                {
                    datapoint.GetBytes();
                    yield return datapoint;
                }
            }

            yield break;
        }
    }

    public class Reader<T> where T : Datapoint<T>
    {
        private MemoryStream inStream;

        public Reader(Stream file)
        {
            inStream = new MemoryStream();
            file.CopyTo(inStream);
            file.Close();
            file.Dispose();

            inStream.Seek(0, SeekOrigin.Begin);
        }

        public IEnumerable<T> ReadAll()
        {
            List<string> allLines = new List<string>();

            using(StreamReader reader = new StreamReader(inStream))
            {
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    allLines.Add(reader.ReadLine());
                }
            }

            Console.WriteLine("[{1}] Lines: {0}", allLines.Count, typeof(T).Name);

            for(int i = 0; i < allLines.Count; i++)
            {
                T dp = Activator.CreateInstance<T>();

                dp.Load(allLines.ElementAt(i));

                if (dp.IsValid)
                {
                    dp.GetBytes();
                    yield return dp;                
                }
            }

            yield break;
        }
    }
}
