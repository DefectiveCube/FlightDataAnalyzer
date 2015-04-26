using System;

namespace XPlaneGenConsole
{
    public abstract class DatapointFactory<T>
        where T : Datapoint, new()
    {
        public abstract T Create(string[] values);
    }
}
