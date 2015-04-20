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

            while (count > 0)
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

        public FlightCSVReader(Stream stream)
        {
            reader = new StreamReader(stream);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
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
            using (reader)
            {
                while (!reader.EndOfStream)
                {
                    T datapoint = Activator.CreateInstance<T>();

                    datapoint.Load(reader.ReadLine());

                    if (datapoint.IsValid)
                    {
                        datapoint.GetBytes();
                        yield return datapoint;
                    }
                }
            }

            yield break;
        }
    }
}
