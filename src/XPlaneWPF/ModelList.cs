using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using XPlaneGenConsole;

namespace XPlaneWPF
{
    public class ModelList : ObservableCollection<Type>
    {
        public event PropertyChangedEventHandler ItemPropertyChanged;

        public ModelList()
        {
            CollectionChanged += TypeList_CollectionChanged;
            ItemPropertyChanged += Item_PropertyChanged;

            Add(typeof(Prototype.EngineDatapoint));
            Add(typeof(Prototype.FlightDatapoint));
            Add(typeof(Prototype.SystemDatapoint));
        }

        private void TypeList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            /*if(e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }
            }

            if(e.OldItems != null)
            {
                foreach(INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= Item_PropertyChanged;
                }
            }*/
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

            OnCollectionChanged(args);

            if (ItemPropertyChanged != null)
            {
                ItemPropertyChanged(sender, e);
            }
        }

    }
}
