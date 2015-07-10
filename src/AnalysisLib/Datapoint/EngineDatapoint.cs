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

        public override BinaryDatapoint Create()
        {
            return new EngineDatapoint();
            //throw new NotImplementedException();
        }

        // All CHT and EGT fields are in formatted of "###.#" but the tenths digit is always ZERO. Therefore, it would be advisable to store as a ushort instead of a float and reduce space consumption
        // Note:  Would save 24 bytes per datapoint (19% decrease)

        [CsvField(3)]
        [Format(TemperatureUnit.DegreeFahrenheit, "###")]
        [Storage(24, typeof(short))]
        public Temperature OilTemperature { get; set; }

        [CsvField(4)]
        [Format(PressureUnit.Undefined)]
        [Storage(25, typeof(short))]
		public Pressure OilPressure { get; set; }

        [CsvField(5)]
        [Format(RotationalSpeedUnit.RevolutionPerMinute)]
        [Storage(26, typeof(short))]
		public RotationalSpeed EngineRPM { get; set; }

        [CsvField(6)]
        [Format(PressureUnit.Undefined)]
        [Storage(3, typeof(float))]
		public Pressure EngineManifold { get; set; }

        [CsvField(7)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(4, typeof(float))]
        public Temperature EngineTIT { get; set; }

        [CsvField(8)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(5, typeof(ushort))]
        public Temperature CHT_1 { get; set; }

        [CsvField(9)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(6, typeof(ushort))]
        public Temperature CHT_2 { get; set; }

        [CsvField(10)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(7, typeof(ushort))]
        public Temperature CHT_3 { get; set; }

        [CsvField(11)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(8, typeof(ushort))]
        public Temperature CHT_4 { get; set; }

        [CsvField(12)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(9, typeof(ushort))]
        public Temperature CHT_5 { get; set; }

        [CsvField(13)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(10, typeof(ushort))]
        public Temperature CHT_6 { get; set; }

        [CsvField(14)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(11, typeof(ushort))]
        public Temperature EGT_1 { get; set; }

        [CsvField(15)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(12, typeof(ushort))]
        public Temperature EGT_2 { get; set; }

        [CsvField(16)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(13, typeof(ushort))]
        public Temperature EGT_3 { get; set; }

        [CsvField(17)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(14, typeof(ushort))]
        public Temperature EGT_4 { get; set; }

        [CsvField(18)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(15, typeof(ushort))]
        public Temperature EGT_5 { get; set; }

        [CsvField(19)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(16, typeof(ushort))]
        public Temperature EGT_6 { get; set; }

        [CsvField(20)]
        [Format(RatioUnit.Percent)]
        [Storage(17, typeof(float))]
        public Ratio EnginePercentPower { get; set; }

        [CsvField(21)]
        [Format(VolumeUnit.UsGallon)]
        [Storage(18, typeof(float))]
        public Volume FuelFlow { get; set; }

        [CsvField(22)]
        [Format(VolumeUnit.UsGallon)]
        [Storage(19, typeof(float))]
        public Volume FuelUsed { get; set; }

        [CsvField(23)]
        [Format(VolumeUnit.UsGallon)]
        [Storage(20, typeof(float))]
        public Volume FuelRemaining { get; set; }

        [CsvField(25)]
        [Storage(21, typeof(float))]
        public Ratio FuelEconomy { get; set; }

        [CsvField(26)]
        [Format(ElectricCurrentUnit.Ampere)]
        [Storage(28, typeof(byte))]
        public ElectricCurrent AlternatorCurrent_1 { get; set; }

        [CsvField(27)]
        [Format(ElectricCurrentUnit.Ampere)]
        [Storage(29, typeof(byte))]
        public ElectricCurrent AlternatorCurrent_2 { get; set; }

        [CsvField(28)]
        [Format(ElectricCurrentUnit.Ampere)]
        [Storage(27, typeof(ushort))]
        public ElectricCurrent BatteryCurrent { get; set; }

        [CsvField(29)]
        [Format(ElectricPotentialUnit.Volt)]
        [Storage(22, typeof(float))]
        public ElectricPotential BusVoltage_1 { get; set; }

        [CsvField(30)]
        [Format(ElectricPotentialUnit.Volt)]
        [Storage(23, typeof(float))]
        public ElectricPotential BusVoltage_2 { get; set; }

        [CsvField(31)]
        [Format(NumberStyles.HexNumber)]
        [Storage(30, typeof(byte))]
        public byte DiscreteInputs { get; set; }

        [CsvField(32)]
        [Format(NumberStyles.HexNumber)]
        [Storage(31, typeof(byte))]
        public byte DiscreteOutputs { get; set; }

		/*internal override void SetBytes()
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
                /*this.FuelEconomy = Data.GetSingle(88);
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
		}*/

		/*internal override byte[] GetBytes()
		{
			if (IsValid) {
				/*Data.BlockCopy (0, sizeof(long), DateTime.Ticks);
				Data.BlockCopy (8, sizeof(int), Flight, Timestamp);
				Data.BlockCopy (16, sizeof(float), EngineManifold, EngineTIT, CHT_1, CHT_2, CHT_3, CHT_4, CHT_5, CHT_6, EGT_1, EGT_2, EGT_3, EGT_4, EGT_5, EGT_6,
					EnginePercentPower, FuelFlow, FuelUsed, FuelRemaining, FuelEconomy, BusVoltage_1, BusVoltage_2);
				Data.BlockCopy (100, sizeof(short), OilTemperature, OilPressure, EngineRPM);
				Data.BlockCopy (106, sizeof(ushort), BatteryCurrent);
				Data.BlockCopy (108, sizeof(byte), AlternatorCurrent_1, AlternatorCurrent_2, DiscreteInputs, DiscreteOutputs);
			}

			return Data;
		}*/
			
		//public void Parse (string[] values)
		//{
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
		//}
	}
}