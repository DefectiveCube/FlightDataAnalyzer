using System;
using System.Collections;

namespace XPlaneGenConsole
{
	public static class Extensions
	{
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