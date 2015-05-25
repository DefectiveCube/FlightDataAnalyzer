using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace XPlaneWPF.Extensions
{
    public class PrimitiveExtension : MarkupExtension
    {
        [ConstructorArgument("Value")]
        public object Value { get; set; }

        [ConstructorArgument("Type")]
        public Type Type { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Type.IsPrimitive)
            {
                return TypeDescriptor.GetConverter(Type).ConvertTo(Value, Type);
            }

            throw new InvalidCastException("Not a primitive");
        }
    }

    public class FuncExtension : MarkupExtension
    {
        [ConstructorArgument("Input")]
        public Type[] Input { get; set; }

        [ConstructorArgument("Output")]
        public Type Output { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }

    public class ActionExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }

    public class PredicateExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }

    public class LambdaExtension : MarkupExtension
    {
        [ConstructorArgument("TypeArguments")]
        public Type[] TypeArguments { get; set; }

        [ConstructorArgument("HasReturnType")]
        public bool HasReturnType { get; set; }

        [ConstructorArgument("Expression")]
        public Expression Expression { get; set; }

        [ConstructorArgument("Parameters")]
        public ParameterExpression[] Parameters {get;set;}

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (TypeArguments == null || TypeArguments.Length == 0)
            {
                return Expression.Lambda(Expression, Parameters);
            }

            var delegateType = HasReturnType ? typeof(Func<>) : typeof(Action<>);
            var generic = delegateType.MakeGenericType(HasReturnType ? TypeArguments.Take(TypeArguments.Count() - 1).ToArray() : TypeArguments);
            var method = typeof(Expression).GetMethod("Lambda").MakeGenericMethod(delegateType);

            var obj = method.Invoke(null, new object[] { Expression, Parameters });

            return obj;

            //throw new NotImplementedException();
            //return Expression.Lambda<>
        }
    }

/*    public class LambdaExtension<TDelegate> : MarkupExtension
    {
        [ConstructorArgument("Delegate")]
        public Delegate Delegate { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }*/

    public class ExpressionExtension : MarkupExtension
    {
        public string Value { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }

    public class EvaluateExtension : MarkupExtension
    {
        [ConstructorArgument("Expression")]
        public Expression Expression { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Expression.ToString();
        }
    }

    public class ParameterExtension : MarkupExtension
    {
        [ConstructorArgument("Name")]
        public string Name { get; set; }

        [ConstructorArgument("Type")]
        public Type Type { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return string.IsNullOrWhiteSpace(Name) ? Expression.Parameter(Type) : Expression.Parameter(Type, Name);
        }
    }

    public class PropertyExtension : MarkupExtension
    {
        [ConstructorArgument("Expression")]
        public Expression Expression { get; set; }

        [ConstructorArgument("Name")]
        public string Name { get; set; }

        [ConstructorArgument("Accessor")]
        public MethodInfo Accessor { get; set; }

        [ConstructorArgument("PropertyInfo")]
        public PropertyInfo PropertyInfo { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Expression.Property(Expression, Name);
            //return Expression.Property(Expression, Accessor);
            //return Expression.Property(Expression, PropertyInfo);
        }
    }

    public class CallExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }

    public class InvokeExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }

    public class ConstantExtension : MarkupExtension
    {
        [ConstructorArgument("Value")]
        public object Value { get; set; }

        [ConstructorArgument("Type")]
        public Type Type { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Expression.Constant(TypeDescriptor.GetConverter(Type).ConvertTo(Value, Type), Type);
        }
    }
}
