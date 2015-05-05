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
	public class CSVReader<T> : IDisposable
		where T : CsvDatapoint<T>, new()
	{
		private MemoryStream stream;
		private StreamReader reader;

		public bool EndOfStream {
			get { return reader.BaseStream.Position >= reader.BaseStream.Length; }
		}

		public CSVReader(Stream stream)
		{
			this.stream = new MemoryStream ();

			stream.CopyTo(this.stream);

			reader = new StreamReader(this.stream);
			reader.BaseStream.Seek(0, SeekOrigin.Begin);
			reader.ReadLine(); // skip first line
		}

        public void Dispose()
        {

        }

		public int ReadInt32(int min = 0, int max = 1)
		{
			int value;

			if (ReadField(min, max).TryParse(out value))
			{
				throw new InvalidCastException();
			}

			return value;
		}

        public Tuple<int,string> ReadFieldWithIndex(int min = 0, int max = 1)
        {
            throw new NotSupportedException();

            //return new Tuple<int, string>(0, string.Empty);
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

            // read through any commas and control characters
			while(p == 44 || p < 32)
			{
				reader.Read();
				p = reader.Peek();
			}

			return sb.ToString();
		}

		/*public async new Task<T> ReadLineAsync()
		{
			var line = await reader.ReadLineAsync();

			T datapoint = Activator.CreateInstance<T>();

			datapoint.Load (line);

			return datapoint.IsValid ? datapoint : null;
		}*/

		public T ReadLine()
		{
			T datapoint = Activator.CreateInstance<T>();
            
			datapoint.Load(reader.ReadLine());

			return datapoint.IsValid ? datapoint : null;
		}

		/*public async new Task<T[]> ReadToEndAsync()
		{
			var start = DateTime.Now;

			List<Task<T>> tasks = new List<Task<T>> ();

			List<T> points = new List<T>();

			using (reader)
			{
				while (!reader.EndOfStream)
				{
					var data = reader.ReadLine ();

					var t = Task<T>.Factory.StartNew (() => {
						T datapoint = Activator.CreateInstance<T>();

						datapoint.Load(data);

						return datapoint;
					});

					tasks.Add (t);
				}
			}

			await Task.WhenAll (tasks.ToArray ());

			Console.WriteLine ("Read: {0}", DateTime.Now.Subtract (start).TotalSeconds);

			foreach (var t in tasks) {
				if (t.Status == TaskStatus.RanToCompletion && t.Result.IsValid) {
					points.Add (t.Result);
				}
			}

			return points.ToArray();
		}*/

		/*public IEnumerable<T> ReadAll(){
			var result = ReadToEndAsync ().Result;

			foreach (var t in result) {
				if(t.IsValid){
					yield return t;
				}
			}

			yield break;
		}*/

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