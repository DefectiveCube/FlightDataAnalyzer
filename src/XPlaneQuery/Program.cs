using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Policy;
using System.Security.Permissions;
using System.Threading;
using XPlaneGenConsole;
using System.Runtime.Remoting;
using System.Reflection;

namespace XPlaneQuery
{
    public class QueryProgram
    {
        public static void Main(string[] args)
        {
            /* Load Assemblies into Partially Trusted Sandbox */

            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = @"H:\repos\XPlaneGen\src\default\bin\Debug";

            var permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read, @"J:\Kirk\Documents")); // Change to import folder of application
            //permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Write, @"J:\Kirk\Documents")); // Change to data folder of application (which should not be easily discovered by a user)

            var sandbox = AppDomain.CreateDomain("UntrustedCodeExecutionSandbox", null, setup, permissions);

            var handle = Activator.CreateInstanceFrom(sandbox, typeof(UntrustedCodeExecution).Assembly.ManifestModule.FullyQualifiedName, typeof(UntrustedCodeExecution).FullName);

            UntrustedCodeExecution instance = (UntrustedCodeExecution)handle.Unwrap();

            instance.DoNothing();
            instance.DisplayAssemblies();

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

			var name = "P_FLIGHT.CSV";
			var output = "Test.bin";

            var import = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "Import");
            var data = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "data"); // this needs to be moved to a hidden folder

            var importFile = Path.Combine(import, name);
            var dataFile = Path.Combine(data, output);

            var parser = CsvParser.GetParser<FlightDatapoint, FlightCsvDatapoint>();
            List<FlightDatapoint> points = new List<FlightDatapoint>();

            var start = DateTime.Now;

            // Read CSV data into Binary Datapoints
            // TODO: use
            using (var reader = new CSVReader<FlightCsvDatapoint>(File.OpenRead(importFile)))
            {
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

            // Write Binary Datapoints to storage
            //var dpWriter = new DatapointWriter<FlightDatapoint>();

            // Read Binary Datapoints from storage
            

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
