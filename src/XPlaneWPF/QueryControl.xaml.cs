using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XPlaneGenConsole;
using XPlaneWPF.Models;

namespace XPlaneWPF
{
    /// <summary>
    /// Interaction logic for QueryControl.xaml
    /// </summary>
    public partial class QueryControl : UserControl
    {
        public ObservableCollection<QuerySelection> Items { get; set; }

        //public ObservableCollection<>

        public ListCollectionView View { get; set; }

        public Type DatapointType { get; private set; }

        public QueryControl()
        {
            InitializeComponent();

            Items = new ObservableCollection<QuerySelection>();
            View = new ListCollectionView(Items);
            View.GroupDescriptions.Add(new PropertyGroupDescription("Group"));

            DataContext = this;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*var type = datapointTypeCB.SelectedItem as Type;

            if (type != null)
            {
                DatapointType = type;

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
            }*/
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            /*var where = Items.Where(i => !string.IsNullOrEmpty(i.Relation));
            var select = Items.Where(i => i.Use);

            var instance = Expression.Parameter(DatapointType, "datapoint");
            Expression property = null;
            double value;

            List<Expression> exps = new List<Expression>();

            foreach (var whereItem in where)
            {
                if (!double.TryParse(whereItem.Value, out value))
                {
                    // Prompt user for input???
                    continue;
                }

                property = Expression.Property(instance, whereItem.Name);

                var unitFromValue = Expression.Call(
                    whereItem.CategoryType.GetMethod("From"),
                    Expression.Constant(value, typeof(double)),
                    Expression.Field(null, whereItem.UnitType, whereItem.Unit));

                ExpressionType type;

                switch (whereItem.Relation)
                {
                    case "=":
                        type = ExpressionType.Equal;
                        break;
                    case "!=":
                        type = ExpressionType.NotEqual;
                        break;
                    case "<":
                        type = ExpressionType.LessThan;
                        break;
                    case "<=":
                        type = ExpressionType.LessThanOrEqual;
                        break;
                    case ">":
                        type = ExpressionType.GreaterThan;
                        break;
                    case ">=":
                        type = ExpressionType.GreaterThanOrEqual;
                        break;
                    default:
                        continue;
                }

                exps.Add(BinaryExpression.MakeBinary(type, property, unitFromValue));
            }

            exps.Add(instance);

            var query = Query.Create(DatapointType) as Query<Prototype.EngineDatapoint>;


            //Expression.Property(instance,"name");
            //ar result = Expression.Lambda(null);

            //Expression.Equals()

            //var whereCall = Where(DatapointType, Expression.Block(exps));
            //       var lambda = Expression.Lambda<Func<typeof(DatapointType), bool>>(Expression.Block(exps), new ParameterExpression[] { instance });
            //     var whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { }, null, lambda);

            var data = new Prototype.FlightDatapoint[] { };
            */
        }

        private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            /*var cb = sender as CheckBox;
            var group = cb.Tag as string;

            if (group != null)
            {
                foreach (var item in Items.Where(i => i.Group == group))
                {
                    item.Use = true;
                }
            }*/
        }

        private void CheckBox_ItemChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            /*
            var cb = sender as CheckBox;
            var group = cb.Tag as string;

            foreach (var item in constraintsDG.GroupStyle)
            {
              
            }
            if (Items.Where(i => i.Group == group).All(i => i.Use))
            {
                //cb.IsChecked = true;
            }
            else
            {
                //cb.IsChecked = null;
            }
             */
        }

        private void CheckBox_ItemUnchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            /*
            var cb = sender as CheckBox;
            var group = cb.Tag as string;

            if (Items.Where(i => i.Group == group).All(i => !i.Use))
            {
                //cb.IsChecked = false;
            }
            else
            {
                //cb.IsChecked = null;
            }
             */
        }

        private void CheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            /*
            var cb = sender as CheckBox;
            var group = cb.Tag as string;

            foreach (var item in Items.Where(i => i.Group == group))
            {
                item.Use = false;
            }*/
        }

        private void CheckBox_Indeterminate(object sender, System.Windows.RoutedEventArgs e)
        {
            /*
            var cb = sender as CheckBox;        

            CheckBox_Unchecked(sender, e);

            cb.IsChecked = false;
            return;
             */
        }
    }

    
}
