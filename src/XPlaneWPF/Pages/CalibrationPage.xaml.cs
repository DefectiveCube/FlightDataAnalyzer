using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XPlaneWPF.Pages
{
    /// <summary>
    /// Interaction logic for CalibrationPage.xaml
    /// </summary>
    public partial class CalibrationPage : Page
    {
        public CalibrationPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;

            var top = App.Current.MainWindow.Top;
            var left = App.Current.MainWindow.Left;

            App.Current.MainWindow.Top = 0;
            App.Current.MainWindow.Left = 0;
            App.Current.MainWindow.Width = 1920;
            App.Current.MainWindow.Height = 1080;
        }
    }
}
