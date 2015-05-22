using System;
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
using System.Windows.Shapes;

namespace XPlaneWPF
{
    /// <summary>
    /// Interaction logic for DataModelInfoWindow.xaml
    /// </summary>
    public partial class DataModelInfoWindow : Window
    {
        public ObservableCollection<DataModelInfo> Items { get; set; }

        public DataModelInfoWindow()
        {
            InitializeComponent();

            DataContext = this;

            Items = new ObservableCollection<DataModelInfo>();
            Items.Add(new DataModelInfo() { Type = typeof(Prototype.EngineDatapoint) });
            Items.Add(new DataModelInfo() { Type = typeof(Prototype.FlightDatapoint) });
            Items.Add(new DataModelInfo() { Type = typeof(Prototype.SystemDatapoint) });
        }
    }

    public class DataModelInfo
    {
        public Type Type { get; set; }

        public string Name
        {
            get { return Type.Name; }
        }

        public string Namespace
        {
            get { return Type.Namespace; }
        }

        public string Assembly
        {
            get { return Type.Assembly.Location; }
        }

        public string Version
        {
            get { return FileVersionInfo.GetVersionInfo(Type.Assembly.Location).ProductVersion; }
        }

        public int Fields
        {
            get
            {
                //var obj = Activator.CreateInstance(Type) as dynamic;

                //return obj.FIELDS_COUNT ?? 0;

                return 0;
            }
        }

        public int Size
        {
            get { return 0; }
        }
    }
}