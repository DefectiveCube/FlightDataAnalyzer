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
        private static Dictionary<Type, Invocation> ImportFuncs;

        /// <summary>
        /// Collection of Actions used to invoke the setter (through compiled expressions)
        /// </summary>
        private static Dictionary<string, Dictionary<int, Delegate>> ImportSetters;

        static Conversion()
        {
            IndexDict = new Dictionary<string, HashSet<int>>();
            //PropertyDict = new Dictionary<Tuple<string, int>, Tuple<CsvFieldAttribute, PropertyInfo>>();
            ImportFuncs = new Dictionary<Type, Invocation>();
            ImportSetters = new Dictionary<string, Dictionary<int, Delegate>>();

            LoadImportFunctions();

            // TODO: don't hardcode these, and allow custom types to be used
            LoadReflectionProperties<FlightDatapoint, FlightCsvDatapoint>();
            //LoadReflectionProperties<EngineDatapoint, EngineCsvDatapoint>();
            //LoadReflectionProperties<SystemDatapoint, SystemCsvDatapoint>();
        }

        private static void Build()
        {
        }

        private static void LoadImportFunctions()
        {
			var fp = new FlightDatapoint ();

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

			//var pitch = (Angle)ImportFuncs [typeof(Angle)].Invoke ("10.0", AngleUnit.Degree);

			//Invocations<Angle,AngleUnit,float>.Get ();

			//Setter<FlightDatapoint,Angle> ("Pitch").Invoke (fp, pitch);
		}

        private static void Load<TType, TEnum>(Func<string, TEnum, TType> func)
            where TType : struct
            where TEnum : struct, IConvertible
        {
			var i = Invocation.Use<string,TEnum,TType> (func);
			ImportFuncs.Add (typeof(TType), i);
        }

        private static void Load<TResult>()
        {
			Func<string, TResult> func = value => value.As (typeof(TResult));

			var i = Invocation.Use<string,TResult> (func);
			ImportFuncs.Add (typeof(TResult), i);
		}

        private static void LoadReflectionProperties<T, U>()
            where T : BinaryDatapoint, new()
            where U : TextDatapoint, new()
        {
            T dp = Activator.CreateInstance<T>();

            var type = typeof(T);
            ImportSetters.Add(type.FullName, new Dictionary<int, Delegate>());

            foreach (var field in GetFields(type))
            {
				Invocation invoke;

				if (!ImportFuncs.TryGetValue (field.Item4, out invoke)) {
					throw new KeyNotFoundException (string.Format ("No value on key: {0}", field.Item4.Name));
				}					
            }

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
						sorted.Add(attr.Index, new Tuple<string, int, int, Type>(prop.Name, attr.Index, format != null ? format.value : 0, prop.PropertyType));
                    }
                }
            }

            return sorted.Values.ToArray();
        }
			
        internal static Action<T, U> Setter<T, U>(string pName)
            where T : BinaryDatapoint, new()
        {
            var p = Expression.Parameter(typeof(T));
            var p2 = Expression.Parameter(typeof(U), pName);

			try{
				var getter = Expression.Property(p, pName);
				return Expression.Lambda<Action<T, U>>(Expression.Assign(getter, p2), p, p2).Compile();
			}catch(Exception ex){
				Debug.WriteLine (ex.Message);
				Debug.WriteLine (ex.StackTrace);
			}

			return null;
        }

        static dynamic Setter<T>(string name, Type type)
            where T : BinaryDatapoint, new()
		{
			var typeName = type.Name;

			switch (typeName) {
			case "Angle":
				return Setter<T, Angle> (name);
			case "Acceleration":
				return Setter<T, Acceleration> (name);
			case "ElectricCurrent":
				return Setter<T, ElectricCurrent> (name);
			case "ElectricPotential":
				return Setter<T, ElectricPotential> (name);
			case "Temperature":
				return Setter<T,Temperature> (name);
			case "Torque":				
				return Setter<T,Torque> (name);
			case "Volume":
				return Setter<T,Volume> (name);
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
			var keys = ImportSetters [type.FullName].Keys;

            foreach(U dp in data)
            {
                if (!dp.IsValid)
                {
					Debug.WriteLine ("Skipping invalid data record");
                    continue;
                }

                var newDp = Activator.CreateInstance<T>();

				foreach(var key in keys){
					var del = ImportSetters [type.FullName] [key];
				}


                yield return newDp;
            }

            yield break;
        }
    }
}
