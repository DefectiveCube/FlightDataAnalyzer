using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using FDA;

namespace XPlaneWPF.Providers
{
    public class DatapointSourceProvider : DataSourceProvider
    {
        private FileSystemWatcher watcher;

        public DatapointSourceProvider()
        {
            watcher = new FileSystemWatcher();

            watcher.Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlightDataAnalyzer", "data");

            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;

            watcher.Created += watcher_Created;
            watcher.Changed += watcher_Changed;
            watcher.Deleted += watcher_Deleted;
            watcher.Renamed += watcher_Renamed;
            watcher.Error += watcher_Error;

            watcher.EnableRaisingEvents = true;
        }

        void watcher_Error(object sender, ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        void watcher_Created(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void BeginQuery()
        {
            base.OnQueryFinished(null);
        }
    }
}
