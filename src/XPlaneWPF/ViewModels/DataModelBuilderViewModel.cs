using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

using XPlaneWPF.Models;

namespace XPlaneWPF.ViewModels
{
    public class DataModelBuilderViewModel
    {
        public DataModelBuilderViewModel()
        {
            Parameters = new ModelBuildParameters();
        }

        public UnitTypeInfo SelectedItem { get; set; }

        public ModelBuildParameters Parameters { get; set; }
    }
}