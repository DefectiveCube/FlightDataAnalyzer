using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XPlaneGenConsole
{
	public static class Extensions
	{
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
				var c = a * (int)Math.Pow (16, b);
									
				yield return c;
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

		public static Single GetSingle(this byte[] data, int index){
			return BitConverter.ToSingle (data, index);
		}

		public static double GetDouble(this byte[] data, int index){
			return BitConverter.ToSingle (data, index);
		}

		public static int GetInt16(this byte[] data, int index){
			return BitConverter.ToInt16 (data, index);
		}

		public static int GetInt32(this byte[] data, int index){
			return BitConverter.ToInt32 (data, index);
		}

		/*public static bool TryParse(this string value, out byte num){
				
		}

		public static bool TryParse(this string value, out sbyte num){

		}*/

		public static bool TryParseHex(this string value, out byte num){
			num = 0;
			var b = value.GetHexBytes ().ToArray ();


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

		public static bool TryParse(this string value, out int num){
			num = 0;

			bool negate = value.ToCharArray () [0].Equals ('-');

			if (negate) {
				value = value.Substring (1);
			}

			if (value.Length == 0) {
				return false;
			}

			var b = Encoding.ASCII.GetBytes (value);

			foreach (var n in b) {
				
				if (n < '0' || n > '9') {
					return false;
				}

				num *= 10;
				num += (n - 48);
			}

			if (negate) {
				num *= -1;
			}

			return true;
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

		public static bool TryParse(this string value, out float num){
			num = 0.0f;

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

		public static DateTime AsDateTime(this string value){
			var dt = new DateTime (
				         value.Substring (0, 4).AsInt (),
				         value.Substring (4, 2).AsInt (),
				         value.Substring (6, 2).AsInt ()
			         );

			return dt;
		}

		public static TimeSpan AsTimeSpan(this string value){
			var ts = new TimeSpan (
				         value.Substring (0, 2).AsInt (),
				         value.Substring (3, 2).AsInt (),
				         value.Substring (6, 2).AsInt ()
			         );

			return ts;
		}

		public static short AsShort(this string value, short falseValue = short.MinValue){
			short result;

			//return short.TryParse(value, out result) ? result : falseValue;
			return value.TryParse(out result) ? result : falseValue;
		}

		public static short AsShort(this string[] values, int index, short falseValue = short.MinValue){
			return AsShort (values [index], falseValue);
		}

		public static int AsInt(this string value, int falseValue = int.MinValue)
		{
			int result;

			//return int.TryParse (value, out result) ? result : falseValue;

			return value.TryParse (out result) ? result : falseValue;
		}

		public static int AsInt(this string[] values, int index, int falseValue = int.MinValue){

			return AsInt (values [index], falseValue);
		}

		public static float AsFloat(this string value, float falseValue = float.NaN)
		{
			float result;

			//return float.TryParse (value, out result) ? result : falseValue;
			return value.TryParse(out result) ? result : falseValue;
		}

		public static float AsFloat(this string[] values, int index, float falseValue = float.NaN)
		{
			return AsFloat (values [index], falseValue);
		}
	}
}