using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public class CSVWriter : StreamWriter
    {
        public CSVWriter(Stream stream) : base(stream)
        {

        }

        public void WriteSeparator()
        {
            Write(",");
        }

        public override void Write(string value)
        {
            base.Write(value);
        }

        public void Write(string[] values)
        {
            for (int i = 0; i < values.Length - 1; i++)
            {
                Write(values[i]);
                WriteSeparator();
            }

            Write(values.Last());
        }
    }
}
