using System;
using System.Linq.Expressions;

namespace XPlaneGenConsole
{
	[Flags]
	public enum TokenType
	{
		None,
		Operand,
		Operator,
		Function,
		Separator,
		Grouping,
		Field
	}

	public class QueryToken{
		public static readonly QueryToken Empty = new QueryToken();

		public readonly TokenType Token;
		public readonly Type Type;

		public QueryToken(TokenType type = TokenType.None){
			Token = type;
		}

		public virtual Expression GetExpression(Expression left, Expression right){
			return Expression.Empty ();
		}

		public virtual Expression GetExpression(){
			return Expression.Empty ();
		}
	}

	public class QueryToken<T> : QueryToken
		where T: struct
	{
		public readonly T ParsedValue;

		public QueryToken(T value, TokenType type) : base(type)
		{
			ParsedValue = value;
		}


		public override Expression GetExpression ()
		{
			return Token == TokenType.Operand ? Expression.Constant(ParsedValue) :  base.GetExpression ();
		}

		public override string ToString ()
		{
			return ParsedValue.ToString ();
		}
	}
}