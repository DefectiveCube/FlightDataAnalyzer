using System;
using UnitsNet;
using UnitsNet.Units;

namespace XPlaneGenConsole
{
	/// <summary>
	/// Defines the type of unit to use. If undefined, a string representing a lambda should be used. Otherwise, the value is ignored
	/// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal class FormatAttribute : Attribute
    {
        public readonly string Conversion;
        public readonly Type type;
        public readonly int value;

		/// <summary>
		/// Defines the unit type to use
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="conversion">Conversion.</param>
		/// <param name="value">Value.</param>
        private FormatAttribute(Type type, string conversion, int value)
        {
            Conversion = conversion;
            this.type = type;
            this.value = value;

            //var q = QueryBuilder.Query(conversion);
        }        

        public bool Defined
        {
            get { return value != 0; }
        }

        public bool IsCustomized
        {
            get { return !string.IsNullOrEmpty(Conversion); }
        }


        public FormatAttribute(AccelerationUnit unit, string conversion = "", string format = "") : this(typeof(AccelerationUnit),conversion,(int)unit)
        {

        }

        public FormatAttribute(AngleUnit unit, string conversion = "", string format = "") : this(typeof(AngleUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(ElectricCurrentUnit unit, string conversion = "", string format = "") : this(typeof(ElectricCurrentUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(ElectricPotentialUnit unit, string conversion = "", string format = "") : this(typeof(ElectricPotentialUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(LengthUnit unit, string conversion = "", string format = "") : this(typeof(LengthUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(MassUnit unit, string conversion = "", string format = "") : this(typeof(MassUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(PressureUnit unit, string conversion = "", string format = "") : this(typeof(PressureUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(RatioUnit unit, string conversion = "", string format = "") : this(typeof(RatioUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(RotationalSpeedUnit unit, string conversion = "", string format = "") : this(typeof(RotationalSpeedUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(SpeedUnit unit, string conversion = "", string format = "") : this(typeof(SpeedUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(TemperatureUnit unit, string conversion = "", string format = "") : this(typeof(TemperatureUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(TorqueUnit unit, string conversion = "", string format = "") : this(typeof(TorqueUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(VolumeUnit unit, string conversion = "", string format = "") : this(typeof(VolumeUnit), conversion, (int)unit)
        {

        }
    }
}
