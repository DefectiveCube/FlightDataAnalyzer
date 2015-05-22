using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace XPlaneWPF
{
    public enum SupportedUnitTypes
    {
        [SupportedUnitType("Acceleration")]
        [SupportedUnit(typeof(UnitsNet.Units.AccelerationUnit))]
        Acceleration,

        [SupportedUnitType("Angle")]
        [SupportedUnit(typeof(UnitsNet.Units.AngleUnit))]
        Angle,

        [SupportedUnitType("Temperature")]
        [SupportedUnit(typeof(UnitsNet.Units.TemperatureUnit))]
        Temperature,

        [SupportedUnitType("Torque")]
        [SupportedUnit(typeof(UnitsNet.Units.TorqueUnit))]
        Torque,

        [SupportedUnitType("Volume")]
        [SupportedUnit(typeof(UnitsNet.Units.VolumeUnit))]
        Volume,

        [SupportedUnitType("Boolean")]
        @Boolean,

        [SupportedUnitType("Byte")]
        @Byte,

        [SupportedUnitType("Integer 16-bit")]
        int16,
        
        [SupportedUnitType("Integer 32-bit")]
        int32,

        [SupportedUnitType("Integer 64-bit")]
        int64,

        [SupportedUnitType("Floating Point 32-bit")]
        @float,

        [SupportedUnitType("Floating Point 64-bit")]
        @double
    }


}
