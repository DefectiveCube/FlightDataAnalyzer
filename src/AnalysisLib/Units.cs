using System;
using System.Collections.Generic;
using UnitsNet;
using UnitsNet.Units;

namespace XPlaneGenConsole
{
	public class UnitInfo
	{			
		public static IEnumerable<string> GetUnitTypes()
		{
			yield return "Acceleration";
			yield return "Angle";
			yield return "Area";
			yield return "Density";
			yield return "Duration";
			yield return "Electric Current";
			yield return "Electric Potential";
			yield return "Electrical Resistance";
			yield return "Energy";
			yield return "Flow";
			yield return "Force";
			yield return "Frequency";
			yield return "Information";
			yield return "Kinematic Viscosity";
			yield return "Length";
			yield return "Level";
			yield return "Mass";
			yield return "Power";
			yield return "PowerRatio";
			yield return "Pressure";
			yield return "Ratio";
			yield return "Rotational Speed";
			yield return "Specific Weight";
			yield return "Speed";
			yield return "Temperature";
			yield return "Torque";
			yield return "Volume";
		}

		public static IEnumerable<string> GetUnitNames(Type type)
		{
			var name = type.Name;

			switch (name) {
			case "Acceleration":
				return GetAccelerationUnitNames ();
			case "Angle":
				return GetAngleUnitNames ();
			case "Temperature":
				return GetTemperatureUnitNames ();
			case "Volume":
				return GetVolumeUnitNames ();
			}

			return new string[]{ };
		}

		public static IEnumerable<string> GetAccelerationUnitNames()
		{
			yield return "Meter Per Second Squared";
			yield break;
		}

		public static IEnumerable<string> GetAngleUnitNames()
		{
			yield return "Degree";
			yield return "Radian";
			yield return "Gradient";
			yield break;
		}

		public static IEnumerable<string> GetTemperatureUnitNames()
		{
			yield return "Kelvin";
			yield return "Celsius";
			yield return "Fahrenheit";
			yield break;
		}

		public static IEnumerable<string> GetVolumeUnitNames()
		{
			yield return "Cubic Meter";
			yield break;
		}
	}
}