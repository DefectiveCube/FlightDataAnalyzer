using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;
using ExParse;

namespace ExParseTest
{
	[TestFixture ()]
	public class Test
	{
		[Test()]
		public void ExpressionTest()
		{
			var add = Expression.Add (
				Expression.Parameter (typeof(int), "a"),
				Expression.Parameter (typeof(int), "b"));

			Console.WriteLine (add.ToString ());

			add = Expression.Assign (
				Expression.Parameter (typeof(bool), "a"),
				Expression.Constant (false)
			);


			Console.WriteLine (add.ToString ());

			Assert.Pass ();

		}

		[Test ()]
		public void TestCase ()
		{

			var q = new Query ();

			//q.Parse<string> ("x + y");
			q.Parse<int> ("f => (f - 32) * 5 / 9",typeof(int),typeof(int));

			Assert.Pass ();
		}
	}
}