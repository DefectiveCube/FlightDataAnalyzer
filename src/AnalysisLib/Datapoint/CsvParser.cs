using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FDA.Attributes;
using UnitsNet;
using UnitsNet.Units;

namespace FDA
{
    // TODO: cleanup!

    public class CsvParser
    {
        private static MethodInfo ConversionMethod = 
            typeof(Extensions)
                .GetMethods (BindingFlags.Public | BindingFlags.Static)
			    .Where (m => m.Name.Equals ("As") && m.GetGenericArguments ().Length == 0)
                .First ();

        // QueryBuilder.Query builds Expressions from a string input
		private static MethodInfo Query = typeof(QueryBuilder).GetMethod ("Query", BindingFlags.Public | BindingFlags.Static);

        private static Expression CallConvert(Expression exp, Type value = null)
        {
			value = value ?? typeof(double); // Units are typically backed by a double value. This does not apply to primitive units

            // Calls "As" which takes a string, and a type for parameters. Since As returns as dynamic, Convert must be called
            var call = Expression.Call(ConversionMethod, exp, Expression.Constant(value, typeof(Type)));

            return Expression.Convert(call, value);
        }

        private static MethodCallExpression CallFrom(Expression index, Type type, int unit = -1)
        {
            var name = type.Name;

            switch (name)
            {
                case "Angle":
                    return CallFrom<Angle, AngleUnit>(index, unit > -1 ? (AngleUnit)unit : AngleUnit.Degree);
                case "Acceleration":
                    return CallFrom<Acceleration, AccelerationUnit>(index, unit > -1 ? (AccelerationUnit)unit : AccelerationUnit.MeterPerSecondSquared);
                case "ElectricCurrent":
                    return CallFrom<ElectricCurrent, ElectricCurrentUnit>(index, unit > -1 ? (ElectricCurrentUnit)unit : ElectricCurrentUnit.Ampere);
                case "ElectricPotential":
                    return CallFrom<ElectricPotential, ElectricPotentialUnit>(index, unit > -1 ? (ElectricPotentialUnit)unit : ElectricPotentialUnit.Volt);
                case "Frequency":
                    return CallFrom<Frequency, FrequencyUnit>(index, unit > -1 ? (FrequencyUnit)unit : FrequencyUnit.Hertz);
                case "Length":
                    return CallFrom<Length, LengthUnit>(index, unit > -1 ? (LengthUnit)unit : LengthUnit.Meter);
                case "Ratio":
                    return CallFrom<Ratio, RatioUnit>(index, unit > -1 ? (RatioUnit)unit : RatioUnit.DecimalFraction);
                case "RotationalSpeed":
                    return CallFrom<RotationalSpeed, RotationalSpeedUnit>(index, unit > -1 ? (RotationalSpeedUnit)unit : RotationalSpeedUnit.RevolutionPerMinute);
                case "Speed":
                    return CallFrom<Speed, SpeedUnit>(index, unit > -1 ? (SpeedUnit)unit : SpeedUnit.MeterPerSecond);
                case "Temperature":
                    return CallFrom<Temperature, TemperatureUnit>(index, unit > -1 ? (TemperatureUnit)unit : TemperatureUnit.Kelvin);
                case "Torque":
                    return CallFrom<Torque, TorqueUnit>(index, unit > -1 ? (TorqueUnit)unit : TorqueUnit.Newtonmeter);
                case "Volume":
                    return CallFrom<Volume, VolumeUnit>(index, unit > -1 ? (VolumeUnit)unit : VolumeUnit.CubicMeter);
            }

            throw new ArgumentOutOfRangeException();
        }

        private static MethodCallExpression CallFrom<T, U>(Expression exp, U unit)
        {
            return Expression.Call(typeof(T).GetMethod("From"), exp, Expression.Constant(unit, typeof(U)));
        }

        // TODO: create a return type, as Anon-Types cannot be returned and used
        private static dynamic GetPropertyInfo<T>()
            where T: BinaryDatapoint, new()
        {
            return typeof(T).GetProperties()
                .Select(p => new
                {
                    Name = p.Name,
                    Field = p.GetCustomAttributes<CsvFieldAttribute>().OrderBy(f => f.Index).Select(c => c),
                    Format = p.GetCustomAttribute<FormatAttribute>(),
                    Type = p.PropertyType
                })
                .Where(w => w.Field.Count() != 0);
            
        }

