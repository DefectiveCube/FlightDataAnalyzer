using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Markup;
namespace XPlaneWPF.Extensions
{
    public class BrushExtension : MarkupExtension
    {
        [ConstructorArgument("Name")]
        public string Name { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var color = (Color)ColorConverter.ConvertFromString(Name);

            return new SolidColorBrush(color);
        }
    }
}
