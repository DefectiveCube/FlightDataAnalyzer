using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XPlaneWPF.Providers
{
    public class UnitInfoProvider : DataSourceProvider
    {
        private List<string> info = new List<string>();

        public Type UnitType { get; set; }

        public string[] Info
        {
            get { return info.ToArray(); }
        }

        protected override void BeginQuery()
        {
            Exception error = null;

            if (UnitType != null && UnitType.IsEnum)
            {
                info.Clear();
                info.AddRange(Enum.GetNames(UnitType));
            }
            else
            {
                error = new InvalidCastException("Type is not enum");
            }

            OnQueryFinished(Info, error, null, null);
        }
    }
}