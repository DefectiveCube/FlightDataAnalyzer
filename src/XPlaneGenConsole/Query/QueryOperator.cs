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
            return Expression.MakeBinary(GetExpressionType(Operation), left, right);
		}

        private ExpressionType GetExpressionType(QueryOperations type)
        {
            switch (Operation)
            {
                case QueryOperations.Add:
                    return ExpressionType.Add;
                case QueryOperations.Subtract:
                    return ExpressionType.Subtract;
                case QueryOperations.Multiply:
                    return ExpressionType.Multiply;
                case QueryOperations.Divide:
                    return ExpressionType.Divide;
                case QueryOperations.Modulus:
                    return ExpressionType.Modulo;
                case QueryOperations.LessThan:
                    return ExpressionType.LessThan;
                case QueryOperations.GreaterThan:
                    return ExpressionType.GreaterThan;
                case QueryOperations.BitwiseAnd:
                    return ExpressionType.And;
                case QueryOperations.BitwiseXor:
                    return ExpressionType.ExclusiveOr;
                default:
                    throw new NotSupportedException(Operation.ToString());
            }

        }
    }
}