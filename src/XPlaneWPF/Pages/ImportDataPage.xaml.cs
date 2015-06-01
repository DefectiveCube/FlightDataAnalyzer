using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Microsoft.Win32;
using XPlaneGenConsole;
using XPlaneWPF.ViewModels;

namespace XPlaneWPF.Pages
{
    public partial class ImportDataPage : Page
    {        
        public ImportDataPage()
        {
            InitializeComponent();
            Loaded += ImportDataPage_Loaded;
        }

        void ImportDataPage_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = App.Current.MainWindow.DataContext as AppWindowViewModel;

            if (viewModel != null)
            {
                viewModel.PageName = "Flight Data Analyzer > Import Data";
            }
        }
    }
}