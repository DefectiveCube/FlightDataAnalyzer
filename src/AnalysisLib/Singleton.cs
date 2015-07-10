using System;

namespace XPlaneGenConsole
{
	interface ISingleton<T>
	{
		T Instance{ get; }
	}

	public class Singleton<T>
	{
		private static readonly Lazy<T> instance = new Lazy<T> (() => Activator.CreateInstance<T> ());

		public Singleton()
        {

        }

		public static T Instance
		{ 
			get { return instance.Value; } 
		}
	}
}