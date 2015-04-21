using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public abstract class XPlaneFileMapping
    {
//		public abstract void DoSomething ();
    }

	public class XPlaneFileMapping<T> : XPlaneFileMapping
		where T : Datapoint<T>
	{
		
	}

	public class XPlaneFileMapping<T, U> : XPlaneFileMapping
		where T : Datapoint<T>
		where U : Datapoint<U>
	{
		public void Add(Func<T,U,object> func){

		}
	}

    public class XPlaneFileMapping<T, U, V> : XPlaneFileMapping
        where T : Datapoint<T>
        where U : Datapoint<U>
        where V : Datapoint<V>
    {
        public void Add(Func<T,U,V,object> action)
		{

		}
    }
}