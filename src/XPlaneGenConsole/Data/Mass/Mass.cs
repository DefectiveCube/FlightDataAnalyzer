using System;

namespace XPlaneGenConsole
{
	public enum MetricPrefix
	{
		Tera,
		Giga,
		Mega,
		Kilo,
		Hecto,
		Deca,
		None,
		Deci,
		Centi,
		Milli,
		Micro,
		Nano,
		Pico
	}

	public struct Mass
	{
	}

	public struct Meter
	{
		MetricPrefix Unit;

		double value;
	}

	public struct Gram
	{
		MetricPrefix Unit;

		double value;
	}

	public struct Pounds
	{

	}
}