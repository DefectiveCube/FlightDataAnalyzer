using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace XPlaneGenConsole
{
    public class ConsoleApp
    {
        const string name = "Flight Data Analyzer";
        const double version = 0.01;

        string[] validKeys = new[] { "-e", "-f", "-s" };

        FileProcessor<FlightDatapoint> flightProcessor = new FileProcessor<FlightDatapoint>();
        FileProcessor<EngineDatapoint> engineProcessor = new FileProcessor<EngineDatapoint>();
        FileProcessor<SystemDatapoint> systemProcessor = new FileProcessor<SystemDatapoint>();

        int Menu()
        {
            var sb = new StringBuilder();

            sb.Append(name);
            sb.Append(": Generator v");
            sb.Append(version);
            sb.AppendLine();

            // Import
            // Associate

            sb.AppendLine();
            sb.AppendLine("1. Import flight data");
            sb.AppendLine("2. Generate FDR file");
            sb.AppendLine("X. Quit");

            Console.WriteLine(sb.ToString());

            int option;

            int.TryParse(Console.ReadLine(), out option);

            return option;
        }

        void BatchImport()
        {

        }

        void ImportData(string[] args = default(string[]))
        {
            var keys = new[] { "engine", "flight", "system" };
            var paths = new Dictionary<string, string>();
            var path = string.Empty;

            foreach (var key in keys)
            {
                while (true)
                {
                    Console.WriteLine("Enter the path of the {0} system file. Enter a blank line to bypass that system", key);

                    path = Console.ReadLine();

                    if(path == string.Empty)
                    {
                        Console.WriteLine("Skipping {0} system file", key);
                        break;
                    }

                    var fileInfo = new FileInfo(path);

                    if (fileInfo.Exists)
                    {
                        paths.Add(key, Console.ReadLine());
                        break;
                    }
                    else
                    {
                        Console.WriteLine("File not found: {0}", fileInfo.FullName);
                        Console.WriteLine();
                    }
                }
            }

            //Debug.Assert(args.Length % 2 == 0 && args.Length > 1, "Invalid number of arguments");

            foreach(var file in paths)
            {
                if (!File.Exists(file.Value))
                {
                    Console.WriteLine("Warning: '{0}' does not exist", file.Value);
                    continue;
                }

                switch (file.Key)
                {
                    case "engine":
                        engineProcessor.Add(file.Value);
                        break;
                    case "flight":
                        flightProcessor.Add(file.Value);
                        break;
                    case "system":
                        systemProcessor.Add(file.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(file.Key);
                }
            }

            engineProcessor.Start();
            systemProcessor.Start();
            flightProcessor.Start();

            engineProcessor.WaitAll();
            systemProcessor.WaitAll();
            flightProcessor.WaitAll();

            Console.WriteLine("Press enter to exit");
            Console.Read();
        }

        public void Run(string[] args)
        {
            var menuOption = Menu();

            switch (menuOption)
            {
                case 1:
                    ImportData(args);
                    break;
                default:
                    break; 
            }
        }


        /// <summary>
        /// Returns string[] as KeyValuePair
        /// </summary>
        static IEnumerable<KeyValuePair<string, string>> ProcessArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i += 2)
            {
                yield return new KeyValuePair<string, string>(args[i].Replace(Environment.NewLine, string.Empty), args[i + 1]);
            }

            yield break;
        }
    }
}
