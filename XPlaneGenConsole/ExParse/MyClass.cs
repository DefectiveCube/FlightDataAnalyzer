using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ExParse
{
	public class Query
	{
		public IEnumerable<ParameterExpression> Parameters{ get; private set; }
		public IEnumerable<string> Properties{ get; private set; }

		public Expression Parse<T>(string exp, Type returnType, params Type[] parameters)
			where T: struct
		{
			var sb = new StringBuilder ();

			sb.Append ("Func<");

			foreach (var t in parameters) {
				sb.Append (t.Name);
				sb.Append (", ");
			}

			sb.Append (typeof(T).Name);
			sb.Append ("> ");

			var tree = CSharpSyntaxTree.ParseText (sb.ToString ());

			Console.WriteLine (tree.ToString ());

			foreach (var node in tree.GetRoot().ChildNodes()) {
				Console.WriteLine (node.Kind ());
			}
			//var k = tree.GetRoot ().DescendantNodes ().First ().Kind ();

			//SyntaxKind.


			var lambdaOp = exp.IndexOf ("=>");


			//var p = Expression.Parameter (typeof(T), "f");
			//var 

			//var exp = 

			//var left = Expression.Property(pe, typeof(T).GetProperty("VerticalSpeed"));
			//var right = Expression.Constant((short)0);
			//var exp = Expression.GreaterThan(left, right);

			//var lamdba = Expression.Lambda<Func<T, bool>>(exp, new ParameterExpression[] { pe });
			//var whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { Query.ElementType }, Query.Expression, lamdba);


			//var lambda = Expression.Lambda<Func<T, bool>> (exp, Parameters);

				//new ParameterExpression[]{ p });

			return null;

		}
	}
}