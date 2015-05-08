using System;
using XPlaneGenConsole;

namespace Prototype
{
    public class SystemCsvDatapoint : CsvDatapoint<SystemCsvDatapoint>
    {
        public override void Load(string value)
        {
            throw new NotImplementedException();
        }

        public override void Load(string[] values)
        {
            throw new NotImplementedException();
        }
    }
}
