using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
	/// <summary>
	/// For reading CSV files
	/// </summary>
	/// <typeparam name="T"></typeparam>
    [Obsolete()]
	public class CSVReader<T> : IDisposable
		where T : CsvDatapoint<T>, new()
	{
		private BufferedStream bs;
		//private FileStream stream;
		private StreamReader reader;

		public bool EndOfStream {
			get { return reader.BaseStream.Position >= reader.BaseStream.Length; }
		}

		public CSVReader(FileStream stream)
		{
//			this.stream = stream;

			bs = new BufferedStream (stream);
			reader = new StreamReader (bs);
			reader.BaseStream.Seek(0, SeekOrigin.Begin);
			reader.ReadLine(); // skip first line
		}

        public void Dispose()
        {
			bs.Close ();
            reader.Close();
            //stream.Close();

			bs = null;
            reader = null;
            //stream = null;
        }

		/*public int ReadInt32(int min = 0, int max = 1)
		{
			int value;

			if (ReadField(min, max).TryParse(out value))
			{
				throw new InvalidCastException();
			}

			return value;
		}*/

        /*public Tuple<int,string> ReadFieldWithIndex(int min = 0, int max = 1)
        {
            throw new NotSupportedException();

            //return new Tuple<int, string>(0, string.Empty);
        }*/

		/*public string ReadField(int min = 0, int max = 1)
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
					reader.Load();
					break;
				}

				sb.Append((char)reader.Load());
			}

			int p = reader.Peek();

            // read through any commas and control characters
			while(p == 44 || p < 32)
			{
				reader.Load();
				p = reader.Peek();
			}

			return sb.ToString();
		}*/

		public T ReadLine()
		{
			T datapoint = Activator.CreateInstance<T>();
            
			datapoint.Load(reader.ReadLine());

			return datapoint.IsValid ? datapoint : null;
		}

		public IEnumerable<T> ReadToEnd()
		{
			var start = DateTime.Now;;
            var list = new List<T>();

			using (reader) {
				while (!reader.EndOfStream) {
                    T datapoint = CsvDatapoint<T>.Factory.CreateFromString(reader.ReadLine());

                    if (datapoint.IsValid)
                    {
                        yield return datapoint;
                    }				
				}
			}

			Console.WriteLine ("Read: {0}",DateTime.Now.Subtract (start).TotalSeconds);
            yield break;
		}
	}
}