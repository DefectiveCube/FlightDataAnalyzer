using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using FDA;
using XPlaneWPF.Models;

namespace XPlaneWPF.Pages
{
    /// <summary>
    /// Interaction logic for QueryPage.xaml
    /// </summary>
    public partial class QueryPage : Page
    {
        public QueryPage()
        {
            InitializeComponent();

            _buildCommand.CanExecute += viewModel.BuildCommand_CanExecute;
            _buildCommand.Executed += viewModel.BuildCommand_Executed;

            typeListBox.SelectionChanged += viewModel.TypeSelectionChanged;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string tag = (sender as Control).Tag as string;

            Page page;

            switch (tag)
            {
                case "Home":
                    page = new StartupPage();
                    break;
                case "Data":
                    page = new ImportDataPage();
                    break;
                case "Query":
                    page = new QueryPage();
                    break;
                case "Model":
                    page = new DataModelPage();
                    break;
                default:
                    return;
            }

            NavigationService.Navigate(page);
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scv = sender as ScrollViewer;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
