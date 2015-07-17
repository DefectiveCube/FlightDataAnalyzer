using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnitsNet;
using UnitsNet.Units;

namespace FDA.Extensions
{
	public static class UnitsExtensions
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
                case "String":
                    return value;
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

        public static bool IsNaN(this double d)
        {
            return double.IsNaN(d);
        }

        public static double AsDouble(this char a)
        {
            // Only valid chars are digits
            switch (a)
            {
                case '0':
                    return 0.0;
                case '1':
                    return 1.0;
                case '2':
                    return 2.0;
                case '3':
                    return 3.0;
                case '4':
                    return 4.0;
                case '5':
                    return 5.0;
                case '6':
                    return 6.0;
                case '7':
                    return 7.0;
                case '8':
                    return 8.0;
                case '9':
                    return 9.0;
                default:
                    return double.NaN;
            }
        }

        public static double AsDouble(char a, char b, bool allowNegative = true)
        {
            // First char could be '-' or digit
            // Second char must be digit
            double c = a.AsDouble();
            double d = b.AsDouble();

            if (!double.IsNaN(c) && !double.IsNaN(d))
            {
                return 10.0 * c + d;
            }
            else if (allowNegative && c == '-' && !double.IsNaN(d))
            {
                return -1.0 * d;
            }
            else
            {
                return double.NaN;
            }
        }

        public static double AsDouble(char a, char b, char c, bool allowNegative = true)
        {
            // First char could be - or digit
            // Second char could be '.' if first is digit
            // Third char must be digit
            double d = a.AsDouble();
            double e = b.AsDouble();
            double f = c.AsDouble();

            if (!d.IsNaN() && !e.IsNaN() && !f.IsNaN())
            {
                return 100.0 * d + 10.0 * e + f;
            }
            else if (!d.IsNaN() && e == '.' && !f.IsNaN())
            {
                return d + f / 10.0;
            }
            else if (d == '-' && !e.IsNaN() && !f.IsNaN())
            {
                return -1 * (e * 10.0 + f);
            }
            else
            {
                return double.NaN;
            }
        }

        public static double AsDouble(char a, char b, char c, char d, bool allowNegative = true)
        {
            throw new NotImplementedException();
        }
	}
}