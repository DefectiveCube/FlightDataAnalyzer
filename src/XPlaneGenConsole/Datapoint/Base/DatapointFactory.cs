using System;

namespace XPlaneGenConsole
{
    public abstract class DatapointFactory<T>
        where T : Datapoint, new()
    {
        public abstract T CreateFromString(string value);

        public abstract T CreateFromString(string[] values);
    }

    public class CsvFactory<T> : DatapointFactory<T>
        where T : CsvDatapoint<T>, new()
    {
        public override T CreateFromString(string value)
        {
            return CreateFromString(value.Split(','));
        }

        public override T CreateFromString(string[] values)
        {
            T datapoint = Activator.CreateInstance<T>();

            datapoint.Load(values);

            return datapoint;
        }
    }
}
