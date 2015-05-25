using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

using XPlaneWPF.Models;
using XPlaneWPF.ViewModels;

namespace XPlaneWPF.Controls
{
    /// <summary>
    /// Interaction logic for FileDialog.xaml
    /// </summary>
    public partial class FileDialog : UserControl
    {
        public FileDialog()
        {
            InitializeComponent();

            Loaded += ViewModel.OnLoaded;
        }

        private void drivesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var info = drivesListBox.SelectedItem as DriveInfo;

            ViewModel.Path.Directory = info.RootDirectory;
        }
    }
}
