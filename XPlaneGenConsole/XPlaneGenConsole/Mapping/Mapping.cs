using System;

namespace XPlaneGenConsole
{
	public sealed class Mapping<T> : Mapping<T,T,T,T>
	{ }

	/// <summary>
	/// Input of Source = Output of Destination
	/// Output of Source = Input of Destination
	/// </summary>
	public sealed class Mapping<T,U> : Mapping<T,U,U,T>
	{ }

	public class Mapping<T,TResult,U,UResult>
	{
		//private List<Map> map;

		public virtual Func<T,TResult> Source{ get; protected set; }
		public virtual Func<U,UResult> Destination{ get; protected set; }

		public Mapping()
		{
			Source = t => default(TResult);
			Destination = u => default(UResult);
		}

		public Mapping(Func<T,TResult> source, Func<U,UResult> destination)
		{
			Source = source;
			Destination = destination;
		}

		public UResult ConvertTo(U arg)
		{
			// Convert Source Property to Destination Property using Func<>

			return Destination (arg);
		}

		public TResult ConvertFrom(T arg)
		{
			return Source (arg);
		}
	}
}