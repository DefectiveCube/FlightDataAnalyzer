using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using UnitsNet.Units;

namespace XPlaneGenConsole
{
    public static class Conversion
    {

        /// <summary>
        /// Key: type's full name
        /// Value: list of indices
        /// </summary>
        private static Dictionary<string, HashSet<int>> IndexDict; // Contains a list of csv indices that are used


        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<string, PropertyInfo> PropertyDict;

        // not used (yet)
        private static Dictionary<string, string> FormatDict;

        /// <summary>
        /// Colletion of Funcs used to import various datatypes
        /// </summary>
        private static Dictionary<Type, Delegate> ImportFuncs;

        /// <summary>
        /// Collection of Actions used to invoke the setter (through compiled expressions)
        /// </summary>
        private static Dictionary<string, Dictionary<int, Delegate>> ImportSetters;

        static Conversion()
        {
            IndexDict = new Dictionary<string, HashSet<int>>();
            //PropertyDict = new Dictionary<Tuple<string, int>, Tuple<CsvFieldAttribute, PropertyInfo>>();
            ImportFuncs = new Dictionary<Type, Delegate>();
            ImportSetters = new Dictionary<string, Dictionary<int, Delegate>>();

            LoadImportFunctions();

            // TODO: don't hardcode these, and allow custom types to be used
            LoadReflectionProperties<FlightDatapoint, FlightCsvDatapoint>();
            //LoadReflectionProperties<EngineDatapoint, EngineCsvDatapoint>();
            //LoadReflectionProperties<SystemDatapoint, SystemCsvDatapoint>();
        }

        private static void Build()
        {
            ImportSetters.Add(typeof(Temperature).FullName, new Dictionary<int, Delegate>());

        }

        private static void LoadImportFunctions()
        {
            Load<byte>();
            Load<short>();
            Load<int>();
            Load<float>();
            Load<DateTime>();
            Load<TimeSpan>();

            Load<Acceleration, AccelerationUnit>((value, e) => Acceleration.From(value.AsFloat(), e));
            Load<Angle, AngleUnit>((value, e) => Angle.From(value.AsFloat(), e));
            Load<ElectricCurrent, ElectricCurrentUnit>((value, e) => ElectricCurrent.From(value.AsFloat(), e));
            Load<ElectricPotential, ElectricPotentialUnit>((value, e) => ElectricPotential.From(value.AsFloat(), e));
            Load<Length, LengthUnit>((value, e) => Length.From(value.AsFloat(), e));
            Load<Ratio, RatioUnit>((value, e) => Ratio.From(value.AsFloat(), e));
            Load<RotationalSpeed, RotationalSpeedUnit>((value, e) => RotationalSpeed.From(value.AsFloat(), e));
            Load<Speed, SpeedUnit>((value, e) => Speed.From(value.AsFloat(), e));
            Load<Temperature, TemperatureUnit>((value, e) => Temperature.From(value.AsFloat(), e));
            Load<Torque, TorqueUnit>((value, e) => Torque.From(value.AsFloat(), e));
            Load<Volume, VolumeUnit>((value, e) => Volume.From(value.AsFloat(), e));
        }

        private static void Load<TType, TEnum>(Func<string, TEnum, TType> func)
            where TType : struct
            where TEnum : struct, IConvertible
        {
            ImportFuncs.Add(typeof(TType), func);
        }

        private static void Load<TOut>()
        {
            ImportFuncs.Add(typeof(TOut), Func<string, TOut> b = value => value.As(typeof(TOut)));
        }

        private static void LoadReflectionProperties<T, U>()
            where T : BinaryDatapoint, new()
            where U : TextDatapoint, new()
        {
            //var setter = Setter<T, Angle>("Pitch");

            T dp = Activator.CreateInstance<T>();

            var type = typeof(T);
            ImportSetters.Add(type.FullName, new Dictionary<int, Delegate>());

            foreach (var field in GetFields(type))
            {
                ImportSetters[type.FullName][field.Item2] = Setter<T>("", field.Item4);
                var typeName = field.Item4.Name;

                switch (typeName)
                {
                    case "Angle":
                        var func = ImportFuncs[field.Item4];
//                        Setter<T, Angle>(field.Item1)(dp, ((Func<string, Angle>)func)("");
                        break;
                    default:
                        break;

                }
//                var s = Setter<T>("", field.Item4) as Func<,>;

                var del = ImportFuncs[field.Item4];

                if(del != null)
                {
                    throw new ArgumentOutOfRangeException();
                }
                var pType = field.Item4;
//Setter<BinaryDatapoint,

//                ImportSetters[type.FullName].Add(field.Item2, 
            }
            // We only need to obtain the setters on the properties being changed
            //Setter<T, Temperature>("Temperature");

                // Property Type







            Debug.Write("Finished Reflecting Class: ");
            Debug.WriteLine(type.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerable<Tuple<string, int, int, Type>> GetFields(Type type)
        {
            var props = type.GetProperties();
            var sorted = new SortedDictionary<int, Tuple<string, int, int, Type>>();

            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes<CsvFieldAttribute>();  // CsvFieldAttribute describes which column to use
                FormatAttribute format;

                if (attrs != null && attrs.Count() > 0)
                {
                    format = prop.GetCustomAttribute<FormatAttribute>();    // FormatAttribute describes the unit type, so parsing is correct. Optional: a func string (for conversion from unsupported units)

                    foreach (var attr in attrs) {
                        sorted.Add(attr.Index, new Tuple<string, int, int, Type>(prop.Name, attr.Index, format.value, prop.PropertyType));
                    }
                }
            }

            return sorted.Values.ToArray();
        }

        private static Action<T, U> Setter<T, U>(string pName)
            where T : BinaryDatapoint, new()
        {
            var p = Expression.Parameter(typeof(T));
            var p2 = Expression.Parameter(typeof(U), pName);
            var getter = Expression.Property(p, pName);

            return Expression.Lambda<Action<T, U>>(Expression.Assign(getter, p2), p, p2).Compile();
        }

        static Delegate Setter<T>(string name, Type type)
            where T : BinaryDatapoint, new()
        {
            var typeName = type.Name;

            switch (typeName)
            {
                case "Angle":
                    return Setter<T, Angle>(name);
                case "Acceleration":
                    return Setter<T, Acceleration>(name);
                default:
                    return null;
            }
        }

        public static IEnumerable<T> Convert<T, U>(IEnumerable<U> data)
            where T : BinaryDatapoint, new()
            where U : CsvDatapoint<U>, new()
        {
            // Obtain all the prop. info

            var type = typeof(T);
            foreach(U dp in data)
            {
                if (!dp.IsValid)
                {
                    continue;
                }

                var newDp = Activator.CreateInstance<T>();


                yield return newDp;
            }

            yield break;
        }
    }
}
