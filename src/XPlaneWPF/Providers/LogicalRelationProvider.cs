using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XPlaneWPF.Models;

namespace XPlaneWPF.Providers
{
    public class LogicalRelationProvider : DataSourceProvider
    {
        private List<LogicalRelation> relations = new List<LogicalRelation>();

        public IEnumerable<LogicalRelation> Relations
        {
            get { return relations; }
            set
            {
                relations.Clear();
                relations.AddRange(value);
            }
        }

        protected override void BeginQuery()
        {
            relations.Add(new LogicalRelation() { Operator = "(None)" });
            relations.Add(new LogicalRelation() { OperationName = ExpressionType.Equal, Operator = "Equal To" });
            relations.Add(new LogicalRelation() { OperationName = ExpressionType.NotEqual, Operator = "Not Equal To" });
            relations.Add(new LogicalRelation() { OperationName = ExpressionType.LessThan, Operator = "Less Than" });
            relations.Add(new LogicalRelation() { OperationName = ExpressionType.LessThanOrEqual, Operator = "Less Than Or Equal To" });
            relations.Add(new LogicalRelation() { OperationName = ExpressionType.GreaterThan, Operator = "Greater Than" });
            relations.Add(new LogicalRelation() { OperationName = ExpressionType.GreaterThanOrEqual, Operator = "Greater Than Or Equal To" });

            OnQueryFinished(Relations);
        }
    }
}