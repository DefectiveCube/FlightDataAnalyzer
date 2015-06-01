using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Svg2Xaml;

namespace XPlaneWPF.Controls
{
    /// <summary>
    /// Interaction logic for FloatingActionButton.xaml
    /// </summary>
    public partial class FloatingActionButton : Button
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(string), typeof(FloatingActionButton));
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(FloatingActionButton));

        public FloatingActionButton()
        {
            InitializeComponent();
        }

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set
            {
                SetValue(IconProperty, value);

                var path = Path.Combine(Extensions.SVG.SvgExtension.Directory, value);

                try
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        Image = Svg2Xaml.SvgReader.Load(fs);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
    }
}
