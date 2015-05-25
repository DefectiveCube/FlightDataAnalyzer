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
using XPlaneGenConsole;

namespace XPlaneWPF.Pages
{
    /// <summary>
    /// Interaction logic for DataModelPage.xaml
    /// </summary>
    public partial class DataModelPage : Page
    {
        public ObservableCollection<DataModelPropertyInfo> Info { get; set; }

        public DataModelPage()
        {
            InitializeComponent();

            Info = new ObservableCollection<DataModelPropertyInfo>();

            this.Loaded += DataModelPage_Loaded;
            this.ModelListBox.SelectionChanged += ModelListBox_SelectionChanged;

            DataContext = this;
        }

        void ModelListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var type = ModelListBox.SelectedItem as Type;

            if (type != null)
            {
                Info.Clear();

                var info = new DataModelPropertyInfo();

                var props = type.GetProperties()
                    .Where(s => s.GetCustomAttribute<FormatAttribute>() != null)
                    .Select(
                                    s => new DataModelPropertyInfo(
                                        s.GetCustomAttribute<FormatAttribute>(),
                                        s.GetCustomAttribute<StorageAttribute>(),
                                        s.GetCustomAttribute<GroupAttribute>(),
                                        s.GetCustomAttribute<GraphAttribute>(),
                                        s.GetCustomAttribute<CsvFieldAttribute>(),
                                        s.PropertyType)
                                    {
                                        Name = s.Name,
                                        Type = s.PropertyType.Name
                                        /*Unit = s.GetCustomAttribute<FormatAttribute>().UnitName,
                                        Conversion = s.GetCustomAttribute<FormatAttribute>().Conversion,
                                        StorageType = s.GetCustomAttribute<StorageAttribute>().Type,
                                        IsHexadecimal = s.GetCustomAttribute<FormatAttribute>().Style == System.Globalization.NumberStyles.HexNumber,
                                        Column = s.GetCustomAttribute<CsvFieldAttribute>().Index*/

                                        // Group
                                    });

                foreach (var item in props)
                {
                    Info.Add(item);
                }
            }
        }

        void DataModelPage_Loaded(object sender, RoutedEventArgs e)
        {
/*            ModelListBox.Items.Add(typeof(Prototype.EngineDatapoint));
            ModelListBox.Items.Add(typeof(Prototype.FlightDatapoint));
            ModelListBox.Items.Add(typeof(Prototype.SystemDatapoint));*/
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
                    //page = new DataPage();
                    page = new DataModelBuilderPage();
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
    }
}
