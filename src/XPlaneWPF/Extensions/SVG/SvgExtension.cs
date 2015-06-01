using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Markup;

namespace XPlaneWPF.Extensions.SVG
{
    public class SvgExtension : MarkupExtension
    {
        public static string Directory { get; set; }

        [ConstructorArgument("Path")]
        public string Path { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            string path = System.IO.Path.Combine(Directory, Path);
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
    }
}