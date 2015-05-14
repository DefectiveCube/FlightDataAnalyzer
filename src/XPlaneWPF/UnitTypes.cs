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

    public class UnitExtension : MarkupExtension
    {
        private Type _enumType;

        public UnitExtension(Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }

            _enumType = enumType;
        }

        public Type EnumType
        {
            get { return _enumType; }
            set
            {
                if (_enumType == value)
                {
                    return;
                }

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (!enumType.IsEnum)
                {
                    throw new ArgumentException("Type must be an Enum");
                }

                _enumType = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var enumValues = Enum.GetValues(EnumType);


            var result = from object enumValue in enumValues
                         select new EnumerationMember
                         {
                             Name = GetSupportedUnit(enumValue),
                             Value = enumValue
                         };

            return result.Where(r => !string.IsNullOrEmpty(r.Name));
        }

        private string GetSupportedUnit(object enumValue)
        {
            var name = EnumType
                .GetField(enumValue.ToString())
                .GetCustomAttribute<SupportedUnit>();

            return name != null ? name.Unit.ToString() : "";
        }

        public class EnumerationMember
        {
            public string Name { get; set; }

            public object Value { get; set; }
        }
    }

    public class UnitTypeExtension : MarkupExtension
    {
        private Type _enumType;

        public UnitTypeExtension(Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }

            _enumType = enumType;
        }

        public Type EnumType
        {
            get { return _enumType; }
            set
            {
                if (_enumType == value)
                {
                    return;
                }

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (!enumType.IsEnum)
                {
                    throw new ArgumentException("Type must be an Enum");
                }

                _enumType = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var enumValues = Enum.GetValues(EnumType);


            var result = from object enumValue in enumValues
                         select new EnumerationMember
                         {
                             Name = GetSupportedUnitName(enumValue),
                             Value = enumValue
                         };

            return result;
        }

        private string GetSupportedUnitName(object enumValue)
        {
            var name = EnumType
                .GetField(enumValue.ToString())
                .GetCustomAttribute<SupportedUnitType>();

            return name != null ? name.UnitTypeName : enumValue.ToString();
        }

        public class EnumerationMember
        {
            public string Name { get; set; }

            public object Value { get; set; }
        }
    }
}
