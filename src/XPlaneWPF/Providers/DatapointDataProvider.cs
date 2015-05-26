using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XPlaneGenConsole;

namespace XPlaneWPF.Providers
{
    public class DatapointSourceProvider<T> : DataSourceProvider
        where T : BinaryDatapoint, new()
    {
        public ObservableCollection<T> Collection { get; set; }

        protected override void BeginQuery()
        {
            base.OnQueryFinished(null);
        }
    }
}
