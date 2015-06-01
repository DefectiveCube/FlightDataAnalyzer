using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using XPlaneWPF.Extensions.SVG;

namespace XPlaneWPF.Converters
{
    public class StringToImageConverter : IValueConverter
    {
        // Override CanConvertFrom to return true for String-to-Complex conversions. 
        /*public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }*/

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Path = value as string;

            string path = System.IO.Path.Combine(SvgExtension.Directory, Path);
            DrawingImage image = null;

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    image = Svg2Xaml.SvgReader.Load(fs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(ex.StackTrace);
            }

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}