using System;
using System.ComponentModel;

namespace XPlaneGenConsole
{
	public abstract class Map : ICsvMapping
	{
		public virtual Type Source{ get; protected set; }
		public virtual Type Destination{ get; protected set; }
		public virtual TypeConverter Converter { get; protected set; }

		protected Map()
		{ }

		protected Map(Type source,Type dest)
		{


		}
	}

	/// <summary>
	/// Represents of Map of property conversions for TSource to TDest and back
	/// </summary>
	public sealed class Map<TSource,TDest> : Map
		where TSource: BinaryDatapoint //Datapoint<TSource>
		where TDest: BinaryDatapoint//Datapoint<TDest>
	{
		//
		public Map() : base(typeof(TSource),typeof(TDest))
		{

		}

		public static void Associate(string source, string destination, string function)
		{
			var a = typeof(TSource).GetProperty (source);
			var b = typeof(TDest).GetProperty (destination);

			// input => output
			var t = a.PropertyType; // input
			var u = b.PropertyType; // output

			//Func<t,u> tempConvert = fahrenheit => Convert.ChangeType((fahrenheit - 32) * 5 / 9,u);

			//var tempConv = "f => (f - 32) * 5 / 9";



			Console.WriteLine (t.Name);
			Console.WriteLine (u.Name);
		}

		void Add<T>(){
			var m = new Mapping<T> ();

			m.Source (default(T));
		}

		void Add<T,U>(){
		}
	}
}