using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace XPlaneGenConsole
{
	public class FlightDataContext
	{
		public FlightDataContext ()
		{


		}

		internal static object Execute(Expression exp, bool IsEnumerable)
		{
			throw new NotSupportedException ();
		}
	}
}