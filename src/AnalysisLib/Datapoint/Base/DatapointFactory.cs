using System;

namespace FDA
{
    public abstract class DatapointFactory<T>
        where T : Datapoint, new()
    {
        public abstract T CreateFromString(string value);

        public abstract T CreateFromString(string[] values);
    }
}