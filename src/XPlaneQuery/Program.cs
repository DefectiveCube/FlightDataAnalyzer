using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using XPlaneGenConsole;

namespace XPlaneQuery
{
    public class Program
    {
        public static void Main(string[] args)
        {
			//Console.WriteLine ("query v1.0");


			// 1. Select Data Source
				// 1. Engine
				// 2. Flight
				// 3. System
			// 2. Projection
			// 3. Selection
			// 4. Execute Query
			// 5. Change Output
			// 6. Exit


			//Map<SystemDatapoint,XPlaneDatapoint>.Associate ("AirTemperature", "Temperature");

			Fahrenheit f = 10.0;

			var list = new List<Expression> ();

			list.Add (QueryBuilder.Build ("3 + 4"));
			list.Add (QueryBuilder.Build ("3 - 4"));
			list.Add (QueryBuilder.Build ("3 * 4"));
			list.Add (QueryBuilder.Build ("3 / 4"));
			list.Add (QueryBuilder.Build ("3 % 4"));
				
			list.Add (QueryBuilder.Build ("3 * (4 + 2)"));

			foreach (var exp in list) {
				Console.WriteLine (exp.ToString ());
			}
				
			Console.WriteLine (QueryBuilder.Build ("3 + 4", typeof(FlightDatapoint),typeof(EngineDatapoint)));

			//var f = new FlightDatapoint (new byte[]{ });
			var name = "P_FLIGHT.CSV";
			var output = "Test.bin";

            var import = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "Import");
            var data = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "data");

            var importFile = Path.Combine(import, name);
            var dataFile = Path.Combine(data, output);

			var start = DateTime.Now;
			using (var reader = new CSVReader<FlightDatapoint> (File.OpenRead (importFile)))
			using (var writer = new DataWriter<FlightDatapoint> (dataFile)) {
				writer.Write (reader.ReadToEnd ().ToArray ());
			}

			//Console.WriteLine (DateTime.Now.Subtract (start).TotalSeconds);

			/*using (var reader = new DataReader<FlightDatapoint> (File.OpenRead (dataFile))) {
				
			}*/





			/*
            Dictionary<int, int> flightIndex = new Dictionary<int, int>();

            using(var reader2 = new FlightDataReader<FlightDatapoint>(File.OpenRead(dataFile)))
            {
                var flights = reader2.ReadFlightHeaders();

				int offset = flights.Count () * 24 + 4;

				foreach (var f in flights) {
					flightIndex.Add (f.Flight, offset);

					offset += f.Count * FlightDatapoint.BYTES_COUNT;
				}
            }
*/

			//var tempConv = "f => (f - 32) * 5 / 9";
            Console.ReadLine();

            /*if (!File.Exists(file))
            {
                Console.WriteLine("File not found");
                Console.ReadLine();
                return;
            }*/

            //var stream = File.OpenRead(file);

            //Console.WriteLine("File Size: {0} bytes", stream.Length);

            //Console.WriteLine("Reading");

            //var reader = new FlightDataReader<FlightDatapoint>(File.OpenRead(file));
            //var reader = new FlightCSVReader<FlightDatapoint>(File.OpenRead(file));

            //var query = new QueryExpression<FlightDatapoint>();

            //Console.WriteLine("Processing Data");

            //query.Data = reader.ReadToEnd().ToArray();

            //Console.WriteLine("Calculating");

            //var results = query.Results().ToArray();

            //Console.WriteLine(results.Count());
            //Console.WriteLine(DateTime.Now.Subtract(start).TotalSeconds);

            /*var f = new Flight<EngineDatapoint, FlightDatapoint, SystemDatapoint>();
			var importDir = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "import");

            f.Import(
                Path.Combine(importDir, "P_ENGINE.csv"),
                Path.Combine(importDir, "P_FLIGHT.csv"),
                Path.Combine(importDir, "P_SYSTEM.csv")
                );

            //f.Load()

			Func<FlightDatapoint,TimeSpan> duration = a => a.DateTime.Subtract (DateTime.Now);
            */
			//Console.WriteLine("Done");
            //Console.ReadLine();
        }
    }
}
