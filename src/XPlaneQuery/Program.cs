using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using XPlaneGenConsole;
using XPlaneGenConsole.Data;

namespace XPlaneQuery
{
    public class QueryProgram
    {
        public static void Main(string[] args)
        {
			//var t1 = Term.Parse ("2x", "x");
			//var t2 = Term.Parse ("3x", "x");
			//var t3 = t1 * t2;

			//var t = Equation.Parse ("4*x^2 + 3*x^2", "x");
			//var s = Equation.Parse ("4*x^2 + 3*x^3 * 4*x^2 + 3*x^3", "x");
			//var s = Equation.Parse ("4 + 3", "x");
			//var r = Equation.Parse ("(x) * (1)", "x");

			//r = r.ToGeneralForm ();

			//var qt = Equation.Parse ("(x-1) * 4", "x");
			//var e = Equation.Parse ("(x-1) * (x-2) * ((x-1) * (x-2))", "x");
			var q = Equation.Parse ("(x-1) * (x-2) * (x-1)", "x");

			//q = q.ToGeneralForm ();
			//var eq = Equation.Parse ("(x-1) * (x-3) * (x-4) * (x-6)", "x");

			//var eq1 = eq.ToGeneralForm ();

			//var exp = eq1.CompiledExpression.Compile ();

			/*Console.WriteLine (exp.ToString ());

			foreach (var num in Enumerable.Range(1,6)) {
				Console.WriteLine (exp (num));
			}*/


			//var eq = Equation.Parse ("x^2 * x^2", "x");
			// (x-1) * (x-3) * (x-4) * (x-6)

			/*
			 * (x-1) * (x-3) = x^2 -4x 3
			 * 
			 * (x-4) * (x-6) = x^2 -10x 24
			 */ 


			// x^4 	-10x^3 	24x^2
			// 		-4x^3 	40x^2 	-96x
			// 				3x^2 	-30x 	72

			// x^4 -14x^3 67x^2 -126x 72


			/*
			(x^2 -4x 3) * (x-4) = x^3 -4x^2 3x -4x^2 -16x -12

				x^3 -8x^2 -13x -12 * (x-6)


				x^4 -8x^3 -13x^2 -12x
				-6x^3 48x^2 78x 72

				x^4 -14x^3 35x^2 66x 72

			*/
/*			Term a = new Term () {
				Coefficient = 1,
				Identifier = "x",
				Exponent = 2
			};

			Console.WriteLine (a.GetExpression ().ToString ());

			var func = a.GetExpression ().Compile ();

			var result = func (2);*/
			//Term f = new Term (){ Coefficient = 24, Identifier = "x", Exponent = 0 };
				
			//Console.WriteLine ((a * d).ToString ());
			//Console.WriteLine ((b * d).ToString ());
			//Console.WriteLine(


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

			//Fahrenheit f = 10.0;
            //Celsius c = f;

			var list = new List<Expression> ();

			/*list.Add (QueryBuilder.Build ("3 + 4"));
			list.Add (QueryBuilder.Build ("3 - 4"));
			list.Add (QueryBuilder.Build ("3 * 4"));
			list.Add (QueryBuilder.Build ("3 / 4"));
			list.Add (QueryBuilder.Build ("3 % 4"));
				
			list.Add (QueryBuilder.Build ("3 * (4 + 2)"));

			foreach (var exp in list) {
				Console.WriteLine (exp.ToString ());
			}
				
			Console.WriteLine (QueryBuilder.Build ("3 + 4", typeof(FlightDatapoint),typeof(EngineDatapoint)));*/

			var name = "P_FLIGHT.CSV";
			var output = "Test.bin";

            var import = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "Import");
            var data = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "data");

            var importFile = Path.Combine(import, name);
            var dataFile = Path.Combine(data, output);

            Conversion<FlightCsvDatapoint, FlightDatapoint> con = new Conversion<FlightCsvDatapoint, FlightDatapoint>();
            //con.Add(new delegate(Extensions.AsInt))
            


			var start = DateTime.Now;
			using (var reader = new CSVReader<FlightCsvDatapoint> (File.OpenRead (importFile)))
			using (var writer = new DataWriter<FlightDatapoint> (dataFile)) {
                //reader.ReadToEnd().ToArray();
				//writer.Write (reader.ReadToEnd ().ToArray ());
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
