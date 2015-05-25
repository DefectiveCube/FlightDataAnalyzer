using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneWPF.Models
{
    public class ModelBuildParameters : INotifyPropertyChanged
    {
        private string name, ns;
        private ObservableCollection<ModelBuildProperty> properties;

        public event PropertyChangedEventHandler PropertyChanged;


        public ModelBuildParameters()
        {
            properties = new ObservableCollection<ModelBuildProperty>();

            properties.CollectionChanged += Properties_CollectionChanged;
        }

        private void Properties_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Properties");
        }

        /// <summary>
        /// Name of the Type
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Namespace
        {
            get { return ns; }
            set
            {
                if (ns == value) return;
                ns = value;
                OnPropertyChanged("Namespace");
            }
        }

        /// <summary>
        /// Author of the Type (Defaults to Username, but can be overwritten)
        /// </summary>
        public string Author { get; set; }

        public ObservableCollection<ModelBuildProperty> Properties
        {
            get { return properties; }
            set
            {
                properties = value;
                OnPropertyChanged("Properties");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
