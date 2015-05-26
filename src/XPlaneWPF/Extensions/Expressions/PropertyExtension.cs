using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

using Exp = System.Linq.Expressions.Expression;

namespace XPlaneWPF.Extensions.Expressions
{
    public class PropertyExtension : MarkupExtension
    {
        public string Name { get; set; }

        public Exp Instance { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Exp.Property(Instance, Name);
        }
    }
}
