using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public class EngineCsvDatapoint : CsvDatapoint<EngineCsvDatapoint>
    {
        public override void Load(string value)
        {
            throw new NotImplementedException();
        }

        public override void Load(string[] values)
        {
            throw new NotImplementedException();
        }

        public new Action<EngineDatapoint, string[]> GetParser()
        {
            //return CsvParser.GetParser<EngineDatapoint, EngineCsvDatapoint>();
            throw new NotSupportedException();
        }
    }
}
