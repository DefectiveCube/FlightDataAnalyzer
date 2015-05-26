using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneWPF.Models
{
    public class QuerySelection : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool use;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public bool Use
        {
            get { return use; }
            set
            {
                use = value;
                OnPropertyChanged("Use");
            }
        }

        public string Name { get; set; }

        public string Category { get { return CategoryType.Name; } }

        public Type CategoryType { get; set; }

        public string Unit { get; set; }

        public Type UnitType { get; set; }

        public string[] Units
        {
            get { return UnitType != null && UnitType.IsEnum ? Enum.GetNames(UnitType) : new string[] { }; }
        }

        public string Value { get; set; }

        public string Relation { get; set; }

        public string Conversion { get; set; }

        public string Group { get; set; }

        public string DataType { get; set; }
    }
}
