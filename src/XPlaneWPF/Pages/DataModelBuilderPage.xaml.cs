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
    /// Interaction logic for DataModelBuilder.xaml
    /// </summary>
    public partial class DataModelBuilderPage : Page
    {
        public DataModelBuilderPage()
        {
            InitializeComponent();
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
                case "Import":
                    page = new ImportDataPage();
                    break;
                case "Query":
                    page = new QueryPage();
                    break;
                case "Model":
                    //page = new DataModelPage();
                    page = new DataModelBuilderPage();
                    break;
                default:
                    return;
            }

            NavigationService.Navigate(page);
        }
    }
}
