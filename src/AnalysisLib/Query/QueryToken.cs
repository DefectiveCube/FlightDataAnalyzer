using System;
using System.Linq.Expressions;

namespace FDA
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
	{
		public readonly T ParsedValue;

		public QueryToken(T value, TokenType type) : base(type)
		{
			ParsedValue = value;
		}


		public override Expression GetExpression ()
		{
            switch (Token)
            {
                case TokenType.Operand:
                    return Expression.Constant(ParsedValue);
                default:
                    return base.GetExpression();
            }
		}

		public override string ToString ()
		{
			return ParsedValue.ToString ();
		}
	}

    public class QueryFieldToken : QueryToken
    {
        public readonly string Value;

        public QueryFieldToken(string value) : base(TokenType.Field)
        {
            Value = value;
        }

        public override Expression GetExpression()
        {
            return Expression.Parameter(typeof(double), Value);
        }
    }
}