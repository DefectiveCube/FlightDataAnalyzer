using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneWPF.Models
{
    public class LogicalRelation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string @operator;
        private Expression left, right;

        public ExpressionType OperationName
        {
            get;
            set;
        }

        public string Operator
        {
            get { return @operator; }
            set
            {
                if (@operator == value) return;
                @operator = value;
                OnPropertyChanged("Operator");
            }
        }

        public Expression Left { get; set; }

        public Expression Right { get; set; }

        public BinaryExpression Value
        {
            get
            {
                return Expression.MakeBinary(OperationName, left, right);
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
