using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace XPlaneWPF.Extensions.Expressions
{
    public class ConvertExtension : MarkupExtension
    {
        [ConstructorArgument("Type")]
        public Type Type { get; set; }

        [ConstructorArgument("Value")]
        public Expression Value { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Expression.Convert(Value, Type);
        }
    }
}
