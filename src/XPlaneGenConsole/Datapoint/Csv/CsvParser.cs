using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using UnitsNet;
using UnitsNet.Units;

namespace XPlaneGenConsole
{
	public class CsvParser
	{
		private static MethodCallExpression Call(Type type, Expression index, int unit)
		{
			if (unit == 0 && type != typeof(double)) {
				Debug.Write (type.Name);
				Debug.WriteLine (" has an undefined scale");
				return null; // no conversions for undefined types
			}

			var name = type.Name;

			switch (name) {
			case "Angle":
				return Call<Angle,AngleUnit> (index, (AngleUnit)unit);
			case "Acceleration":
				return Call<Acceleration,AccelerationUnit> (index, (AccelerationUnit)unit);
			case "ElectricCurrent":
				return Call<ElectricCurrent,ElectricCurrentUnit> (index, (ElectricCurrentUnit)unit);
			case "ElectricPotential":
				return Call<ElectricPotential,ElectricPotentialUnit> (index, (ElectricPotentialUnit)unit);
			case "Length":
				return Call<Length,LengthUnit> (index, (LengthUnit)unit);
			case "Ratio":
				return Call<Ratio, RatioUnit> (index, (RatioUnit)unit);
			case "Speed":
				return Call<Speed,SpeedUnit> (index, (SpeedUnit)unit);
			case "Temperature":
				return Call<Temperature,TemperatureUnit> (index, (TemperatureUnit)unit);
			case "Torque":
				return Call<Torque,TorqueUnit> (index, (TorqueUnit)unit);
			case "Volume":
				return Call<Volume,VolumeUnit> (index, (VolumeUnit)unit);
			}

			return null;
		}

		private static MethodCallExpression Call<T,U>(Expression index, U unit)
		{
			return Expression.Call (typeof(T).GetMethod ("From"), index, Expression.Constant (unit, typeof(U)));
		}

		static IEnumerable<Tuple<string,int,int,Type>> GetFields<T>()
			where T: BinaryDatapoint, new()
		{
			var props = typeof(T).GetProperties ();

			foreach (var prop in props) {
				var attrs = prop.GetCustomAttributes<CsvFieldAttribute> ();

				if (attrs != null && attrs.Count () > 0) {
					
					var format = prop.GetCustomAttribute<FormatAttribute> ();

					var formatValue = format != null ? format.value : 0; //defaults are 0

					foreach (var attr in attrs) {
						yield return new Tuple<string,int,int,Type> (prop.Name, formatValue, attr.Index, prop.PropertyType);		
					}
				}
			}

			yield break;
		}

		public static Action<T,double[]> GetParser<T,U>()
			where T: BinaryDatapoint, new()
			where U: CsvDatapoint<U>, new()
		{
			var record = typeof(T).GetCustomAttribute<CsvRecordAttribute> ();

			if (record == null) {
				throw new Exception ("Type does not support CSV records");
			}

			var instance = Expression.Parameter (typeof(T), "datapoint");
			var nums = Expression.Parameter (typeof(double[]), "values");

			List<Expression> exps = new List<Expression> ();

			foreach (var prop in GetFields<T>()) {
				// 1: prop name
				// 2: format
				// 3: index
				// 4: type
				var methodCall = Call (
					                  prop.Item4,
					                  Expression.ArrayAccess (nums, Expression.Constant (prop.Item3)),
					                  prop.Item2);
					
				if (methodCall == null) {
					Debug.WriteLine (string.Format ("Property '{0}' Type '{1}' has no conversion", prop.Item1, prop.Item4));
					continue;
				}

				exps.Add (Expression.Assign (Expression.Property (instance, prop.Item1), methodCall));
			}

			var block = Expression.Block ( exps.ToArray ());

			return Expression.Lambda <Action<T,double[]>> (block, instance, nums).Compile ();
		}
	}
}