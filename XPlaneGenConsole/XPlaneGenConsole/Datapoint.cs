using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public abstract class Datapoint
    {
        protected static bool IsEmptyRow(string[] values, int index)
        {
            return values.Where((_value, i) => i > index).Any(s => !string.IsNullOrWhiteSpace(s) && !s.Equals("-"));
        }

        protected static DateTime ParseDateTime(string value)
        {
            return ParseDateTime(value, "yyyyMMdd H:mm:ss");
        }

        protected static DateTime ParseDateTime(string dateTime, string format)
        {
            return DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Tries to parse value as float, otherwise returns float.NaN
        /// </summary>
        protected static float ParseFloat(string value)
        {
            float result;

            return float.TryParse(value, out result) ? result : float.NaN;
        }

        /// <summary>
        /// Tries to parse value as 32-bit integer, otherwise returns int.MinValue
        /// </summary>
        protected static int ParseInt32(string value)
        {
            int result;

            return int.TryParse(value, out result) ? result : int.MinValue;
        }

        /// <summary>
        /// Tries to parse value as 16-bit integer, otherwise returns short.MinValue
        /// </summary>
        protected static short ParseInt16(string value)
        {
            short result;

            return short.TryParse(value, out result) ? result : short.MinValue;
        }

        /// <summary>
        /// Tries to parse value as unsigned 16-bit integer, otherwise returns ushort.MinValue
        /// </summary>
        protected static ushort ParseUInt16(string value)
        {
            ushort result;

            return ushort.TryParse(value, out result) ? result : ushort.MinValue;
        }

        /// <summary>
        /// Tries to parse value as unsigned 8-bit integer, otherwise returns byte.MinValue
        /// </summary>
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
            FlightTimes = new ConcurrentBag<DateTime>();
        }

        /// <summary>
        /// The amount of CSV fields being read
        /// </summary>
        /// <returns></returns>
        internal static int FIELDS_COUNT { get; set; }

        /// <summary>
        /// The amount of bytes in the datapoint
        /// </summary>
        /// <returns></returns>
        internal static int BYTES_COUNT { get; set; }

        protected static int KEY { get; set; }

        protected static Random R { get; set; }

        internal static ConcurrentBag<DateTime> FlightTimes { get; set; }

        /// <summary>
        /// True, if datapoint has usable data
        /// </summary>
        public bool IsValid { get; set; }

        public abstract int Flight { get; set; }

        public abstract int Timestamp { get; set; }

        public abstract DateTime DateTime { get; set; }

        internal abstract byte[] Data { get; set; }

        internal abstract byte[] GetBytes();

        internal abstract void SetBytes();

        public virtual void Load(byte[] data) { }

        public virtual void Load(string value) { }

        public virtual void Load(string[] values) { }
    }
}
