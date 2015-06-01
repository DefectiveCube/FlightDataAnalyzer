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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using XPlaneWPF.ViewModels;

namespace XPlaneWPF.Pages
{
    public partial class StartupPage : Page
    {
        public StartupPage()
        {
            InitializeComponent();
            Loaded += StartupPage_Loaded;
        }

        void StartupPage_Loaded(object sender, RoutedEventArgs e)
        {            
            // Verify Folder Structure

            /*var viewModel = App.Current.MainWindow.DataContext as AppWindowViewModel;

            if (viewModel != null)
            {
                viewModel.PageName = "Flight Data Analyzer > Start";
            }*/

            /*var dialog = new XPlaneWPF.Dialogs.Dialog();
            App.Current.MainWindow.Effect = new BlurEffect()
            {
                Radius = 25
            };
            //dialog.ShowDialog();
            App.Current.MainWindow.Effect = null;*/
        }
    }
}
