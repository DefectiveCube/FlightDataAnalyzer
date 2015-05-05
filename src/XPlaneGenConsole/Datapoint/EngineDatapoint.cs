using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

using UnitsNet;
using UnitsNet.Units;

namespace XPlaneGenConsole
{
	[CsvRecord(EngineDatapoint.FIELDS_COUNT)]
	[Serializable()]
	public sealed class EngineDatapoint : BinaryDatapoint
	{
		public const int BYTES_COUNT = 122;
		public const int FIELDS_COUNT = 33;

		public EngineDatapoint () 
		{

		}

        [Format(TemperatureUnit.DegreeFahrenheit, "###")]
        public Temperature OilTemperature { get; set; }

        [Format(PressureUnit.Undefined)]
		public Pressure OilPressure { get; set; }

        [Format(RotationalSpeedUnit.RevolutionPerMinute)]
		public RotationalSpeed EngineRPM { get; set; }

        [Format(PressureUnit.Undefined)]
		public Pressure EngineManifold { get; set; }

        [Format(TemperatureUnit.DegreeFahrenheit)]
		public Temperature EngineTIT { get; set; }

        [Format(TemperatureUnit.DegreeFahrenheit)]
        public Temperature CHT_1 { get; set; }

        [Format(TemperatureUnit.DegreeFahrenheit)]
        public Temperature CHT_2 { get; set; }

        [Format(TemperatureUnit.DegreeFahrenheit)]
        public Temperature CHT_3 { get; set; }

        [Format(TemperatureUnit.DegreeFahrenheit)]
        public Temperature CHT_4 { get; set; }

        [Format(TemperatureUnit.DegreeFahrenheit)]
        public Temperature CHT_5 { get; set; }

        [Format(TemperatureUnit.DegreeFahrenheit)]
        public Temperature CHT_6 { get; set; }

        [Format(TemperatureUnit.DegreeFahrenheit)]
        public Temperature EGT_1 { get; set; }

        [Format(TemperatureUnit.DegreeFahrenheit)]
        public Temperature EGT_2 { get; set; }

        [Format(TemperatureUnit.DegreeFahrenheit)]
        public Temperature EGT_3 { get; set; }

        [Format(TemperatureUnit.DegreeFahrenheit)]
        public Temperature EGT_4 { get; set; }

        [Format(TemperatureUnit.DegreeFahrenheit)]
        public Temperature EGT_5 { get; set; }

        [Format(TemperatureUnit.DegreeFahrenheit)]
        public Temperature EGT_6 { get; set; }

        [Format(RatioUnit.Percent)]
		public Ratio EnginePercentPower { get; set; }

        [Format(VolumeUnit.UsGallon)]
		public Volume FuelFlow { get; set; }

        [Format(VolumeUnit.UsGallon)]
		public Volume FuelUsed { get; set; }

        [Format(VolumeUnit.UsGallon)]
		public Volume FuelRemaining { get; set; }

		public Ratio FuelEconomy { get; set; }

        [Format(ElectricCurrentUnit.Ampere)]
		public ElectricCurrent AlternatorCurrent_1 { get; set; }

        [Format(ElectricCurrentUnit.Ampere)]
        public ElectricCurrent AlternatorCurrent_2 { get; set; }

        [Format(ElectricCurrentUnit.Ampere)]
        public ElectricCurrent BatteryCurrent { get; set; }

        [Format(ElectricPotentialUnit.Volt)]
        public ElectricPotential BusVoltage_1 { get; set; }

        [Format(ElectricPotentialUnit.Volt)]
        public ElectricPotential BusVoltage_2 { get; set; }

		public byte DiscreteInputs { get; set; }

		public byte DiscreteOutputs { get; set; }

