using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace XPlaneWPF.Extensions.Expressions
{
    public abstract class BinaryExtension : MarkupExtension
    {
        protected BinaryExtension(ExpressionType type)
        {
            Type = type;
        }

        public ExpressionType Type { get; protected set; }

        [ConstructorArgument("Left")]
        public Expression Left { get; set; }

        [ConstructorArgument("Right")]
        public Expression Right { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Expression.MakeBinary(Type, Left, Right);
        }
    }

    public class AddExtension : BinaryExtension
    {
        public AddExtension() : base(ExpressionType.Add) { }
    }

    public class AddAssignExtension : BinaryExtension
    {
        public AddAssignExtension() : base(ExpressionType.AddAssign) { }
    }

    public class AddAssignCheckedExtension : BinaryExtension
    {
        public AddAssignCheckedExtension() : base(ExpressionType.AddAssignChecked) { }
    }

    public class AddCheckedExtension : BinaryExtension
    {
        public AddCheckedExtension() : base(ExpressionType.AddChecked) { }
    }

    public class AndExtension : BinaryExtension
    {
        public AndExtension() : base(ExpressionType.And) { }
    }

    public class AndAlsoExtension : BinaryExtension
    {
        public AndAlsoExtension() : base(ExpressionType.AndAlso) { }
    }

    public class AndAssignExtension : BinaryExtension
    {
        public AndAssignExtension() : base(ExpressionType.AndAssign) { }
    }

    public class EqualExtension : BinaryExtension
    {
        public EqualExtension() : base(ExpressionType.Equal) { }
    }


    public class LessThanExtension : BinaryExtension
    {
        public LessThanExtension() : base(ExpressionType.LessThan) { }
    }

    public class LessThanOrEqualExtension : BinaryExtension
    {
        public LessThanOrEqualExtension() : base(ExpressionType.LessThanOrEqual) { }
    }

    public class NotEqualExtension : BinaryExtension
    {
        public NotEqualExtension() : base(ExpressionType.NotEqual) { }
    }

    public class SubtractExtension : BinaryExtension
    {
        public SubtractExtension() : base(ExpressionType.Subtract) { }
    }

    public class MultiplyExtension : BinaryExtension
    {
        public MultiplyExtension() : base(ExpressionType.Multiply) { }
    }

    public class DivideExtension : BinaryExtension
    {
        public DivideExtension() : base(ExpressionType.Divide) { }
    }

    public class ModuloExtension : BinaryExtension
    {
        public ModuloExtension() : base(ExpressionType.Modulo) { }
    }
}