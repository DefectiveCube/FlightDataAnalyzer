using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Policy;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using XPlaneGenConsole;
using System.Runtime.Remoting;
using System.Reflection;

namespace XPlaneQuery
{
    public class QueryProgram
    {
        private static AppDomain sandbox;
        private static UntrustedCodeExecution instance;
        private static List<Type> DatapointTypes = new List<Type>();
        private static List<Type> SelectedTypes = new List<Type>();
        private static List<PropertyInfo> ProjectionFields = new List<PropertyInfo>();
        private static List<PropertyInfo> SelectionFields = new List<PropertyInfo>();

        private static void InitializeSandbox()
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = @"J:\Kirk\Documents\FlightDataAnalyzer\lib";

            var permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.AllAccess, @"J:\Kirk\Documents"));
//            permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Write, @"J:\Kirk\Documents")); // Change to data folder of application (which should not be easily discovered by a user)

            sandbox = AppDomain.CreateDomain("UntrustedCodeExecutionSandbox", null, setup, permissions);           

            var handle = Activator.CreateInstanceFrom(sandbox, typeof(UntrustedCodeExecution).Assembly.ManifestModule.FullyQualifiedName, typeof(UntrustedCodeExecution).FullName);

            instance = (UntrustedCodeExecution)handle.Unwrap();

            Console.WriteLine("Loading models");
            instance.LoadAssemblies();
        }

        static void LoadAssembly(string name)
        {
            // This isn't ideal, because this loads the assembly on the main appdomain. This also loads code in the current security context, which is probably too much. This is potentially dangerous
            var asm = AppDomain.CurrentDomain.Load(name);

            // Register datapoint types
            var attrs = asm.GetCustomAttributes<DatapointAttribute>();

            foreach (var attr in attrs)
            {
                DatapointTypes.Add(attr.Type);
                Console.WriteLine("Added Datapoint Type: {0}", attr.Type.FullName);
            }
        }


        static void MainMenu()
        {
            // Force load of assembly
            LoadAssembly("Prototype");

            Console.Clear();
            Console.SetWindowSize(120, 40);
            Console.SetBufferSize(120, 40);

            var sb = new StringBuilder();

            sb.Append(new string('*', 120));
            sb.AppendFormat("*{0}*", new string(' ', 118));
            sb.Append("* Query v1.0 [BETA]");
            sb.Append(new string(' ', 100));
            sb.Append('*');
            sb.AppendFormat("*{0}*", new string(' ', 118));
            sb.AppendLine(new string('*', 120));
            sb.AppendLine("1. Select Data Models");
            sb.AppendLine("2. Edit Projection Query");
            sb.AppendLine("3. Edit Selection Query");
            sb.AppendLine("4. Execute Query");
            sb.AppendLine("5. Enter Querystring [Advanced]");
            //sb.AppendLine("6. Generate Approximation Functions");
            sb.AppendLine("6. Exit");
            sb.AppendLine();

            int option = 0;
            bool keepRunning = true;
            while (keepRunning)
            {
                Console.Write(sb.ToString());

                if (int.TryParse(Console.ReadLine(), out option))
                {
                    switch (option)
                    {
                        case 1:
                            Menu_SelectDataModels();
                            continue;
                        case 2:
                            continue;
                        case 3:
                            continue;
                        case 4:
                            continue;
                        case 5:
                            continue;
                        case 6:
                            keepRunning = false;
                            break;
                        default:
                            continue;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.Write(sb.ToString());
                }
            }
        }

        static void Menu_SelectDataModels()
        {
            Console.Clear();

            Dictionary<int, string> typesToAdd = new Dictionary<int, string>();

            var sb = new StringBuilder();

            if (DatapointTypes.Count > 0)
            {
                foreach (var str in DatapointTypes.Select((t, i) => new
                {
                    Id = i,
                    Name = t.FullName
                }))
                {
                    sb.AppendFormat("{0}. {1}", str.Id + 1, str.Name);
                    sb.AppendLine();
                }
            }
            else
            {
                Console.WriteLine("No Models Loaded");
                Console.ReadLine();
                return;
            }

            sb.AppendFormat("{0}. Clear Selections", DatapointTypes.Count + 1);
            sb.AppendLine();
            sb.AppendFormat("{0}. Return to Main Menu", DatapointTypes.Count + 2);
            sb.AppendLine();

            int number = 0;

            while (true)
            {
                Console.Clear();
                Console.Write(sb.ToString());

                if (int.TryParse(Console.ReadLine(), out number))
                {
                    if (number > -1 && number <= DatapointTypes.Count && !typesToAdd.ContainsKey(number))
                    {
                        typesToAdd.Add(number, DatapointTypes.ElementAt(number - 1).Name);
                        continue;
                    }
                    else if (number == DatapointTypes.Count + 1)
                    {
                        typesToAdd.Clear();
                        continue;
                    }
                    else if (number == DatapointTypes.Count + 2)
                    {
                        // Exit to main menu
                        break;
                    }
                    else
                    {
                        // Already added or out-of-range
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }


            SelectedTypes.Clear();
            SelectedTypes.AddRange(DatapointTypes.Where(d => typesToAdd.Values.Contains(d.Name)));

            Console.Clear();
        }

        static void Menu_ExecuteQuery()
        {
            Console.Clear();

            var sb = new StringBuilder();

            sb.AppendLine("1. Export as XPlane FDR");
            sb.AppendLine("2. Export as CSV");
            sb.AppendLine("3. Export as XML");
            sb.AppendLine("4. Back to Main Menu");
            sb.AppendLine();
            sb.Append("Enter your choice: ");

            Console.Write(sb.ToString());

            Console.ReadLine();
        }

        static void ParseCSV(string file)
        {
            var name = "P_FLIGHT.CSV";
            var output = "Test.bin";

            var import = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "Import");
            var data = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "data"); // this needs to be moved to a hidden folder

            var importFile = Path.Combine(import, name);
            var dataFile = Path.Combine(data, output);

            //CsvConverter<FlightDatapoint>.Load(importFile, dataFile);
        }

        public static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            //CsvConverter.Load ("/Users/Kirk/FlightDataAnalyzer/Import/P_FLIGHT.CSV");

            /* Load Assemblies into Partially Trusted Sandbox */


            // Permissions dont seem to want to work, despite all access :\
            //Console.WriteLine("Initializing sandbox");
            //Console.WriteLine("Please wait");
            //InitializeSandbox();

            // TODO: index already parsed data, for easy access

            // TODO: pause execution until data is loaded

            MainMenu();




            //var readSomething = DatapointReader<FlightDatapoint>.GetReadAction();
            //var writeSomething = BinaryDatapoint.GetReadAction<FlightDatapoint>();

            /*using (BinaryWriter writer = new BinaryWriter(File.OpenWrite("test.bin")))
            {
                var fp = new FlightDatapoint()
                {
                    Flight = int.MaxValue,
                    AHRSSeq = byte.MaxValue,
                    DateTime = DateTime.Now
                };

                writeSomething(fp, writer);
            }

            using (var _reader = new BinaryReader(File.OpenRead("test.bin")))
            {
                var _dp = readSomething(_reader);
            }*/
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

            /*
            Console.WriteLine ("What type of file?");

            var sb = new StringBuilder ();
            sb.AppendLine ("1. Engine");
            sb.AppendLine ("2. Flight");
            sb.AppendLine ("3. System");

            int option = 0;

            Console.Write (sb.ToString ());

            while (!int.TryParse (Console.ReadLine (), out option) && option < 1 || option > 3) {
                Console.Clear ();
                Console.Write (sb.ToString ());
            }

            Console.WriteLine ("Enter path of file:");
            var path = Console.ReadLine ();

            while (path != string.Empty && !File.Exists (path)) {
                Console.Clear ();
                Console.WriteLine ("Enter path of file:");
                path = Console.ReadLine();
            }

            Console.Clear ();

            var info = new FileInfo (path);

            Console.WriteLine ("Reading from {0}", info.FullName);
            */
            /*List<BinaryDatapoint> points = new List<BinaryDatapoint> ();
            dynamic parser;
            dynamic reader;

            switch (option) {
            case 1:
                parser = CsvParser.GetParser<EngineDatapoint,EngineCsvDatapoint> ();
                reader = new CSVReader<EngineCsvDatapoint> (File.OpenRead (path));
                break;
            case 2:
                parser = CsvParser.GetParser<FlightDatapoint,FlightCsvDatapoint> ();
                reader = new CSVReader<FlightCsvDatapoint> (File.OpenRead (path));
                break;
            case 3:
                parser = CsvParser.GetParser<SystemDatapoint, SystemCsvDatapoint> ();
                reader = new CSVReader<SystemCsvDatapoint> (File.OpenRead (path));
                break;
            default:
                throw new ArgumentOutOfRangeException ();
            }*/


            var name = "P_FLIGHT.CSV";
            //var name = path;
            var output = "Test.bin";

            var import = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "Import");
            var data = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "data"); // this needs to be moved to a hidden folder

            var importFile = Path.Combine(import, name);
            var dataFile = Path.Combine(data, output);


            return;

            var start = DateTime.Now;
            // Load CSV data into Binary Datapoints
            //using (var reader = new CSVReader<FlightCsvDatapoint>(File.OpenRead(importFile)))
            {
                // This is quite fast (800K records parsed into datapoints in 6 seconds on a I7 4790, and 10+ seconds on a MBP (early-2011) with Mono C#). However, it soaks up ~1GB of RAM!!! Yikes! 
                // Not good efficiency when the file is 130 MB
                // The culprit here is that there isn't a true requirement to have ALL of the data stored in memory (as it is just going straight to secondary storage) in-between read and write processes.

                /*var ignore = reader.ReadToEnd ()
                    .ToArray()
                    .AsParallel ()
                    .Where (r => r.IsValid)
                    .Select (a => {
                        var pt = new FlightDatapoint ();
                        parser (pt, a.Value.ToArray ());
                        return pt;
                    })	
                    .OrderBy (f => f.Flight)
                    .ToArray ();

                Console.WriteLine("PLINQ: {0}", ignore.Count());*/

                // Producer-Consumer Model
                // The producer is the reader, which pushs each line to the queue. Since the reader is reading a file, there is no benefit of parallelization.
                // The consumer is the writer, which writes each datapoint to the output binary file. This is also not going to benefit from parallelization.
                // However, the parsing operation that takes string values and parses them all into values, can be parallelized.
                // After profiling the time it takes to parse text into datapoints, it's clear that this process is CPU-bound, not IO-bound as the parsing operation takes ~90% of the time when run sequentially.
                // Therefore, the optimal performance is found by increasing the amount of threads until either the reader or writer cannot keep up.

                // The producer should keep reading values into the queue
                // There is X amount of threads that will each do the work of parsing string values into a datapoint

                // Data should be sorted by flight, and then date (earliest to latest). Obviously, the sorting operation will take additional time. However, data will be coming in already ordered so the only variance will be
                // from the timing differences of each thread. This means that the probability of getting data in the worst-case is almost none, and in-fact will likely be near or relatively close to optimal.

                // For a not-sorting read+parse, the total operation takes between 2-3 seconds! This is using threads and not PLINQ, but isn't sorted. It also only used 375 MB of RAM!



                /*				foreach (var flightData in reader.ReadToEnd().ToArray())
                                {
                                    if (flightData.IsValid)
                                    {
                                        var _data = flightData.Value.ToArray();
                                        var pt = new FlightDatapoint();
                                        //pt.IsValid = true;
                                        parser(pt, _data);
                                        //points.Add(pt);
                                    }
                                }*/
            }
            //onsole.WriteLine(DateTime.Now.Subtract(start).TotalSeconds);


            // Write Binary Datapoints to storage
            //var dpWriter = new DatapointWriter<FlightDatapoint>();

            // Load Binary Datapoints from storage


            //using (var writer = new DataWriter<FlightDatapoint> (dataFile)) {
            //reader.ReadToEnd().ToArray();
            //writer.Write (reader.ReadToEnd ().ToArray ());
            //}

            /*using (var reader = new DataReader<FlightDatapoint> (File.OpenRead (dataFile))) {
				
			}*/

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();

        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.Clear();
            Console.WriteLine("Closing");
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(300);
                Console.Write(".");
            }
        }
    }
}
