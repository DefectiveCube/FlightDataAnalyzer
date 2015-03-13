using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XPlaneGenConsole;

namespace XPlaneTest
{
    class TestConsole
    {
        static void Main(string[] args)
        {
            var test = new TestConsole();
            test.Run();

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        List<TestField> fields = new List<TestField>();

        public TestConsole()
        {
            var f = new TestField()
            {
                System = "",
                Field = "",
                Type = typeof(int)
            };
        }

        void Run()
        {

        }

        /// <summary>
        /// Create a new test case
        /// </summary>
        void Create()
        {
            // Which datapoint types?
            var sb = new StringBuilder();

            sb.AppendLine("Which systems?");
            sb.AppendLine("1. Engine");
            sb.AppendLine("2. Flight");
            sb.AppendLine("3. System");

            // Which fields?
            sb.AppendLine("1.");

        }

        void Create(EngineFields engine = EngineFields.None, FlightFields flight = FlightFields.None, SystemFields system = SystemFields.None)
        {
            // Field A and Field B
            // Operator or Predicate
        }
        
        void SelectEngineFields()
        {

        }

        void SelectFlightFields()
        {

        }

        void SelectSystemFields()
        {

        }
    }

    class TestDatapoint
    {

    }

    [Flags]
    public enum Systems
    {
        Engine,
        Flight,
        System
    }

    [Flags]
    public enum Relation
    {
        LessThan,
        GreaterThan,
        EqualTo
    }
}
