using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XPlaneWPF.Models;
using XPlaneGenConsole;

namespace XPlaneWPF.ViewModels
{
    public class QueryBuilderViewModel
    {
        public ObservableCollection<QuerySelection> Items { get; set; }

        public ListCollectionView View { get; set; }

        public bool CanBuild
        {
            get { return Items.Where(i => i.Use).Count() > 0; }
        }

        public UnitTypeInfo SelectedItem { get; set; }

        public QueryBuilderViewModel()
        {
            Items = new ObservableCollection<QuerySelection>();
            Items.CollectionChanged += Items_CollectionChanged;
            View = new ListCollectionView(Items);
            View.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        }

        void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("[QueryBuilder ViewModel] Collection Changed {0}", e.Action);
            
        }

        public void TypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var type = (sender as ListBox).SelectedItem as Type;

            if (type != null)
            {
                Items.Clear();

                var props = type.GetProperties()
                    .Where(p => p.GetCustomAttribute<GraphAttribute>() != null)
                    .Select(p => new QuerySelection()
                    {
                        Name = p.Name,
                        Use = true,
                        CategoryType = p.PropertyType,
                        Unit = p.GetCustomAttribute<FormatAttribute>().UnitName,
                        UnitType = p.GetCustomAttribute<FormatAttribute>().type,
                        Value = string.Empty,
                        DataType = p.GetCustomAttribute<GraphAttribute>().DataType.ToString(),
                        Group = p.GetCustomAttribute<GroupAttribute>().Group,
                        Conversion = p.GetCustomAttribute<FormatAttribute>().Conversion
                    });

                foreach (var prop in props)
                {
                    Items.Add(prop);
                }
            }
        }
        
        public void BuildCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanBuild;
        }

        public void BuildCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BuildQuery();
        }

        public void BuildQuery()
        {
            var itemsToUse = Items.Where(i => i.Use);
        }

        public void RunQuery()
        {
            // from files

            // where

            // select
        }
    }
}
