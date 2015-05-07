using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XPlaneGenConsole;

namespace XPlaneQuery
{   
    public class QueryProgram
    {
        public static void Main(string[] args)
        {


            //var exp = QueryBuilder.Query("9.81 * a").Compile();

            //Console.WriteLine(exp(10));
            
            //Console.WriteLine(exp.ToString());
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

            //Conversion.Convert<FlightDatapoint, FlightCsvDatapoint>(new FlightCsvDatapoint[] { });

			var name = "P_FLIGHT.CSV";
			var output = "Test.bin";

            var import = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "Import");
            var data = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "data");

            var importFile = Path.Combine(import, name);
            var dataFile = Path.Combine(data, output);

			var start = DateTime.Now;

            using (var reader = new CSVReader<FlightCsvDatapoint>(File.OpenRead(importFile)))
            {
                var parser = CsvParser.GetParser<FlightDatapoint, FlightCsvDatapoint>();
                List<FlightDatapoint> points = new List<FlightDatapoint>();

                foreach (var flightData in reader.ReadToEnd().ToArray())
                {
                    if (flightData.IsValid)
                    {
                        var _data = flightData.Value.ToArray();
                        var pt = new FlightDatapoint();
                        pt.IsValid = true;
                        parser(pt, _data);
                        points.Add(pt);
                    }
                }
            }
            Console.WriteLine(DateTime.Now.Subtract(start).TotalSeconds);
			//using (var writer = new DataWriter<FlightDatapoint> (dataFile)) {
                //reader.ReadToEnd().ToArray();
				//writer.Write (reader.ReadToEnd ().ToArray ());
			//}

            /*using (var reader = new DataReader<FlightDatapoint> (File.OpenRead (dataFile))) {
				
			}*/

            Console.ReadLine();
        }
    }
}
