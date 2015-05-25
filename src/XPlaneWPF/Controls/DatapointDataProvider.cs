using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            //base.BeginQuery();

            base.OnQueryFinished(null);
        }

    }
}
