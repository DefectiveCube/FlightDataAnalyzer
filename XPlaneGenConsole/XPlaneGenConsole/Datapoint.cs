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
            return DateTime.ParseExact(value, "yyyyMMdd H:mm:ss", CultureInfo.InvariantCulture);
        }

        protected static float ParseFloat(string value)
        {
            float result;

            return float.TryParse(value, out result) ? result : float.NaN;
        }

        protected static int ParseInt32(string value)
        {
            int result;

            return int.TryParse(value, out result) ? result : int.MinValue;         
        }

        protected static short ParseInt16(string value)
        {
            short result;

            return short.TryParse(value, out result) ? result : short.MinValue;
        }

        protected static ushort ParseUInt16(string value)
        {
            ushort result;

            return ushort.TryParse(value, out result) ? result : ushort.MinValue;
        }

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

        internal static int SIZE { get; set; }

        internal static int LENGTH { get; set; }

        protected static int KEY { get; set; }

        protected static Random R { get; set; }

        /// <summary>
        /// True, if datapoint has usable data
        /// </summary>
        public bool IsValid { get; set; }

        public abstract int Flight { get; set; }
        public abstract int Timestamp { get; set; }
        public abstract DateTime DateTime { get; set; }

        internal static ConcurrentBag<DateTime> FlightTimes { get; set; }

        internal abstract byte[] Data { get; set; }
        internal abstract byte[] GetBytes();
        internal abstract void SetBytes();

        public void Load(byte[] data)
        {
            Data = data;
            SetBytes();
        }

        public virtual void Load(string value)
        {
            string[] values = value.Split(new char[] { ',' });
            Load(values);
        }

        public virtual void Load(string[] values)
        {

        }
    }
}
