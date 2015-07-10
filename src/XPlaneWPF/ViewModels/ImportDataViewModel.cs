using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using FDA;
using XPlaneWPF.Commands;

namespace XPlaneWPF.ViewModels
{
    public class ImportDataViewModel : INotifyPropertyChanged
    {
        private Type model;
        private FileInfo importFile;
        private DriveInfo selectedDrive;
        private DirectoryInfo selectedDirectory;

        public event PropertyChangedEventHandler PropertyChanged;

        public ImportDataViewModel()
        {
            ImportCommand = new Command(Import, p => true);
            OpenCommand = new Command(OpenFile, p => true);
            ExploreCommand = new Command(Navigate, p => true);

            Drives = new ObservableCollection<DriveInfo>(DriveInfo.GetDrives());
            Directories = new ObservableCollection<DirectoryInfo>();
            Files = new ObservableCollection<FileInfo>();
        }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public ICommand ExploreCommand { get; set; }

        public ICommand OpenCommand { get; set; }

        public ICommand ImportCommand { get; set; }

        public string ImportFilePath
        {
            get { return ImportFile != null ? ImportFile.FullName : string.Empty; }
            set
            {
                ImportFile = !string.IsNullOrWhiteSpace(value) ? new FileInfo(value) : null;
                OnPropertyChanged("ImportFilePath");
                OnPropertyChanged("CanImport");
            }
        }

        public bool CanImport
        {
            get { return Model != null && ImportFile != null && ImportFile.Exists && ImportCommand.CanExecute(ImportFilePath); }
        }

        public ObservableCollection<DriveInfo> Drives { get; set; }
        
        public ObservableCollection<DirectoryInfo> Directories { get; set; }

        public ObservableCollection<FileInfo> Files { get; set; }

        public DriveInfo SelectedDrive
        {
            get { return selectedDrive; }
            set
            {
                if (selectedDrive == value) return;
                selectedDrive = value;
                OnPropertyChanged("SelectedDrive");

                Directories.Clear();
                Files.Clear();

                foreach (var dir in selectedDrive.RootDirectory.EnumerateDirectories())
                {
                    Directories.Add(dir);
                }
            }
        }

        public DirectoryInfo SelectedDirectory
        {
            get { return selectedDirectory; }
            set
            {
                if (selectedDirectory == value) return;
                selectedDirectory = value;
                OnPropertyChanged("SelectedDirectory");

                Files.Clear();

                if (selectedDirectory != null)
                {
                    foreach (var file in selectedDirectory.GetFiles())
                    {
                        Files.Add(file);
                    }
                }
            }
        }
        
        public DirectoryInfo Directory { get { return ImportFile != null ? ImportFile.Directory : null; } }

        public FileInfo ImportFile
        {
            get { return importFile; }
            set
            {
                if (importFile == value) return;
                importFile = value;
                OnPropertyChanged("ImportFile");
                OnPropertyChanged("CanImport");
            }
        }

        public Type Model
        {
            get { return model;}
            set
            {
                if (model == value) return;
                model = value;
                OnPropertyChanged("Model");
                OnPropertyChanged("CanImport");
            }
        }

        private void Converter_MessageWritten(object sender, string e)
        {
            Console.WriteLine(e);
        }

        // Opens a path (directories open in Explorer, files open in their default app)
        public void Navigate(object obj)
        {
            var path = obj as string;

            var valid = System.IO.Directory.Exists(path) || System.IO.File.Exists(path);

            if (valid)
            {
                try
                {
                    System.Diagnostics.Process.Start(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        // Invokes the Importation Process
        public async void Import(object obj)
        {            
            //ProgressPanel.Visibility = Visibility.Visible;

            Type generic, delegateType;
            MethodInfo method, handlerInfo, addHandler;
            EventInfo evtInfo;
            Delegate del;
            object[] addHandlerArgs = null;

            generic = typeof(CsvConverter<>).MakeGenericType(Model);

            if (generic == null)
            {
                throw new ArgumentNullException("Unable to generate converter");
            }

            method = generic.GetMethod("LoadAsync");

            if (method == null)
            {
                throw new MissingMethodException("Unable to locate method on converter");
            }
            
            // Get Event Info
            evtInfo = generic.GetEvent("MessageWritten", BindingFlags.Static | BindingFlags.Public);

            if (evtInfo != null)
            {
                delegateType = evtInfo.EventHandlerType;
                handlerInfo = this.GetType().GetMethod("Converter_MessageWritten", BindingFlags.NonPublic | BindingFlags.Instance);
                del = Delegate.CreateDelegate(delegateType, this, handlerInfo);
                addHandler = evtInfo.GetAddMethod();
                addHandlerArgs = new object[] { del };
                addHandler.Invoke(this, addHandlerArgs);
            }

            // Assemble output file path
            //var output = new FileInfo(ImportFilePath + ".gz");
            var outputDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "data");

            //TODO: output file needs to be unique

            try
            {
                await (Task)method.Invoke(null, new[] { ImportFilePath, outputDirectory });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                // Detach event handler
                if(evtInfo is EventInfo && addHandlerArgs != null){
                    evtInfo.GetRemoveMethod().Invoke(this, addHandlerArgs);
                }
            }

            //ImportButton.IsEnabled = true;
            //FilePath.Clear();

            ImportFilePath = string.Empty;
        }

        public void OpenFile(object obj)
        {
            var list = new List<string>();
            var dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

            if (dialog.ShowDialog() == true)
            {
                list.Add(dialog.FileName);
                ImportFilePath = dialog.FileName;
            }
        }
    }
}