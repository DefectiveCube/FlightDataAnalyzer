using System;
using System.Collections;

namespace XPlaneGenConsole
{
	public static class Extensions
	{
		public static void BlockCopy<T>(this byte[] source, int offset, int size, params T[] value)
			where T: struct
		{
			Buffer.BlockCopy (value, 0, source, offset, value.Length * size);
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

		public static short AsShort(this string value, short falseValue = short.MinValue){
			short result;

			return short.TryParse(value, out result) ? result : falseValue;
		}

		public static int AsInt(this string value, int falseValue = int.MinValue)
		{
			int result;

			return int.TryParse (value, out result) ? result : falseValue;
		}

		public static float AsFloat(this string value, float falseValue = float.NaN)
		{
			float result;

			return float.TryParse (value, out result) ? result : falseValue;
		}

		public static float AsFloat(this string[] values, int index, float falseValue = float.NaN)
		{
			return AsFloat (values [index], falseValue);
		}
	}
}