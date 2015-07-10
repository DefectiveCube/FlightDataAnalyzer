using System;
using System.Linq;
using System.Linq.Expressions;
using UnitsNet;
using UnitsNet.Units;

namespace FDA
{
	public static class Units
	{
		public static AccelerationUnit Acceleration() { return AccelerationUnit.MeterPerSecondSquared; }

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
		}
	}
}