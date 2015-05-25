using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Microsoft.Win32;
using XPlaneGenConsole;

namespace XPlaneWPF.Pages
{
    /// <summary>
    /// Interaction logic for DataPage.xaml
    /// </summary>
    public partial class ImportDataPage : Page
    {

        public ObservableCollection<DriveInfo> Drives { get; private set; }

        public ObservableCollection<Type> ModelTypes { get; set; }
        
        public ImportDataPage()
        {
            InitializeComponent();

            Drives = new ObservableCollection<DriveInfo>();
            ModelTypes = new ObservableCollection<Type>();
            DataContext = this;
            this.Loaded += DataPage_Loaded;
        }

        void DataPage_Loaded(object sender, RoutedEventArgs e)
        {
            ModelTypes.Clear();
            ModelTypes.Add(typeof(Prototype.EngineDatapoint));
            ModelTypes.Add(typeof(Prototype.FlightDatapoint));
            ModelTypes.Add(typeof(Prototype.SystemDatapoint));

            var allDrives = DriveInfo.GetDrives();

            foreach (var drive in allDrives)
            {
                Drives.Add(drive);
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

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var list = new List<string>();
            var dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

            if (dialog.ShowDialog() == true)
            {
                list.Add(dialog.FileName);
            }

            this.FilePath.Text = dialog.FileName;
        }

        private async void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var type = ModelTypesBox.SelectedItem as Type;
            var generic = typeof(CsvConverter<>).MakeGenericType(type);
            var method = generic.GetMethod("LoadAsync");

            ImportButton.IsEnabled = false; 

            try
            {
                await (Task)method.Invoke(null, new[] { FilePath.Text, FilePath.Text + ".gz" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }

            ImportButton.IsEnabled = true;
            FilePath.Clear();
        }
    }
}
