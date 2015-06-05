using System;

namespace XPlaneGenConsole
{
	/*public class CsvFactory<T> : DatapointFactory<T>
		where T : CsvDatapoint<T>, new()
	{
		public override T CreateFromString(string value)
		{
			return CreateFromString (value.Split (new char[]{ ',' }, StringSplitOptions.RemoveEmptyEntries));
		}

		public override T CreateFromString(string[] values)
		{
			T datapoint = Activator.CreateInstance<T>();

			datapoint.Load(values);

			return datapoint;
		}
	}*/
}