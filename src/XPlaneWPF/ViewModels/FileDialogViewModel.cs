using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows;

using XPlaneWPF.Models;

namespace XPlaneWPF.ViewModels
{
    public class FileDialogViewModel
    {
        private DriveInfo drive;
        private DirectoryInfo dir;
        private FileInfo file;

        public ObservableCollection<DriveInfo> Drives { get; private set; }

        public ObservableCollection<DirectoryInfo> Directories { get; private set; }

        public ObservableCollection<FileInfo> Files { get; private set; }

        public DriveInfo Drive
        {
            get { return drive; }
            set
            {
                if (drive == value) return;

                drive = value;
                OnDriveChanged();
            }
        }

        public DirectoryInfo Directory
        {
            get { return dir; }
            set
            {
                if (dir == value) return;

                dir = value;
                OnDirectoryChanged();
            }
        }

        public FileInfo File
        {
            get;
            set;
        }

        public Info Path { get; set; }

        public FileDialogViewModel()
        {
            Path = new Info();
            Drives = new ObservableCollection<DriveInfo>();
            Directories = new ObservableCollection<DirectoryInfo>();
            Files = new ObservableCollection<FileInfo>();
        }

        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            Drives.Clear();

            var drives = DriveInfo.GetDrives();

            foreach (var drive in drives)
            {
                Drives.Add(drive);
            }
        }

        public void OnDriveChanged()
        {
            if (drive.IsReady)
            {
                Directories.Clear();
                foreach (var dir in Drive.RootDirectory.EnumerateDirectories())
                {
                    Directories.Add(dir);
                }

                Files.Clear();
                foreach (var file in Drive.RootDirectory.EnumerateFiles())
                {
                    Files.Add(file);
                }
            }
            else
            {
                Console.WriteLine("drive is not ready");
            }
        }

        public void OnDirectoryChanged()
        {
            if (!drive.IsReady)
            {
                Console.WriteLine("Drive not ready");
                return;
            }

            var isReparsePoint = (Directory.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint;
            if (isReparsePoint)
            {
                Console.WriteLine("Access Denied");
                return;
            }

            var access = Directory.GetAccessControl();
            /*foreach (FileSystemAccessRule rule in access.GetAccessRules(true, true, typeof(NTAccount)))
            {
                Console.WriteLine("\tIdentityReference = {0}", rule.IdentityReference);
                Console.WriteLine("\tInheritanceFlags  = {0}", rule.InheritanceFlags);
                Console.WriteLine("\tPropagationFlags  = {0}", rule.PropagationFlags);
                Console.WriteLine("\tAccessControlType = {0}", rule.AccessControlType);
                Console.WriteLine("\tFileSystemRights  = {0}", rule.FileSystemRights);
                Console.WriteLine();
            }

            if (drive.IsReady)
            {
                Files.Clear();
                foreach(var file in Directory.EnumerateFiles())
                {
                    Files.Add(file);
                }
            }else
            {
                Console.WriteLine("drive is not ready");
            }
            */
        }
    }
}
