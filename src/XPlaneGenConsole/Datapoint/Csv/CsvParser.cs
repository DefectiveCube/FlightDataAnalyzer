using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using UnitsNet;
using UnitsNet.Units;

namespace XPlaneGenConsole
{
    public class CsvParser
    {
        private static MethodInfo ConversionMethod = typeof(Extensions).GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => m.Name.Equals("As") && m.GetGenericArguments().Length == 0).First();
        private static MethodInfo Query = typeof(QueryBuilder).GetMethod("Query", BindingFlags.Public | BindingFlags.Static);

        private static Expression CallConvert(Expression exp, Type value = null)
        {
            if(value == null)
            {
                value = typeof(double);
            }

            // Calls "As" which takes a string, and a type for parameters. Since As returns as dynamic, Convert must be called
            var call = Expression.Call(ConversionMethod, exp, Expression.Constant(value, typeof(Type)));

            return Expression.Convert(call, value);
        }

        private static MethodCallExpression Call(Expression index, Type type, int unit = -1)
        {
            var name = type.Name;

            switch (name)
            {
                case "Angle":
                    return Call<Angle, AngleUnit>(index, unit > -1 ? (AngleUnit)unit : AngleUnit.Degree);
                case "Acceleration":
                    return Call<Acceleration, AccelerationUnit>(index, unit > -1 ? (AccelerationUnit)unit : AccelerationUnit.MeterPerSecondSquared);
                case "ElectricCurrent":
                    return Call<ElectricCurrent, ElectricCurrentUnit>(index, unit > -1 ? (ElectricCurrentUnit)unit : ElectricCurrentUnit.Ampere);
                case "ElectricPotential":
                    return Call<ElectricPotential, ElectricPotentialUnit>(index, unit > -1 ? (ElectricPotentialUnit)unit : ElectricPotentialUnit.Volt);
                case "Length":
                    return Call<Length, LengthUnit>(index, unit > -1 ? (LengthUnit)unit : LengthUnit.Meter);
                case "Ratio":
                    return Call<Ratio, RatioUnit>(index, unit > -1 ? (RatioUnit)unit : RatioUnit.DecimalFraction);
                case "Speed":
                    return Call<Speed, SpeedUnit>(index, unit > -1 ? (SpeedUnit)unit : SpeedUnit.MeterPerSecond);
                case "Temperature":
                    return Call<Temperature, TemperatureUnit>(index, unit > -1 ? (TemperatureUnit)unit : TemperatureUnit.Kelvin);
                case "Torque":
                    return Call<Torque, TorqueUnit>(index, unit > -1 ? (TorqueUnit)unit : TorqueUnit.Newtonmeter);
                case "Volume":
                    return Call<Volume, VolumeUnit>(index, unit > -1 ? (VolumeUnit)unit : VolumeUnit.CubicMeter);
            }

            throw new ArgumentOutOfRangeException();
        }

        private static MethodCallExpression Call<T, U>(Expression exp, U unit)
        {
            return Expression.Call(typeof(T).GetMethod("From"), exp, Expression.Constant(unit, typeof(U)));
        }

        static IEnumerable<Tuple<string, CsvFieldAttribute[], FormatAttribute, Type>> GetFields<T>()
            where T : BinaryDatapoint, new()
        {
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes<CsvFieldAttribute>();

                if (attrs != null && attrs.Count() > 0)
                {
                    var format = prop.GetCustomAttribute<FormatAttribute>(); // should be null when the property type is a primitive

                    yield return new Tuple<string, CsvFieldAttribute[], FormatAttribute, Type>(prop.Name, attrs.ToArray(), format, prop.PropertyType);
                }
            }

            yield break;
        }

        public static Action<T, string[]> GetParser<T, U>()
            where T : BinaryDatapoint, new()
            where U : CsvDatapoint<U>, new()
        {
            var record = typeof(T).GetCustomAttribute<CsvRecordAttribute>();

            if (record == null)
            {
                throw new Exception("Type does not support CSV records");
            }

            // Both types must have the same amount of fields
            // TODO: check const values and compare. Throw if different

            var instance = Expression.Parameter(typeof(T), "datapoint");
            var nums = Expression.Parameter(typeof(string[]), "values");

            List<Expression> exps = new List<Expression>();

            foreach (var field in GetFields<T>())
            {
                // 1: string: Name of Property
                // 2: CsvFieldAttribute
                // 3: FormatAttribute
                // 4: Type: Type of Property

                Expression methodCall = null;
                var index = Expression.ArrayAccess(nums, Expression.Constant(field.Item2.First().Index));

                if (field.Item3 != null)
                {
                    if (field.Item3.IsDefinedUnit)
                    {
                        // Is supported unit
                        methodCall = Call(CallConvert(index), field.Item4, field.Item3.value);

                        if (methodCall == null)
                        {
                            Debug.WriteLine(string.Format("Property '{0}' with Type '{1}' was not parsed", field.Item1, field.Item4.FullName));
                            continue;
                        }
                    }
                    else if (field.Item3.IsCustomized)
                    {
                        // Convert to supported unit
                        var exp = QueryBuilder.Query(field.Item3.Conversion);
                        
                        // Invoke querybuilder
                        var invoke = Expression.Invoke(exp, CallConvert(index));

                        // Call "From" on type, using default unit
                        methodCall = Call(invoke, field.Item4);
                    }
                    else if(field.Item3.Style != NumberStyles.None)
                    {
                        Debug.WriteLine(field.Item3.Style);

                        methodCall = CallConvert(index, field.Item4);
                        continue;
                    }
                    else
                    {
                        // Not able to parse as unit is unknown, and there is no conversion provided
                        Debug.WriteLine(string.Format("Property '{0}' with Type '{1}' has an undefined unit and no conversion was provided", field.Item1, field.Item4.FullName));
                        continue;
                    }
                }
                else
                {
                    if (field.Item2.Length == 1)
                    {
                        // Must be primitive type
                        methodCall = CallConvert(index, field.Item4);
                    }
                    else if (field.Item2.Length == 2)
                    {
                        // Should be DateTime and TimeSpan
                        var left = CallConvert(index, field.Item4);
                        var right = CallConvert(Expression.ArrayAccess(nums, Expression.Constant(field.Item2.Last().Index)), field.Item2.Last().Type);

                        methodCall = Expression.Add(left, right);
                    }


                    if (methodCall == null)
                    {
                        Debug.WriteLine(string.Format("Property '{0}' with Type '{1}' was not parsed", field.Item1, field.Item4.FullName));
                        continue;
                    }
                }

                exps.Add(Expression.Assign(Expression.Property(instance, field.Item1), methodCall));
            }

            var block = Expression.Block(exps.ToArray());

            return Expression.Lambda<Action<T, string[]>>(block, instance, nums).Compile();
        }
    }
}