using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace XPlaneWPF.Extensions.Android
{
    public class DevicePixelExtension : MarkupExtension
    {

        // Android Device Pixel = 1/160 inch
        // WPF Device Pixel = 1/96 inch
        // Difference: 1 2/3

        [ConstructorArgument("Value")]
        public double Value { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value * (1 + (2 / 3));
        }
    }
}
