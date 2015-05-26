using System;
using System.Linq.Expressions;
using System.Windows.Markup;

namespace XPlaneWPF.Extensions.Expressions
{
    public class ParameterExtension : MarkupExtension
    {

        public ParameterExtension()
        {

        }

        [ConstructorArgument("Name")]
        public string Name { get; set; }

        [ConstructorArgument("Type")]
        public Type Type { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Expression.Parameter(Type, Name);
        }
    }
}