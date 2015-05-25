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

namespace XPlaneWPF
{
    /// <summary>
    /// Interaction logic for AppWindow.xaml
    /// </summary>
    public partial class AppWindow :  NavigationWindow
    {
        private QueryWindow queryWindow;

        public AppWindow()
        {
            InitializeComponent();           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            queryWindow = new QueryWindow();
            queryWindow.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataModelInfoWindow window = new DataModelInfoWindow();
            window.Show();

            /*DataModelBuilder builder = new DataModelBuilder();
            builder.Show();*/
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DataModelBuilder builder = new DataModelBuilder();
            builder.Show();
        }
    }
}
