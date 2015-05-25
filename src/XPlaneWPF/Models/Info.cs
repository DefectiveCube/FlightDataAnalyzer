using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneWPF.Models
{
    public class Info : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DirectoryInfo directory;

        public DirectoryInfo Directory
        {
            get { return directory; }
            set
            {
                if (directory != null && directory.FullName == value.FullName) return;
                directory = value;
                OnPropertyChanged("Directory");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
