using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XPlaneGenConsole
{
    public abstract class FlightFile<T>
        where T : Datapoint<T>
    {
        public T Datapoints { get; set; }

        public string Path { get; protected set; }

        public abstract void Write();

        public abstract void Read();
    }

    public class BinaryFile : FlightFile<BinaryDatapoint>
    {
        public void GetReader() { }

        public void GetWriter() { }

        public override void Read()
        {
            throw new NotImplementedException();
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}