using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XPlaneGenConsole;

namespace XPlaneWPF.Models
{
    public class PropertiesModel : INotifyPropertyChanged
    {
        private string name, type, unit, conversion, format, storage;
        private int column;
        private bool isUnsigned;
        private Type unitType, storageType;
        private string[] names;

        public event PropertyChangedEventHandler PropertyChanged;

        public PropertiesModel()
        {

        }

        public PropertiesModel(FormatAttribute format, StorageAttribute storage, GroupAttribute group, GraphAttribute graph, CsvFieldAttribute csv = null, Type propertyType = null)
        {
            Unit = format.UnitName;
            Conversion = format.Conversion;
            IsHexadecimal = format.Style == System.Globalization.NumberStyles.HexNumber;

            StorageType = storage.Type ?? propertyType;

            if (storageType == null)
            {
                throw new ArgumentNullException("type");
            }

            Column = csv.Index;
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public int Column
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
            get {
                if (storageType != typeof(bool))
                {
                    ValueType val = (ValueType)storageType.GetField("MinValue").GetValue(null);
                    bool IsSigned = storageType.IsPrimitive && Convert.ToBoolean(val);

                    return storageType.IsPrimitive && !IsSigned;
                }

                return true;
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

        public Type StorageType
        {
            get { return storageType; }
            set
            {
                storageType = value;
                OnPropertyChanged("Storage");
            }
        }

        public string Storage
        {
            get { return storageType.Name; }
        }

        public bool IsHexadecimal
        {
            get;
            set;
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
