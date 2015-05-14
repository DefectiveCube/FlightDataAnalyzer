using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DataModelBuilder : Window
    {
        private ObservableCollection<DataModelProperties> properties { get; set; }

        public DataModelBuilder()
        {
            InitializeComponent();
            DataContext = this;
            properties = new ObservableCollection<DataModelProperties>();
            DataGrid.ItemsSource = properties;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            properties.Add(new DataModelProperties()
            {
                Name = "Test",
                Column = 0,
                Type = "Angle",
                Unit = "Undefined",
                IsUnsigned = false,
                Conversion = string.Empty,
                Format = "#.##"
            });

            Debug.WriteLine(properties.Count);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            properties.Remove(properties.Last());
        }
    }
}