		internal override void SetBytes()
		{
			if (Data.Length == BYTES_COUNT)
			{
				DateTime = new DateTime(BitConverter.ToInt64(Data, 0));
				Flight = BitConverter.ToInt32(Data, 8);
				Timestamp = BitConverter.ToInt32(Data, 12);
                EngineManifold = Pressure.FromPsi(Data.GetSingle(16));
                EngineTIT = Temperature.FromDegreesFahrenheit(Data.GetSingle(20));
				CHT_1 = Temperature.FromDegreesFahrenheit(Data.GetSingle(24));
                CHT_2 = Temperature.FromDegreesFahrenheit(Data.GetSingle(28));
                CHT_3 = Temperature.FromDegreesFahrenheit(Data.GetSingle(32));
                CHT_4 = Temperature.FromDegreesFahrenheit(Data.GetSingle(36));
                CHT_5 = Temperature.FromDegreesFahrenheit(Data.GetSingle(40));
                CHT_6 = Temperature.FromDegreesFahrenheit(Data.GetSingle(44));
                EGT_1 = Temperature.FromDegreesFahrenheit(Data.GetSingle(48));
                EGT_2 = Temperature.FromDegreesFahrenheit(Data.GetSingle(52));
                EGT_3 = Temperature.FromDegreesFahrenheit(Data.GetSingle(56));
                EGT_4 = Temperature.FromDegreesFahrenheit(Data.GetSingle(60));
                EGT_5 = Temperature.FromDegreesFahrenheit(Data.GetSingle(64));
                EGT_6 = Temperature.FromDegreesFahrenheit(Data.GetSingle(68));
                EnginePercentPower = Ratio.FromPercent(Data.GetSingle(72));
                FuelFlow = Volume.FromUsGallons(Data.GetSingle(76));
                FuelUsed = Volume.FromUsGallons(Data.GetSingle(80));
                FuelRemaining = Volume.FromUsGallons(Data.GetSingle(84));
                /*this.FuelEconomy = Data.GetSingle(88);*/
                BusVoltage_1 = ElectricPotential.FromVolts(Data.GetSingle(92));
                BusVoltage_2 = ElectricPotential.FromVolts(Data.GetSingle(96));
                OilTemperature = Temperature.FromDegreesFahrenheit(Data.GetInt16(100));
                OilPressure = Pressure.FromPsi(Data.GetInt16(102));
                EngineRPM = RotationalSpeed.FromRevolutionsPerMinute(Data.GetInt16(104));
                BatteryCurrent = ElectricCurrent.FromAmperes(Data.GetInt32(106)); // needs to be unsigned
                AlternatorCurrent_1 = ElectricCurrent.FromAmperes(Data[108]);
                AlternatorCurrent_2 = ElectricCurrent.FromAmperes(Data[109]);
				DiscreteInputs = Data[110];
				DiscreteOutputs = Data[111];
			}
		}

		internal override byte[] GetBytes()
		{
			if (IsValid) {
				/*Data.BlockCopy (0, sizeof(long), DateTime.Ticks);
				Data.BlockCopy (8, sizeof(int), Flight, Timestamp);
				Data.BlockCopy (16, sizeof(float), EngineManifold, EngineTIT, CHT_1, CHT_2, CHT_3, CHT_4, CHT_5, CHT_6, EGT_1, EGT_2, EGT_3, EGT_4, EGT_5, EGT_6,
					EnginePercentPower, FuelFlow, FuelUsed, FuelRemaining, FuelEconomy, BusVoltage_1, BusVoltage_2);
				Data.BlockCopy (100, sizeof(short), OilTemperature, OilPressure, EngineRPM);
				Data.BlockCopy (106, sizeof(ushort), BatteryCurrent);
				Data.BlockCopy (108, sizeof(byte), AlternatorCurrent_1, AlternatorCurrent_2, DiscreteInputs, DiscreteOutputs);*/
			}

			return Data;
		}
			
		public void Parse (string[] values)
		{
			//Timestamp = values [0].AsInt ();
			//DateTime = values [1].AsDateTime ().Add (values [2].AsTimeSpan ());
			/*OilTemperature = values [3].AsShort ();
			OilPressure = values [4].AsShort ();
			EngineRPM = values [5].AsShort ();
			EngineManifold = values [6].AsFloat ();
			EngineTIT = values [7].AsFloat ();
			EGT_1 = values [8].AsFloat ();
			EGT_2 = values [9].AsFloat ();
			EGT_3 = values [10].AsFloat ();
			EGT_4 = values [11].AsFloat ();
			EGT_5 = values [12].AsFloat ();
			EGT_6 = values [13].AsFloat ();
			CHT_1 = values [14].AsFloat ();
			CHT_2 = values [15].AsFloat ();
			CHT_3 = values [16].AsFloat ();
			CHT_4 = values [17].AsFloat ();
			CHT_5 = values [18].AsFloat ();
			CHT_6 = values [19].AsFloat ();
			EnginePercentPower = values [20].AsFloat ();
			FuelFlow = values [21].AsFloat (21);
			FuelUsed = values [22].AsFloat (22);
			FuelRemaining = values [23].AsFloat ();
			// don't used item 24
			FuelEconomy = values [25].AsFloat ();
			AlternatorCurrent_1 = values [26].AsByte ();
			AlternatorCurrent_2 = values [27].AsByte ();
			BatteryCurrent = values [28].AsUShort (); 
			BusVoltage_1 = values [29].AsFloat ();
			BusVoltage_2 = values [30].AsFloat ();
			DiscreteInputs = values [31].GetHexBytes<byte> ().FirstOrDefault ();
			DiscreteOutputs = values [32].GetHexBytes<byte> ().FirstOrDefault ();*/
		}
	}
}