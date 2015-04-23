using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
	public static class FlightExtensions
	{
		public static IEnumerable<T> DoNothing<T>(this IEnumerable<T> stuff) 
			where T : Datapoint<T>
		{
			return new T[]{ };
		}
	}

    public class Flight { }

    public class Flight<T> : Flight
        where T : Datapoint<T>
    {
		public IEnumerable<T> Data1 { get; protected set; }

		public FlightDataReader<T> Reader1{ get; protected set; }

		XPlaneFileMapping<T> Map { get; set; }

		/// <summary>
		/// Import CSV data
		/// </summary>
		/// <param name="path">Path.</param>
		public void Import(string path)
		{
			Console.WriteLine ("Importing {0}", path);

			var import = new FlightCSVReader<T> (File.OpenRead (path));

			Data1 = import.ReadToEnd ().ToArray ();

			Console.WriteLine ("Finished Import");

			//Data1.DoNothing ();
		}

		/// <summary>
		/// Load binary data
		/// </summary>
		/// <param name="path">Path.</param>
		public void Load(string path)
		{
			Data1 = Reader1.ReadToEnd ().ToArray ();
		}

        public void WriteTo(string path)
        {

        }
    }

	public class Flight<T, U> : Flight<T>
        where T : Datapoint<T>
        where U : Datapoint<U>
    {
		public IEnumerable<U> Data2 { get; protected set; }

		public FlightDataReader<U> Reader2 {get; protected set;}

		XPlaneFileMapping<T,U> Map { get; set; }

		public void Import(string path1, string path2)
		{
			Import (path1);

			Console.WriteLine ("Importing {0}", path2);

			var import = new FlightCSVReader<U> (File.OpenRead (path2));

			Data2 = import.ReadToEnd ().ToArray ();

			Console.WriteLine ("Finished Import");
		}
	}

	public class Flight<T, U, V> : Flight<T,U>
        where T : Datapoint<T>
        where U : Datapoint<U>
        where V : Datapoint<V>
    {
		public IEnumerable<V> Data3 { get; protected set; }

		public FlightDataReader<T> Reader3 { get; protected set; }

        public XPlaneFileMapping<T, U, V> Map { get; set; }

        public void Import(string path1, string path2, string path3)
        {
			Import (path1, path2);

			Console.WriteLine ("Importing {0}", path3);

			var import = new FlightCSVReader<V> (File.OpenRead (path3));

			Data3 = import.ReadToEnd ().ToArray ();

			Console.WriteLine ("Finished Import");


			/*
            var q1 = from d in data1
                     group d by d.Flight into g
                     select new
                     {
                         Flight = g.Key,
                         Start = (from t2 in g select t2.DateTime).Min(),
                         End = (from t3 in g select t3.DateTime).Max(),
                         Count = (from t4 in g select t4).Count()
                     };

            var q2 = from d in data2
                    group d by d.Flight into g
                    select new
                    {
                        Flight = g.Key,
                        Start = (from t2 in g select t2.DateTime).Min(),
                        End = (from t3 in g select t3.DateTime).Max(),
                        Count = (from t4 in g select t4).Count()
                    };

            var q3 = from d in data3
                     group d by d.Flight into g
                     select new
                     {
                         Flight = g.Key,
                         Start = (from t2 in g select t2.DateTime).Min(),
                         End = (from t3 in g select t3.DateTime).Max(),
                         Count = (from t4 in g select t4).Count()
                     };
                     */
        }

        public void Load(string path1, string path2, string path3)
        {
            //reader1 = new FlightDataReader<T>(File.OpenRead(path1));
            //reader2 = new FlightDataReader<U>(File.OpenRead(path2));
            //reader3 = new FlightDataReader<V>(File.OpenRead(path3));

            //data1 = reader1.ReadToEnd();
            //data2 = reader2.ReadToEnd();
            //data3 = reader3.ReadToEnd();

            var flights1 = (from a in Data1
                            select a.Flight).Distinct();
            var flights2 = (from b in Data2
                            select b.Flight).Distinct();
            var flights3 = (from c in Data3
                            select c.Flight).Distinct();
                
        }
        
        public void WriteTo(string path)
        {
			
        }
    }
}
