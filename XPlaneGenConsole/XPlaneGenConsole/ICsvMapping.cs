using System;
using System.ComponentModel;

namespace XPlaneGenConsole
{
	public interface ICsvMapping
	{
		Type Source{ get; }
		Type Destination{ get; }
		TypeConverter Converter{ get; }
	}
		

	public class EngineDatapointConverter : TypeConverter
	{

	}


	public class FlightDatapointConverter : TypeConverter
	{
		public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType)
		{
			return base.CanConvertFrom (context, sourceType);
		}

		public override bool CanConvertTo (ITypeDescriptorContext context, Type destinationType)
		{
			return base.CanConvertTo (context, destinationType);
		}

		public override object ConvertFrom (ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			return base.ConvertFrom (context, culture, value);
		}

		public override object ConvertTo (ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			return base.ConvertTo (context, culture, value, destinationType);
		}
	}

	public class SystemDatapointConverter : TypeConverter
	{

	}
}