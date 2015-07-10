using System;
using System.Collections.Generic;

namespace FDA
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple= true)]
	public class MethodAttribute : Attribute
	{
		public readonly Type BaseClass;
		public readonly string Method;
		public readonly Type[] Parameters;

		public MethodAttribute(Type type, string method, params Type[] paramTypes)
		{
			BaseClass = type;
			Method = method;
			Parameters = paramTypes;

			//var m = BaseClass.GetMethod (method, BindingFlags.Static);
		}
	}


	public enum QueryFunctions
	{		
		[Method(typeof(Math),"Floor",typeof(double))]
		[Method(typeof(Math),"Floor",typeof(decimal))]
		Floor,

		[Method(typeof(Math),"Ceiling",typeof(double))]
		[Method(typeof(Math),"Ceiling",typeof(decimal))]
		Ceiling,

		Round,

		Power,

		Exponent
	}
		
	public class QueryFunction : QueryToken<QueryFunctions>
	{
		public readonly static Dictionary<string,QueryFunctions> Methods;

		static QueryFunction()
		{
			Methods = new Dictionary<string, QueryFunctions> ();

			var t = typeof(QueryFunctions);
			var mems = t.GetFields ();

			foreach (var m in mems) {
				foreach (MethodAttribute attr in m.GetCustomAttributes(typeof(MethodAttribute),false)) {
					//var type = attr.BaseClass;
					//Methods.Add (attr.Method, Convert.ChangeType (m.GetValue (null), type));
				}
			}	
		}

		public QueryFunction (QueryFunctions funcs) : base(funcs,TokenType.Function)
		{
			
		}
	}
}