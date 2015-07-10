using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FDA
{
    /// <summary>
    /// For reading binary types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Obsolete()]public class DataReader<T> : IDisposable
        where T : BinaryDatapoint, new()
    {
        private BinaryReader reader;
        private MemoryStream stream;
        //private readonly int bytesToRead;

        public bool EndOfStream
        {
            get { return reader.BaseStream.Position == reader.BaseStream.Length; }
        }

        public void Dispose()
        {
            reader.Close();
        }

        public DataReader(Stream stream)
        {
            this.stream = new MemoryStream();

            using (stream)
            {
                stream.CopyTo(this.stream);
            }

            this.stream.Seek(0, SeekOrigin.Begin);

            reader = new BinaryReader(this.stream);
            
            //bytesToRead = BinaryDatapoint.GetByteCount<T>();
        }

        public T Read()
        {
            T datapoint = Activator.CreateInstance<T>();

            //datapoint.Load(reader.ReadBytes(bytesToRead));

            return datapoint;
        }

        public object ReadField(int index) { return 0; }

        public byte[] ReadBytes(int count)
        {
            return reader.ReadBytes(24);
        }
        public short ReadInt16() { return reader.ReadInt16(); }

        public int ReadInt32() { return reader.ReadInt32(); }

        public long ReadInt64() { return reader.ReadInt64(); }

        public uint ReadUInt() { return reader.ReadUInt32(); }

		public byte[] ReadHeader(){
            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            int count = ReadInt32();

            return ReadBytes(count * 24);
		}

        public IEnumerable<FlightHeader> ReadFlightHeaders()
        {
            var head = ReadHeader();

            int count = head.Length / 24;

            for (int i = 0; i < count; i++)
            {
                yield return new FlightHeader()
                {
                    Flight = head.GetInt32(i * 24 + 0),
                    Count = head.GetInt32(i * 24 + 4),
                    Start = DateTime.FromBinary(head.GetInt64(i * 24 + 8)),
                    End = DateTime.FromBinary(head.GetInt64(i * 24 + 16))
                };
            }

            yield break;
        }

        public FlightHeader ReadFlightHeader(int index)
        {
            reader.BaseStream.Seek(index * 24 + 4, SeekOrigin.Begin);

            return new FlightHeader()
            {
                Flight = ReadInt32(),
                Count = ReadInt32(),
                Start = new DateTime(ReadInt64()),
                End = new DateTime(ReadInt64())
            };
        }

        public IEnumerable<T> ReadToEnd()
        {
			var count = reader.ReadInt32 ();

			reader.BaseStream.Seek (count * 3 * sizeof(long), SeekOrigin.Current);
            //var count = stream.Length / bytesToRead;

            Console.WriteLine(count);

            while (count > 0)
            {
                count--;

                yield return Read();
            }

            yield break;
        }
    }

}
