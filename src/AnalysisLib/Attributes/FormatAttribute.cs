using System;
using System.Globalization;
using UnitsNet;
using UnitsNet.Units;

namespace FDA.Attributes
{
    /// <summary>
    /// Specifies:
    /// - The type of the enum to use (found in UnitsNet library)
    /// - Custom Conversions for ingoing and outgoing values (outgoing not implemented)
    /// - Number Styles (e.g. Hex, Grouped, etc.)
    /// - Formatting String (not implemented)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class FormatAttribute : Attribute
    {
        /// <summary>
        /// Conversion expression for an ingoing value
        /// </summary>
        public string Conversion { get; private set; }

        /// <summary>
        /// Conversion Expression for an outgoing value (currently not used)
        /// </summary>
        public string Inversion { get; private set; }

        /// <summary>
        /// The type of the respective UnitsNet enum
        /// </summary>
        public Type UnitEnumType { get; private set; }

        /// <summary>
        /// The value of the selected enum field
        /// </summary>
        public int Unit { get; private set; }

        /// <summary>
        /// The expected style of the number when reading and writing
        /// </summary>
        public NumberStyles Style { get; private set; }

        public string UnitName { get; private set; }

        /// <summary>
        /// True: if the unit is found within the UnitsNet library
        /// False: an unsupported unit (note: a conversion expression is expected in this case)
        /// </summary>
        public bool IsDefinedUnit { get; private set; }

        /// <summary>
        /// True: has a conversion expression string set
        /// </summary>
        public bool IsCustomized { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="unitEnumType">The enum uses</param>
        /// <param name="conversion">Conversion expression for ingoing value</param>
        /// <param name="unitValue">Value for the enum</param>
        private FormatAttribute(Type unitEnumType, string conversion, int unitValue)
        {
            Conversion = conversion;
            UnitEnumType = unitEnumType;
            Unit = unitValue;

            IsDefinedUnit = Unit != 0;
            IsCustomized = !string.IsNullOrEmpty(conversion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="style"></param>
        public FormatAttribute(NumberStyles style = NumberStyles.None)
        {
            Style = style;
        }

        public FormatAttribute(AccelerationUnit unit, string conversion = "", string format = "")
            : this(typeof(AccelerationUnit), conversion, (int)unit)
        {
            UnitName = unit.ToString();
        }

        public FormatAttribute(AngleUnit unit, string conversion = "", string format = "")
            : this(typeof(AngleUnit), conversion, (int)unit)
        {
            UnitName = unit.ToString();
        }

        public FormatAttribute(DurationUnit unit, string conversion = "", string format = "")
            : this(typeof(DurationUnit), conversion, (int)unit)
        {
            UnitName = unit.ToString();
        }

        public FormatAttribute(ElectricCurrentUnit unit, string conversion = "", string format = "")
            : this(typeof(ElectricCurrentUnit), conversion, (int)unit)
        {
            UnitName = unit.ToString();
        }

        public FormatAttribute(ElectricPotentialUnit unit, string conversion = "", string format = "")
            : this(typeof(ElectricPotentialUnit), conversion, (int)unit)
        {
            UnitName = unit.ToString();
        }

        public FormatAttribute(ElectricResistanceUnit unit, string conversion = "", string format = "")
            : this(typeof(ElectricResistanceUnit), conversion, (int)unit)
        {
            UnitName = unit.ToString();
        }
        public FormatAttribute(FrequencyUnit unit, string conversion = "", string format = "")
            : this(typeof(FrequencyUnit), conversion, (int)unit)
        {
            UnitName = unit.ToString();
        }

        public FormatAttribute(LengthUnit unit, string conversion = "", string format = "")
            : this(typeof(LengthUnit), conversion, (int)unit)
        {
            UnitName = unit.ToString();
        }

        public FormatAttribute(MassUnit unit, string conversion = "", string format = "")
            : this(typeof(MassUnit), conversion, (int)unit)
        {
            UnitName = unit.ToString();
        }

        public FormatAttribute(PressureUnit unit, string conversion = "", string format = "")
            : this(typeof(PressureUnit), conversion, (int)unit)
        {
            UnitName = unit.ToString();
        }

        public FormatAttribute(RatioUnit unit, string conversion = "", string format = "")
            : this(typeof(RatioUnit), conversion, (int)unit)
        {
            UnitName = unit.ToString();
        }

        public FormatAttribute(RotationalSpeedUnit unit, string conversion = "", string format = "")
            : this(typeof(RotationalSpeedUnit), conversion, (int)unit)
        {
            UnitName = unit.ToString();
        }

        public FormatAttribute(SpeedUnit unit, string conversion = "", string format = "")
            : this(typeof(SpeedUnit), conversion, (int)unit)
        {
            UnitName = unit.ToString();
        }

        public FormatAttribute(TemperatureUnit unit, string conversion = "", string format = "")
            : this(typeof(TemperatureUnit), conversion, (int)unit)
        {
            UnitName = unit.ToString();
        }

        public FormatAttribute(TorqueUnit unit, string conversion = "", string format = "")
            : this(typeof(TorqueUnit), conversion, (int)unit)
        {
            UnitName = unit.ToString();
        }

        public FormatAttribute(VolumeUnit unit, string conversion = "", string format = "")
            : this(typeof(VolumeUnit), conversion, (int)unit)
        {
            UnitName = unit.ToString();
        }
    }
}