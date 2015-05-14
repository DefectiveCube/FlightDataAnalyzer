using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneWPF
{
    public class DataModelProperties : INotifyPropertyChanged
    {
        private string name, type, unit, conversion, format;
        private ushort column;
        private bool isUnsigned;
        private Type unitType;
        private string[] names;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public ushort Column
        {
            get { return column; }
            set
            {
                column = value;
                OnPropertyChanged("Column");
            }
        }

        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                OnPropertyChanged("Unit");
            }
        }

        public bool IsUnsigned
        {
            get { return isUnsigned; }
            set
            {
                isUnsigned = value;
                OnPropertyChanged("IsUnsigned");
            }
        }

        public string Conversion
        {
            get { return conversion; }
            set
            {
                conversion = value;
                OnPropertyChanged("Conversion");
            }
        }

        public string Format
        {
            get { return format; }
            set
            {
                format = value;
                OnPropertyChanged("Format");
            }
        }

        public Type TypeOfUnit
        {
            get { return unitType; }
            set
            {
                unitType = value;


                if (unitType != null && unitType.IsEnum)
                {
                    UnitNames = Enum.GetNames(TypeOfUnit);
                }

                OnPropertyChanged("TypeOfUnit");
            }
        }

        public string[] UnitNames
        {
            get { return names; }
            set
            {
                names = value;
                //OnPropertyChanged("UnitNames");
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
