using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace XPlaneWPF.Controls
{
    /// <summary>
    /// Interaction logic for InlineMenu.xaml
    /// </summary>
    public partial class InlineMenu : ItemsControl
    {
        public InlineMenu()
        {
            InitializeComponent();
            ItemsSource = MenuItems;
            MenuItems = new ObservableCollection<InlineMenuItem>();
            DataContext = this;
        }

        public ObservableCollection<InlineMenuItem> MenuItems { get; set; }
    }
}
