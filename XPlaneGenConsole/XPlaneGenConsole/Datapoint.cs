using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
	internal static class DatapointExtensions
	{
		internal static void DoNothing(this string value){

		}
	}

    public abstract class Datapoint
    {
        /*protected static bool IsEmptyRow(string[] values, int index)
        {
            return values.Where((_value, i) => i > index).Any(s => !string.IsNullOrWhiteSpace(s) && !s.Equals("-"));
        }*/

		[Obsolete()]
        protected static DateTime ParseDateTime(string value)
        {
            return ParseDateTime(value, "yyyyMMdd H:mm:ss");
        }

		[Obsolete()]
        protected static DateTime ParseDateTime(string dateTime, string format)
        {
            return DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Tries to parse value as float, otherwise returns float.NaN
        /// </summary>
		 
		[Obsolete()]
        protected static float ParseFloat(string value)
        {
            float result;

            return float.TryParse(value, out result) ? result : float.NaN;
        }

        /// <summary>
        /// Tries to parse value as 32-bit integer, otherwise returns int.MinValue
        /// </summary>
		[Obsolete()]
        protected static int ParseInt32(string value)
        {
            int result;

            return int.TryParse(value, out result) ? result : int.MinValue;
        }

        /// <summary>
        /// Tries to parse value as 16-bit integer, otherwise returns short.MinValue
        /// </summary>
		[Obsolete()]
        protected static short ParseInt16(string value)
        {
            short result;

            return short.TryParse(value, out result) ? result : short.MinValue;
        }

        /// <summary>
        /// Tries to parse value as unsigned 16-bit integer, otherwise returns ushort.MinValue
        /// </summary>
		[Obsolete()]
        protected static ushort ParseUInt16(string value)
        {
            ushort result;

            return ushort.TryParse(value, out result) ? result : ushort.MinValue;
        }

        /// <summary>
        /// Tries to parse value as unsigned 8-bit integer, otherwise returns byte.MinValue
        /// </summary>
		[Obsolete()]
        protected static byte ParseByte(string value)
        {
            byte result;

            return byte.TryParse(value, out result) ? result : byte.MinValue;
        }
    }

    public abstract class Datapoint<T> : Datapoint
    {
        static Datapoint()
        {
            R = new Random();
            KEY = R.Next();
        }

        protected static int KEY { get; set; }

        protected static Random R { get; set; }

		/// <summary>
		/// The amount of bytes in the datapoint
		/// </summary>
		/// <returns></returns>
        public readonly int BYTES_COUNT = 0;

		/// <summary>
		/// The amount of CSV fields being read
		/// </summary>
		/// <returns></returns>
		public readonly int FIELDS_COUNT = 0;

		protected Datapoint() { }

		protected Datapoint(int fields, int bytes, byte[] data = null)
		{
			BYTES_COUNT = bytes;
			FIELDS_COUNT = fields;

			Data = new byte[BYTES_COUNT];

			if (data != null) {
				Load (data);
				SetBytes ();
			}
		}

        /// <summary>
        /// True, if datapoint has usable data
        /// </summary>
        public bool IsValid { get; set; }

		public virtual int Flight { get; protected set; }

		public virtual int Timestamp { get; protected set; }

		public virtual DateTime DateTime { get; protected set;}

        public virtual byte[] Data { get; protected set; }

		internal abstract byte[] GetBytes ();

		internal abstract void SetBytes ();

		public virtual void Load(byte[] data) { 
			// TODO: ensure data is valid
			Data = data;

			SetBytes ();
		}

		public virtual void Load(string value) { 
			var values = value.Split (',');

			Load (values);
		}

		public virtual void Load(string[] values) { 
			// Two conditions to verify a valid row
			// 1. There must a be specific amount of CSV fields per record (there is a constant value (SIZE) defined in each type of datapoint)
			// 2. All fields after the 3rd element should be defined. "-" signifies a null value

			IsValid = values.Length == FIELDS_COUNT && !values.Skip (3).All (v => string.IsNullOrEmpty (v) || v.Equals ("-"));
		
			// If the row is 4 fields long, then that is a new flight
			if (!IsValid) {
				if (values.Length == 4) {
					Flight = KEY = R.Next ();
					//FlightTimes.Add (ParseDateTime (values [1] + " " + values [2]));
				}               

				// Assign value to flight
				Flight = KEY;

				return;
			}

			Flight = KEY;

			Parse(values);

			GetBytes ();
		}

        public virtual Task LoadAsync(byte[] data) { return Task.FromResult(default(object)); }

        public virtual Task LoadAsync(string value) { return Task.FromResult(default(object)); }

        public virtual Task LoadAsync(string[] value) { return Task.FromResult(default(object)); }

		protected abstract void Parse (string[] values);
    }
}