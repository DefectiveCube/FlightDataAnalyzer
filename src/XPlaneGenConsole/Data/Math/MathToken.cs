using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole.Data.Math
{
    public class MathToken
    {
        public MathToken leftToken;
        public MathToken rightToken;
        public Expression expression;
        public int Depth;
        public int Index;
    }

    public class MathOperatorToken : MathToken
    {
        public MathOperation Value;
        public MathAssociativity Associativity;
        public int Precedence;

        public MathOperatorToken(MathOperation operation)
        {
            Value = operation;
            Associativity = MathAssociativity.Left;

            switch (Value)
            {
                case MathOperation.Add:
                case MathOperation.Subtract:
                    Precedence = 1;
                    break;
                case MathOperation.Multiply:
                case MathOperation.Divide:
                    Precedence = 2;
                    break;
                case MathOperation.Exponent:
                    Associativity = MathAssociativity.Right;
                    Precedence = 3;
                    break;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
    public class MathOperandToken : MathToken
    {
        public double Value;

        public MathOperandToken(double value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class MathVariableToken : MathToken
    {
        public string Value;

        public MathVariableToken(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }


    public enum MathOperation
    {
        None,
        Add,
        Subtract,
        Multiply,
        Divide,
        Exponent,
        OpenGroup,
        CloseGroup
    }

    public enum MathAssociativity
    {
        Left,
        Right
    }
}
