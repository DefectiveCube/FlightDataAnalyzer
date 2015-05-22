using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneWPF
{
    public class SupportedUnit : Attribute
    {
        public readonly Type Unit;

        public SupportedUnit(Type unit)
        {
            Unit = unit;
        }
    }
    public class SupportedUnitType : Attribute
    {
        public readonly string UnitTypeName;

        public SupportedUnitType(string name)
        {
            UnitTypeName = name;
        }
    }
}
