using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using UnitsNet;
using UnitsNet.Units;

namespace XPlaneWPF.Controls
{
    public class UnitTypeInfoProvider : DataSourceProvider
    {
        private List<UnitTypeInfo> infoList = new List<UnitTypeInfo>();

        public UnitTypeInfo[] Info { 
            get { return infoList.ToArray(); }
        }

        protected override void BeginQuery()
        {
            infoList.Add(new UnitTypeInfo(typeof(Acceleration), typeof(AccelerationUnit)));
            infoList.Add(new UnitTypeInfo(typeof(Angle), typeof(AngleUnit)));

            base.OnQueryFinished(Info);
        }
    }


    public class UnitTypeInfo
    {
        private readonly Type unitType;
        private readonly Type unit;

        public UnitTypeInfo(Type unitType, Type unit)
        {
            this.unitType = unitType;
            this.unit = unit;
        }

        public string Name { get { return unitType.Name; } }
        
        public IEnumerable<string> Units
        {
            get { return Enum.GetNames(unit).Where(n => n != "Undefined"); }
        }
    }
}
