using System;
using System.Globalization;

namespace XPlaneGenConsole
{

	public interface ITemperature : IConvertible, IComparable<float>, IEquatable<float>
	{
		
	}

	public struct Kelvin
	{
		public const float MinValue = 0.0f;
	}

	public struct Celsius
	{
		public const float MinValue = -273.15f;
	}
}