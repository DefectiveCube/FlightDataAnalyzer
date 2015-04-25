using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace XPlaneGenConsole
{
	[AttributeUsage(AttributeTargets.Field)]
	public class QueryOperationAttribute : Attribute
	{
		public readonly string Value;
		public readonly int Arity;
		public readonly OperatorAssociativity Associativity;

		public QueryOperationAttribute(string value)
		{
			Value = value;
		}
	}


	public enum OperatorAssociativity
	{
		Left,
		Right
	}

	public enum QueryOperations
	{
		None,

		[QueryOperation("=")]
		Equal,

		[QueryOperation("!=")]
		NotEqual,

		[QueryOperation(">")]
		GreaterThan,

		[QueryOperation("<")]
		LessThan,

		[QueryOperation("+")]
		Add,

		[QueryOperation("-")]
		Subtract,

		[QueryOperation("*")]
		Multiply,

		[QueryOperation("/")]
		Divide,

		[QueryOperation("%")]
		Modulus,

		[QueryOperation("&")]
		BitwiseAnd,

		[QueryOperation("|")]
		BitwiseOr,

		[QueryOperation("^")]
		BitwiseXor,

		[QueryOperation("?")]
		Ternary,

		[QueryOperation(":")]
		TernaryGate,

		[QueryOperation("&&")]
		LogicalAnd,

		[QueryOperation("||")]
		LogicalOr,

		[QueryOperation("!")]
		LogicalNot,
	}

	public enum QueryConstants
	{		
		Pi,
		e
	}

	public class QueryOperator : QueryToken<QueryOperations>
	{
		public readonly static Dictionary<string,QueryOperations> Operations;

		static QueryOperator()
		{
			Operations = new Dictionary< string,QueryOperations> ();

			var t = typeof(QueryOperations);
			var mems = t.GetFields ();

			foreach (var m in mems) {
				foreach (QueryOperationAttribute attr in m.GetCustomAttributes (typeof(QueryOperationAttribute))) {
					Operations.Add (attr.Value, (QueryOperations)m.GetValue (null));
				}
			}				
		}

		public static QueryOperations Match(string value)
		{
			QueryOperations ops = QueryOperations.None;

			Operations.TryGetValue (value, out ops);

			return ops;
		}


		public readonly OperatorAssociativity OperatorAssociativity;

		public QueryOperations Operation{ get { return ParsedValue; } }

		public QueryOperator (QueryOperations op) : base(op,TokenType.Operator)
		{
			
		}

		public override Expression GetExpression (Expression left, Expression right)
		{
			switch (Operation) {
			case QueryOperations.Add:
				return Expression.Add (left, right);
			case QueryOperations.Subtract:
				return Expression.Subtract (left, right);
			case QueryOperations.Multiply:
				return Expression.Multiply (left, right);
			case QueryOperations.Divide:
				return Expression.Divide (left, right);
			case QueryOperations.Modulus:
				return Expression.Modulo(left,right);

			case QueryOperations.LessThan:
				return Expression.LessThan (left, right);
			case QueryOperations.GreaterThan:
				return Expression.GreaterThan (left, right);

			case QueryOperations.BitwiseAnd:
				return Expression.And (left, right);			
			case QueryOperations.BitwiseXor:
				return Expression.ExclusiveOr (left, right);


			case QueryOperations.LogicalAnd:
				return Expression.And (left, right);
			default:
				throw new NotSupportedException (Operation.ToString());
			}
		}
	}
}