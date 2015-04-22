using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XPlaneGenConsole;

namespace XPlaneQuery
{
    public class Program
    {
        public static void Main(string[] args)
        {
			var name = "P_FLIGHT.CSV";
            var import = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "Import");
            var data = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "data");
            var output = "Test.bin";

            var importFile = Path.Combine(import, name);
            var dataFile = Path.Combine(data, output);

            using (var reader = new FlightCSVReader<FlightDatapoint>(File.OpenRead(importFile)))
            using (var writer = new FlightDataWriter<FlightDatapoint>(dataFile))
            {
                writer.Write(reader.ReadToEnd().ToArray());
            }

            Dictionary<int, int> flightIndex = new Dictionary<int, int>();

            using(var reader2 = new FlightDataReader<FlightDatapoint>(File.OpenRead(dataFile)))
            {
                var flights = reader2.ReadFlightHeaders();


            }

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
