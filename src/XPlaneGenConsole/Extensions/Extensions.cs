using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnitsNet;
using UnitsNet.Units;

namespace XPlaneGenConsole
{
    public static class Constants
    {
        public static AccelerationUnit Acceleration() { return AccelerationUnit.MeterPerSecondSquared; }

        public static AngleUnit Angle() { return AngleUnit.Degree; }

        public static LengthUnit Length { get { return LengthUnit.Meter; } }
    }

	public static class Extensions
	{
        public static string DefaultUnit()
        {
            return "";
        }

        public static bool HasIndex(this Array source, int index)
        {
            return source.Length > index;
        }

        public static double GForces(this Acceleration source) {
            return source.MeterPerSecondSquared * 9.81;
        }

        public static double Value(this Acceleration source)
        {
            return source.MeterPerSecondSquared;
        }

        public static double Value(this Angle source)
        {
            return source.Degrees;
        }

		public static void BlockCopy<T>(this byte[] source, int offset, int size, params T[] value)
			where T: struct
		{
			Buffer.BlockCopy (value, 0, source, offset, value.Length * size);
		}

		public static byte GetHexByte(this char value)
		{
			switch (value) {
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				return (byte)(value - 48);
			case 'A':
			case 'B':
			case 'C':
			case 'D':
			case 'E':
			case 'F':
				return (byte)(value - 55);
			case 'a':
			case 'b':
			case 'c':
			case 'd':
			case 'e':
			case 'f':
				return (byte)(value - 87);
			default:
				throw new ArgumentOutOfRangeException ();
			}
		}
		public static IEnumerable<T> GetHexBytes<T>(this string value)
			where T: struct
		{
			if (value.Equals ("-")) {
				yield break;
			}

			char[] chrs = value.ToCharArray ();
			int i = chrs [1] == 'x' || chrs [1] == 'X' ? 2 : 0;

			for (; i < chrs.Length; i++) {
				var a = chrs [i].GetHexByte ();
				var b = chrs.Length - i - 1;
				var c = a * (int)Math.Pow (16, b);
				var d = (T)Convert.ChangeType (c, typeof(T));

				yield return d;
			}

			yield break;
		}

		public static IEnumerable<int> GetHexBytes(this string value)
		{
			if (value.Equals ("-")) {
				yield break;
			}

			char[] chrs = value.ToCharArray ();
			int i = chrs [1] == 'x' || chrs [1] == 'X' ? 2 : 0;

			for (; i < chrs.Length; i++) {
				var a = chrs [i].GetHexByte ();
				var b = chrs.Length - i - 1;
									
				yield return a * (int)Math.Pow(16, b);
            }

			yield break;
		}

		public static byte[] GetBytes(this double value){
			return BitConverter.GetBytes (value);
		}

		public static byte[] GetBytes(this float value){
			return BitConverter.GetBytes (value);
		}

		public static byte[] GetBytes(this long value){
			return BitConverter.GetBytes (value);
		}

		public static byte[] GetBytes(this int value){
			return BitConverter.GetBytes (value);
		}

        public static Single GetSingle(this IEnumerable<byte> data, int index = 0)
        {
            return BitConverter.ToSingle(data.ToArray(), index);
        }

        public static double GetDouble(this IEnumerable<byte> data, int index = 0)
        {
            return BitConverter.ToSingle(data.ToArray(), index);
        }

        public static short GetInt16(this IEnumerable<byte> data, int index = 0)
        {
            return BitConverter.ToInt16(data.ToArray(), index);
        }

        public static int GetInt32(this IEnumerable<byte> data, int index = 0)
        {
            return BitConverter.ToInt32(data.ToArray(), index);
        }

        public static long GetInt64(this IEnumerable<byte> data, int index = 0)
        {
            return BitConverter.ToInt64(data.ToArray(), index);
        }

		/*public static bool TryParse(this string value, out byte num){
				
		}

		public static bool TryParse(this string value, out sbyte num){

		}*/

		public static bool TryParseHex(this string value, out byte num){
			num = 0;
			//var b = value.GetHexBytes ().ToArray ();


			return true;
		}

		public static bool TryParse(this string value, out short num){
			num = 0;
			int result;

			if (TryParse (value, out result)) {
				num = (short)result;
				return true;
			}

			return false;
		}

