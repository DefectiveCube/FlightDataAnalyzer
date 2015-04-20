﻿using System;
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
            var start = DateTime.Now;

            var name = "P_FLIGHT.CSV";//"test.bin";
            var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "Import", name);

            if (!File.Exists(file))
            {
                Console.WriteLine("File not found");
                Console.ReadLine();
                return;
            }

            var stream = File.OpenRead(file);

            Console.WriteLine("File Size: {0} bytes", stream.Length);

            Console.WriteLine("Reading");

            //var reader = new FlightDataReader<FlightDatapoint>(File.OpenRead(file));
            var reader = new FlightCSVReader<FlightDatapoint>(File.OpenRead(file));

            var query = new QueryExpression<FlightDatapoint>();

            Console.WriteLine("Processing Data");

            query.Data = reader.ReadToEnd().ToArray();

            Console.WriteLine("Calculating");

            var results = query.Results().ToArray();

            Console.WriteLine(results.Count());
            Console.WriteLine(DateTime.Now.Subtract(start).TotalSeconds);
            Console.ReadLine();
        }
    }
}