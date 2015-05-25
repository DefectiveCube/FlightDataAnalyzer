using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XPlaneGenConsole;

namespace XPlaneWPF.Controls
{
    public class DatapointSourceProvider<T> : DataSourceProvider
        where T : BinaryDatapoint, new()
    {
        public ObservableCollection<T> Collection { get; set; }

        protected override void BeginQuery()
        {
            // Read all files of Type T

            // Evaluate against BinaryExpression

            base.OnQueryFinished(null);
        }

    }
}