        public static bool TryParse(this string value, out int num)
        {
            num = 0;

            bool negate = value.ToCharArray()[0].Equals('-');

            if (negate)
            {
                value = value.Substring(1);
            }

            if (value.Length == 0)
            {
                return false;
            }

            var b = Encoding.ASCII.GetBytes(value);


            foreach (var n in b)
            {

                if (n < '0' || n > '9')
                {
                    return false;
                }

                num *= 10;
                num += (n - 48);
            }

            if (negate)
            {
                num *= -1;
            }

            return true;
        }

		public static bool TryParse(this string value, out ushort num)
		{
			num = 0;
			int _num = 0;

			if (TryParse (value, out num) && _num < ushort.MaxValue) {
				num = (ushort)_num;
				return true;
			}

			return false;
		}

		public static bool TryParse(this string value, out uint num){
			num = 0;

			var b = Encoding.ASCII.GetBytes (value);

			foreach (var n in b) {
				if (n < '0' || n > '9') {
					return false;
				}

				num *= 10;
				num += (uint)(n - 48);
			}

			return true;
		}

		public static bool TryParse(this string value, out ulong num){
			num = 0L;

			return false;
		}

		public static bool TryParse(this string value, out float num)
		{
			num = 0.0f;
			double v = double.NaN;

			return TryParse (value, out v);
		}

		public static bool TryParse(this string value, out double num){
			num = 0.0;

			bool negate = value.ToCharArray()[0].Equals('-');
			bool leftSide = true;
			int offset = 0;

			if (negate) {
				value = value.Substring (1);

				if (value.Length == 0)
				{
					return false;
				}
			}

			var b = Encoding.ASCII.GetBytes (value);

			foreach (var n in b) {
				if (n < '0' || n > '9') {

					if (leftSide && n == '.') {
						leftSide = false;
						continue;
					}

					return false;
				}

				num *= 10;
				num += (n - 48);

				if (!leftSide) {
					offset++;
				}
			}

			num /= (int)Math.Pow (10, offset);


			if (negate) {
				num *= -1;
			}

			return true;
		}

		public static dynamic As<T>(this string value)
		{
			return As (value, typeof(T));
		}

        public static dynamic As(this string value, Type type)
        {
            var t = type.Name;

            switch (t)
            {
                case "Boolean":
                    return value.AsBoolean(false);
                case "Byte":
                    return value.AsByte();
                case "Int16":
                    return value.AsShort();
                case "Int32":
                    return value.AsInt();
                case "Single":
                    return value.AsFloat();
                case "Double":
                    return value.AsDouble();
                case "DateTime":
                    return value.AsDateTime();
                case "TimeSpan":
                    return value.AsTimeSpan();
                default:
                    throw new Exception();
            }
        }

		public static DateTime AsDateTime(this string value){
			var dt = new DateTime (
				         value.Substring (0, 4).AsInt (),
				         value.Substring (4, 2).AsInt (),
				         value.Substring (6, 2).AsInt ()
			         );

			return dt;
		}

        public static DateTime AsDateTime(this string value, string span)
        {
            return AsDateTime(value, span.AsTimeSpan());
        }

        public static DateTime AsDateTime(this string value, TimeSpan ts)
        {
            return AsDateTime(value).Add(ts);
        }

		public static TimeSpan AsTimeSpan(this string value){
			var ts = new TimeSpan (
				         value.Substring (0, 2).AsInt (),
				         value.Substring (3, 2).AsInt (),
				         value.Substring (6, 2).AsInt ()
			         );

			return ts;
		}

        public static bool AsBoolean(this string value, bool? defaultValue = null)
        {
            int num = 0;

            if (TryParse(value, out num) && num == 0 || num == 1)
            {
                return num == 1;
            }

            if (defaultValue.HasValue)
            {
                return defaultValue.Value;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

		public static byte AsByte(this string value, byte defaultValue = byte.MinValue){
			byte result = defaultValue;

			//return value.TryParse(

			return result;
		}

		public static ushort AsUShort(this string value, ushort defaultValue = ushort.MinValue){
			ushort result;

			return value.TryParse (out result) ? result : defaultValue;
		}

		public static short AsShort(this string value, short falseValue = short.MinValue){
			short result;

			return value.TryParse(out result) ? result : falseValue;
		}

		public static int AsInt(this string value, int falseValue = int.MinValue)
		{
			int result;

			return value.TryParse (out result) ? result : falseValue;
		}

		public static float AsFloat(this string value, float defaultValue = float.NaN)
		{
			float result;

			return value.TryParse(out result) ? result : defaultValue;
		}

		public static double AsDouble(this string value, double defaultValue = double.NaN){
			double result;

			return value.TryParse (out result) ? result : defaultValue;
		}
	}
}