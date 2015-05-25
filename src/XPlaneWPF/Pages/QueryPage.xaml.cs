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
    /// Interaction logic for QueryPage.xaml
    /// </summary>
    public partial class QueryPage : Page
    {
        public ObservableCollection<QuerySelection> QueryItems { get; set; }

        public ListCollectionView View { get; set; }

        public QueryPage()
        {
            InitializeComponent();

            QueryItems = new ObservableCollection<QuerySelection>();
            View = new ListCollectionView(QueryItems);
            View.GroupDescriptions.Add(new PropertyGroupDescription("Group"));

            DataContext = this;

            this.Loaded += QueryPage_Loaded;
            ModelListBox.SelectionChanged += ModelListBox_SelectionChanged;
        }

        void QueryPage_Loaded(object sender, RoutedEventArgs e)
        {
            ModelListBox.Items.Clear();

            ModelListBox.Items.Add(typeof(Prototype.EngineDatapoint));
            ModelListBox.Items.Add(typeof(Prototype.FlightDatapoint));
            ModelListBox.Items.Add(typeof(Prototype.SystemDatapoint));
        }

        void ModelListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var type = ModelListBox.SelectedItem as Type;

            if (type != null)
            {
                QueryItems.Clear();

                var props = type.GetProperties()
                    .Where(p => p.GetCustomAttribute<GraphAttribute>() != null)
                    .Select(p => new QuerySelection()
                    {
                        Name = p.Name,
                        Use = true,
                        CategoryType = p.PropertyType,
                        Unit = p.GetCustomAttribute<FormatAttribute>().UnitName,
                        UnitType = p.GetCustomAttribute<FormatAttribute>().type,
                        Value = string.Empty,
                        DataType = p.GetCustomAttribute<GraphAttribute>().DataType.ToString(),
                        Group = p.GetCustomAttribute<GroupAttribute>().Group,
                        Conversion = p.GetCustomAttribute<FormatAttribute>().Conversion
                    });

                foreach (var prop in props)
                {
                    QueryItems.Add(prop);
                }
            }
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
    }
}
