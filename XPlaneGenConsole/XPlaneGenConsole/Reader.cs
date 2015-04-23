using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    /// <summary>
    /// For reading binary types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FlightDataReader<T> : IDisposable
        where T : Datapoint<T>
    {
        private BinaryReader reader;
        private MemoryStream stream;
        private readonly int bytesToRead;

        public bool EndOfStream
        {
            get { return reader.BaseStream.Position == reader.BaseStream.Length; }
        }

        public void Dispose()
        {
            reader.Close();
        }

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
                bytesToRead = FlightDatapoint.BYTES_COUNT;
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

    /// <summary>
    /// For reading CSV files
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FlightCSVReader<T> : TextReader 
		where T : Datapoint<T>
    {
		private MemoryStream stream;
        private StreamReader reader;

        private Func<T> Create = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();

        public bool EndOfStream
        {
            get { return reader.BaseStream.Position >= reader.BaseStream.Length; }
        }

        public FlightCSVReader(Stream stream)
        {
			this.stream = new MemoryStream ();

            stream.CopyTo(this.stream);

            reader = new StreamReader(this.stream);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            reader.ReadLine(); // skip first line
        }

        /*public Task<IEnumerable<T>> ReadToEndAsync()
        {
            await reader.ReadToEndAsync();
        }*/

        public int ReadInt32(int min = 0, int max = 1)
        {
            int value;

            if (ReadField(min, max).TryParse(out value))
            {
                throw new InvalidCastException();
            }

            return value;
        }

        public string ReadField(int min = 0, int max = 1)
        {
            if (min < 0)
                throw new ArgumentOutOfRangeException("min");

            if (max < 1)
                throw new ArgumentOutOfRangeException("max");

            if(max < min)
                max = min;

            var sb = new StringBuilder(max);

            if (min == 0)
            {
                if (reader.Peek() == ',')
                {
                    return string.Empty;
                }
            }
            else
            {
                var block = new char[min];

                reader.ReadBlock(block, 0, min);

                sb.Append(block);
            }

            for (int i = min + 1; i <= max; i++)
            {
                if(reader.Peek() == ',')
                {
                    reader.Read();
                    break;
                }

                sb.Append((char)reader.Read());
            }

            int p = reader.Peek();

            while(p == 44 || p < 32)
            {
                reader.Read();
                p = reader.Peek();
            }

            return sb.ToString();
        }

        public async new Task<T> ReadLineAsync()
        {
            var line = await reader.ReadLineAsync();

            T datapoint = Activator.CreateInstance<T>();

            datapoint.Load(line);

            return datapoint.IsValid ? datapoint : null;
        }

        public new T ReadLine()
        {
            T datapoint = Activator.CreateInstance<T>();

            datapoint.Load(reader.ReadLine());

            if (datapoint.IsValid)
            {
                return datapoint;
            }

            return datapoint.IsValid ? datapoint : null;
        }

        public async new Task<T[]> ReadToEndAsync()
        {
            List<T> points = new List<T>();
            T datapoint;

            using (reader)
            {
                while (!reader.EndOfStream)
                {
                    datapoint = Activator.CreateInstance<T>();

                    await datapoint.LoadAsync(reader.ReadLineAsync().Result);

                    if (datapoint.IsValid)
                    {
                        points.Add(datapoint);
                    }
                }
            }

            return points.ToArray();
        }

        public new IEnumerable<T> ReadToEnd()
		{
			var start = DateTime.Now;;

			using (reader) {
				while (!reader.EndOfStream) {
                    T datapoint = Activator.CreateInstance<T> ();

					datapoint.Load (reader.ReadLine ());

					if (datapoint.IsValid) {
						yield return datapoint;
					}
				}
			}

			Console.WriteLine ("Read: {0}",DateTime.Now.Subtract (start).TotalSeconds);
			yield break;
		}
    }
}