        private static MethodCallExpression BuildMethod()
        {
            throw new NotImplementedException();
        }

        // UnitMethod
        // PrimitiveMethod

        private static MethodCallExpression BuildDefinedUnitMethod()
        {
            throw new NotImplementedException();
        }

        private static MethodCallExpression BuildCustomizedMethod()
        {
            throw new NotImplementedException();
        }

        private static MethodCallExpression BuildDateTimeMethod()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates and returns 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
		public static Action<T, string[]> GetParser<T>()
            where T : BinaryDatapoint, new()
        {
            // TODO: cleanup! This is heavy on meta-programming, so it's a bit of a inherent brain-freeze to read already
            var record = typeof(T).GetCustomAttribute<CsvRecordAttribute>();

            if (record == null)
            {
                throw new Exception("Type does not support CSV records");
            }

            // TODO: Both types must have the same amount of fields! Throw if different

			var instance = Expression.Parameter (typeof(T), "datapoint");
			var nums = Expression.Parameter (typeof(string[]), "values");

            List<Expression> exps = new List<Expression>();
			Expression methodCall = null;

            // Obtain property info from type
            // Select
            //  - Name
            //  - Field: used to determine CSV field information
            //  - Format: used to determine the property type
            //  - Type

			var propInfo = typeof(T).GetProperties ()
				.Select (p => new {
					Name = p.Name,
					Field = p.GetCustomAttributes<CsvFieldAttribute> ().OrderBy (f => f.Index),
					Format = p.GetCustomAttribute<FormatAttribute> (),
					Type = p.PropertyType
				})
				.Where (w => w.Field.Count () != 0);

            var start = DateTime.Now;

			foreach (var info in propInfo)
            {
				methodCall = null;
                var arrayIndex = Expression.ArrayAccess(nums, Expression.Constant(info.Field.First().Index));

                if (info.Format != null)
                {
                    if (info.Format.IsDefinedUnit)
                    {
                        // Is supported unit
                        methodCall = CallFrom(CallConvert(arrayIndex), info.Type, info.Format.Unit);

                        if (methodCall == null)
                        {
                            Debug.WriteLine(string.Format("Property '{0}' with Type '{1}' was not parsed", info.Name, info.Type.FullName));
                            continue;
                        }
                    }
                    else if (info.Format.IsCustomized)
                    {
                        // Convert to supported unit
                        var exp = QueryBuilder.Query(info.Format.Conversion);
                        
                        var invoke = Expression.Invoke(exp, CallConvert(arrayIndex)); // Invoke querybuilder

                        methodCall = CallFrom(invoke, info.Type);   // Call "From" on type, using default unit
                    }
                    else if(info.Format.Style != NumberStyles.None)
                    {
                        methodCall = CallConvert(arrayIndex, info.Type);
                    }
                    else
                    {
                        // Not able to parse as unit is unknown, and there is no conversion provided
                        Debug.WriteLine(string.Format("Property '{0}' with Type '{1}' has an undefined unit and no conversion was provided", info.Name, info.Type.FullName));
                        continue;
                    }
                }
                else
                {
					if (info.Field.Count() == 1)
                    {
                        // Must be primitive type
                        methodCall = CallConvert(arrayIndex, info.Type);
                    }
					else if (info.Field.Count() == 2)
                    {
                        // Should be DateTime and TimeSpan
                        var left = CallConvert(arrayIndex, info.Type);
                        var right = CallConvert(
							Expression.ArrayAccess(nums, Expression.Constant(info.Field.Last().Index)), 
							info.Field.Last().Type);

                        methodCall = Expression.Add(left, right);
                    }

                    if (methodCall == null)
                    {
                        Debug.WriteLine(string.Format("Property '{0}' with Type '{1}' was not parsed", info.Name, info.Type.FullName));
                        continue;
                    }
                }

                exps.Add(Expression.Assign(Expression.Property(instance, info.Name), methodCall));
            }

            var block = Expression.Block(exps.ToArray());

            Debug.WriteLine(string.Format("Create Expression Tree of Type: {0} in {1} seconds", typeof(T).Name, DateTime.Now.Subtract(start).TotalSeconds));
            return Expression.Lambda<Action<T, string[]>>(block, instance, nums).Compile();
        }
    }
}