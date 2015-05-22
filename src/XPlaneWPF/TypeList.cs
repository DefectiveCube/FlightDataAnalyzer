using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace XPlaneWPF
{
    public class TypeList : ObservableCollection<Type>
    {
        public event PropertyChangedEventHandler ItemPropertyChanged;
        
        public TypeList()
        {
            CollectionChanged += TypeList_CollectionChanged;
            ItemPropertyChanged += Item_PropertyChanged;

            Add(typeof(Angle));
            Add(typeof(Acceleration));
            Add(typeof(ElectricCurrent));
            Add(typeof(ElectricPotential));
            Add(typeof(ElectricResistance));
            Add(typeof(Length));
            Add(typeof(Pressure));
            Add(typeof(Ratio));
            Add(typeof(RotationalSpeed));
            Add(typeof(Speed));
            Add(typeof(Temperature));
            Add(typeof(Torque));
            Add(typeof(Volume));
            Add(typeof(bool));
            Add(typeof(byte));
            Add(typeof(short));
            Add(typeof(int));
            Add(typeof(long));
            Add(typeof(ushort));
            Add(typeof(uint));
            Add(typeof(ulong));
            Add(typeof(float));
            Add(typeof(double));
            Add(typeof(DateTime));
            Add(typeof(TimeSpan));
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
