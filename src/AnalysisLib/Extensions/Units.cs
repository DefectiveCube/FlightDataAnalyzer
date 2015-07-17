using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnitsNet;
using UnitsNet.Units;

namespace FDA.Extensions
{
	public static class Units
    {
        public static IEnumerable<Type> GetUnitTypes()
        {
            yield return typeof(Acceleration);
            yield return typeof(Angle);
            yield return typeof(Duration);
            yield return typeof(ElectricCurrent);
            yield return typeof(ElectricPotential);
            yield return typeof(ElectricResistance);
            yield return typeof(Frequency);
            yield return typeof(Length);
            yield return typeof(Mass);
            yield return typeof(Pressure);
            yield return typeof(Ratio);
            yield return typeof(RotationalSpeed);
            yield return typeof(Speed);
            yield return typeof(Temperature);
            yield return typeof(Torque);
            yield return typeof(Volume);

            yield break;
        }

        public static IEnumerable<Type> GetUnitEnumTypes()
        {
            yield return typeof(AccelerationUnit);
            yield return typeof(AngleUnit);
            yield return typeof(DurationUnit);
            yield return typeof(ElectricCurrentUnit);
            yield return typeof(ElectricPotentialUnit);
            yield return typeof(ElectricResistanceUnit);
            yield return typeof(FrequencyUnit);
            yield return typeof(LengthUnit);
            yield return typeof(MassUnit);
            yield return typeof(PressureUnit);
            yield return typeof(RatioUnit);
            yield return typeof(RotationalSpeedUnit);
            yield return typeof(SpeedUnit);
            yield return typeof(TemperatureUnit);
            yield return typeof(TorqueUnit);
            yield return typeof(VolumeUnit);

            yield break;
        }

        public static IEnumerable<object> GetDefaultUnits()
        {
            yield return AccelerationUnit.MeterPerSecondSquared;
            yield return AngleUnit.Degree;
            yield return DurationUnit.Second;
            yield return ElectricCurrentUnit.Ampere;
            yield return ElectricPotentialUnit.Volt;
            yield return ElectricResistanceUnit.Ohm;
            yield return FrequencyUnit.Hertz;
            yield return LengthUnit.Meter;
            yield return MassUnit.Kilogram;
            yield return PressureUnit.Pascal;
            yield return RatioUnit.DecimalFraction;
            yield return RotationalSpeedUnit.RevolutionPerSecond;
            yield return SpeedUnit.MeterPerSecond;
            yield return TemperatureUnit.DegreeCelsius;
            yield return TorqueUnit.Newtonmeter;
            yield return VolumeUnit.CubicMeter;

            yield break;
        }

        public static Type GetUnitType(this Type type)
        {
            if (!type.IsEnum)
            {
                return null;
            }

            return null;
        }

        public static object GetDefaultUnit(this Type type)
        {
            var data = GetDefaultUnits().Zip((type.IsEnum ? GetUnitEnumTypes() : GetUnitTypes()), (unit, unitType) => new { Type = unitType, Unit = unit });


            var name = type.Name;

            switch (name)
            {
                case "AccelerationUnit":
                    return AccelerationUnit.MeterPerSecondSquared.ToString();
                case "AngleUnit":
                    return AngleUnit.Degree.ToString();
                case "LengthUnit":
                    return LengthUnit.Meter.ToString();
            }

            return string.Empty;
        }

        public static object GetDefaultUnit<T>()
        {
            return GetDefaultUnit(typeof(T));
        }

        /*
        public static AccelerationUnit DefaultUnit(this AccelerationUnit unit)
        {
            return AccelerationUnit.MeterPerSecondSquared;
        }

        public static AngleUnit DefaultUnit(this AngleUnit unit){
            return AngleUnit.Degree;
        }
         

		public static AngleUnit Angle() { return AngleUnit.Degree; }

		public static LengthUnit Length { get { return LengthUnit.Meter; } }

        public static Expression FromUnit<T,U>(Expression exp, int unit)
		{
			return Expression.Call(typeof(T).GetMethod("From"), exp, Expression.Constant(unit, typeof(U)));
		}

		public static Expression FromUnit<T,U>(double value, int unit)
		{
			return FromUnit<T,U> (
				Expression.Constant (value, typeof(double)), 
				unit
			);
		}*/
	}
}