using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public class Flight
    {

    }

    public class Flight<T> : Flight
        where T : Datapoint<T>
    {
        IEnumerable<T> data;

        public void WriteTo(string path)
        {

        }
    }

    public class Flight<T, U> : Flight
        where T : Datapoint<T>
        where U : Datapoint<U>
    {
        IEnumerable<T> data1;
        IEnumerable<U> data2;
    }

    public class Flight<T, U, V> : Flight
        where T : Datapoint<T>
        where U : Datapoint<U>
        where V : Datapoint<V>
    {
        IEnumerable<T> data1;
        IEnumerable<U> data2;
        IEnumerable<V> data3;

        FlightDataReader<T> reader1;
        FlightDataReader<U> reader2;
        FlightDataReader<V> reader3;

        XPlaneFileMapping<T, U, V> Map { get; set; }

        public void Import(string path1, string path2, string path3)
        {
            var import1 = new FlightCSVReader<T>(File.OpenRead(path1));
            var import2 = new FlightCSVReader<U>(File.OpenRead(path2));
            var import3 = new FlightCSVReader<V>(File.OpenRead(path3));

            data1 = import1.ReadToEnd().ToArray();
            data2 = import2.ReadToEnd().ToArray();
            data3 = import3.ReadToEnd().ToArray();

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
        }

        public void Load(string path1, string path2, string path3)
        {
            reader1 = new FlightDataReader<T>(File.OpenRead(path1));
            reader2 = new FlightDataReader<U>(File.OpenRead(path2));
            reader3 = new FlightDataReader<V>(File.OpenRead(path3));

            data1 = reader1.ReadToEnd();
            data2 = reader2.ReadToEnd();
            data3 = reader3.ReadToEnd();

            var flights1 = (from a in data1
                            select a.Flight).Distinct();
            var flights2 = (from b in data2
                            select b.Flight).Distinct();
            var flights3 = (from c in data3
                            select c.Flight).Distinct();
                
        }
        
        public void WriteTo(string path)
        {

        }
    }
}
