using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public class XPlaneFileWriter : StreamWriter
    {
        public XPlaneFileWriter(Stream stream) : base(stream)
        {

        }

        public void Write(FlightDatapoint datapoint)
        {

        }

        public void Write(SystemDatapoint datapoint)
        {

        }

        public void Write(EngineDatapoint datapoint)
        {

        }
    }
}
