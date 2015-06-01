using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneWPF.Providers
{
    public class PathProvider
    {
        public bool IsSpecialFolder { get { return true; } }

        public string ContainingDirectory { get { return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); } }

        public string ApplicationDirectory { get { return System.IO.Path.Combine(ContainingDirectory, "FlightDataAnalyzer"); } }

        public string DataDirectory { get { return System.IO.Path.Combine(ApplicationDirectory, "data"); } }

        public string ImportDirectory { get { return System.IO.Path.Combine(ApplicationDirectory, "import"); } }

        public string ModelDirectory { get { return System.IO.Path.Combine(ApplicationDirectory, "models"); } }
    }
}
