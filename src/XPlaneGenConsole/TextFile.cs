using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XPlaneGenConsole
{
    public class TextFile : FlightFile<TextDatapoint>
    {
        public void GetReader() { }

        public void GetWriter() { }

        public override void Write()
        {
            throw new NotImplementedException();
        }

        public override void Read()
        {
            throw new NotImplementedException();
        }
    }
}