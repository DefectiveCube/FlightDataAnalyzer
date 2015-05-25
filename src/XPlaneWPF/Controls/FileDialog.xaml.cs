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

namespace XPlaneWPF.Controls
{
    /// <summary>
    /// Interaction logic for FileDialog.xaml
    /// </summary>
    public partial class FileDialog : UserControl
    {
        public ObservableCollection<DriveInfo> Drives { get; private set; }

        public ObservableCollection<DirectoryInfo> Directories { get; private set; }

        public ObservableCollection<FileInfo> Files { get; private set; }

        public Info Path { get; set; }

        public FileDialog()
        {
            InitializeComponent();

            Drives = new ObservableCollection<DriveInfo>();
            Directories = new ObservableCollection<DirectoryInfo>();
            Files = new ObservableCollection<FileInfo>();

            DataContext = this;
            Loaded += FileDialog_Loaded;
        }

        void FileDialog_Loaded(object sender, RoutedEventArgs e)
        {
            Drives.Clear();

            var drives = DriveInfo.GetDrives();

            foreach (var drive in drives)
            {
                Drives.Add(drive);
            }
        }

        private void drivesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var info = drivesListBox.SelectedItem as DriveInfo;

            Path.Directory = info.RootDirectory;            

            /*
            if (info.RootDirectory.Exists)
            {
                Directories.Clear();
                foreach (var dir in info.RootDirectory.EnumerateDirectories())
                {
                    Directories.Add(dir);
                }

                Files.Clear();
                foreach (var file in info.RootDirectory.EnumerateFiles())
                {
                    Files.Add(file);
                }

            }
            else
            {

            }*/
        }
    }


    public class Info : INotifyPropertyChanged
    {
        public DirectoryInfo Directory { get; set; }
    }
}
