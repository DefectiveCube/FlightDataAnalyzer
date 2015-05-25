using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneWPF.Models
{
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

        public string[] Units { get { return Enum.GetNames(unit); } }
    }
}