using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
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
        public PropertyList Properties { get; set; }

        public DataModelBuilder()
        {
            InitializeComponent();

            Properties = new PropertyList();

            DataGrid.DataContext = this;

            using (var xamlFile = new FileStream(@"Documents/Units.xaml", FileMode.Open, FileAccess.Read))
            {
                var content = XamlReader.Load(xamlFile) as FlowDocument;

                PageViewer.Document = content;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Add(new DataModelPropertyInfo()
             {
                 Name = "Test",
                 Column = 0,
                 Type = typeof(UnitsNet.Angle).Name,
                 Unit = "Undefined",
                 //IsUnsigned = false,
                 Conversion = string.Empty,
                 Format = "#.##"
             });

            Debug.WriteLine(Properties.Count);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //properties.Remove(properties.Last());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Creating");
        }
    }
}
