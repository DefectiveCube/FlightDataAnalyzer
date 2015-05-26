using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace XPlaneWPF.Extensions
{
    public class UnitTypeInfoExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }

    public class UnitInfoExtension : MarkupExtension
    {
        private Type type;

        public Type Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}
