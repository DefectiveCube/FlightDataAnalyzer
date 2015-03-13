using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XPlaneGenConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var validKeys = new[] { "-e", "-f", "-s" };
            FileProcessor<FlightDatapoint> flightProcessor = new FileProcessor<FlightDatapoint>();
            FileProcessor<EngineDatapoint> engineProcessor = new FileProcessor<EngineDatapoint>();
            FileProcessor<SystemDatapoint> systemProcessor = new FileProcessor<SystemDatapoint>();
            
            Console.WriteLine("Flight Data Analyzer: Generator v0.01");

            if(args.Length % 2 != 0 || args.Length < 2)
            {
                //throw new ArgumentException("Invalid number of arguments");
                Console.WriteLine("Invalid number of arguments");
            }

            foreach(var arg in ProcessArguments(args))
            {
                if (!File.Exists(arg.Value))
                {
                    Console.WriteLine("Warning: '{0}' does not exist", arg.Value);
                    continue;
                }

                switch (arg.Key)
                {
                    case "-e":
                        engineProcessor.Add(arg.Value);
                        break;
                    case "-f":
                        flightProcessor.Add(arg.Value);
                        break;
                    case "-s":
                        systemProcessor.Add(arg.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(arg.Key);
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
