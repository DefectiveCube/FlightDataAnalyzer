using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XPlaneWPF.Controls
{
    public class DatapointTypeProvider : DataSourceProvider
    {
        private string directory;
        private Dictionary<string,Type> types; // this isn't correct as the relationship between file and types is NOT One-to-One (It is One-To-Many)
        private FileSystemWatcher watcher;

        public DatapointTypeProvider()
        {
            directory = string.Empty;
            types = new Dictionary<string, Type>();
            watcher = new FileSystemWatcher();

            watcher.Created += OnCreated;
            watcher.Changed += OnChanged;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;

            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
        }

        public bool IncludeSubdirectories { get; set; }

        public NotifyFilters Filter { get; set; }

        public string Path
        {
            get { return directory; }
            set
            {
                if (value == directory) return;
                if (!Directory.Exists(value))
                {
                    watcher.EnableRaisingEvents = false;
                    return;
                }

                watcher.Path = directory = value;
                watcher.EnableRaisingEvents = true;

                OnPropertyChanged(new PropertyChangedEventArgs("Directory"));
                if (!base.IsRefreshDeferred) base.Refresh();
            }
        }

        public Type[] Types
        {
            get { return types.Values.ToArray(); }
            private set
            {
                //types = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("Types"));
                //if (!base.IsRefreshDeferred) base.Refresh();
            }
        }

        protected override void BeginQuery()
        {
            Exception error = null;
            Type[] result = null;

            if (string.IsNullOrWhiteSpace(directory))
            {
                error = new ArgumentNullException("directory is null or empty");
            }
            else if (Directory.Exists(Path))
            {
                Load();

                result = Types;
            }
            else
            {
                error = new DirectoryNotFoundException("directory does not exist");
            }

            base.OnQueryFinished(result, error, null, null);
        }

        private void Load()
        {
            types.Add("EngineDatapoint", typeof(Prototype.EngineDatapoint));
            types.Add("FlightDatapoint", typeof(Prototype.FlightDatapoint));
            types.Add("SystemDatapoint", typeof(Prototype.SystemDatapoint));

            /*if (Directory.Exists(Path))
            {
                foreach (var file in Directory.EnumerateFiles(Path))
                {
                    types.Add(file, typeof(object));
                }
            }*/
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("{0} changed [{0}]", e.Name, e.ChangeType);

            // Change old type to new type
            types[e.Name] = typeof(object);

            base.OnQueryFinished(types.Values);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("{0} created", e.Name);

            // Add new type
            types.Add(e.Name, typeof(object));

            base.OnQueryFinished(types.Values);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("{0} deleted", e.Name);

            // Remove old type
            types.Remove(e.Name);

            base.OnQueryFinished(types.Values);
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine("{0} renamed to {1}", e.OldName, e.Name);

            // Change old type to new type
            types[e.Name] = typeof(object);

            base.OnQueryFinished(types.Values);
        }
    }
}
