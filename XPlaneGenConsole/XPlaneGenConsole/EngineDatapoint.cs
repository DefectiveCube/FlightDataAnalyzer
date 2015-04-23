using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace XPlaneGenConsole
{
    public class EngineDatapoint : Datapoint<EngineDatapoint>
    {
		public new const int BYTES_COUNT = 122;
		public new const int FIELDS_COUNT = 33;

		public EngineDatapoint() : base(FIELDS_COUNT,BYTES_COUNT)
		{ }
			
		public EngineDatapoint(byte[] data) : base(FIELDS_COUNT,BYTES_COUNT,data)
		{ }

        public short OilTemperature { get; set; }

        public short OilPressure { get; set; }

        public short EngineRPM { get; set; }

        public float EngineManifold { get; set; }

        public float EngineTIT { get; set; }

        public float CHT_1 { get; set; }

        public float CHT_2 { get; set; }

        public float CHT_3 { get; set; }

        public float CHT_4 { get; set; }

        public float CHT_5 { get; set; }

        public float CHT_6 { get; set; }

        public float EGT_1 { get; set; }

        public float EGT_2 { get; set; }

        public float EGT_3 { get; set; }

        public float EGT_4 { get; set; }

        public float EGT_5 { get; set; }

        public float EGT_6 { get; set; }

        public float EnginePercentPower { get; set; }

        public float FuelFlow { get; set; }

        public float FuelUsed { get; set; }

        public float FuelRemaining { get; set; }

        public float FuelEconomy { get; set; }

        public byte AlternatorCurrent_1 { get; set; }

        public byte AlternatorCurrent_2 { get; set; }

        public ushort BatteryCurrent { get; set; }

        public float BusVoltage_1 { get; set; }

        public float BusVoltage_2 { get; set; }

        public byte DiscreteInputs { get; set; }

        public byte DiscreteOutputs { get; set; }

        internal override void SetBytes()
        {
            if (Data.Length == EngineDatapoint.BYTES_COUNT)
            {
                this.DateTime = new DateTime(BitConverter.ToInt64(Data, 0));
                this.Flight = BitConverter.ToInt32(Data, 8);
                this.Timestamp = BitConverter.ToInt32(Data, 12);
                this.EngineManifold = Data.GetSingle(16);
                this.EngineTIT = Data.GetSingle(20);
                this.CHT_1 = Data.GetSingle(24);
                this.CHT_2 = Data.GetSingle(28);
                this.CHT_3 = Data.GetSingle(32);
                this.CHT_4 = Data.GetSingle (36);
                this.CHT_5 = Data.GetSingle (40);
                this.CHT_6 = Data.GetSingle (44);
                this.EGT_1 = Data.GetSingle (48);
                this.EGT_2 = Data.GetSingle (52);
                this.EGT_3 = Data.GetSingle (56);
                this.EGT_4 = Data.GetSingle(60);
                this.EGT_5 = Data.GetSingle(64);
                this.EGT_6 = Data.GetSingle(68);
                this.EnginePercentPower = Data.GetSingle(72);
                this.FuelFlow = Data.GetSingle(76);
                this.FuelUsed = Data.GetSingle(80);
                this.FuelRemaining = Data.GetSingle(84);
                this.FuelEconomy = Data.GetSingle(88);
                this.BusVoltage_1 = Data.GetSingle(92);
                this.BusVoltage_2 = Data.GetSingle(96);
                this.OilTemperature = BitConverter.ToInt16(Data, 100);
                this.OilPressure = BitConverter.ToInt16(Data, 102);
                this.EngineRPM = BitConverter.ToInt16(Data, 104);
                this.BatteryCurrent = BitConverter.ToUInt16(Data, 106);
                this.AlternatorCurrent_1 = Data[108];
                this.AlternatorCurrent_2 = Data[109];
                this.DiscreteInputs = Data[110];
                this.DiscreteOutputs = Data[111];
            }
        }

        internal override byte[] GetBytes()
        {
			if (IsValid) {
				Data.BlockCopy (0, sizeof(long), DateTime.Ticks);
				Data.BlockCopy (8, sizeof(int), Flight, Timestamp);
				Data.BlockCopy (16, sizeof(float), EngineManifold, EngineTIT, CHT_1, CHT_2, CHT_3, CHT_4, CHT_5, CHT_6, EGT_1, EGT_2, EGT_3, EGT_4, EGT_5, EGT_6,
					EnginePercentPower, FuelFlow, FuelUsed, FuelRemaining, FuelEconomy, BusVoltage_1, BusVoltage_2);
				Data.BlockCopy (100, sizeof(short), OilTemperature, OilPressure, EngineRPM);
				Data.BlockCopy (106, sizeof(ushort), BatteryCurrent);
				Data.BlockCopy (108, sizeof(byte), AlternatorCurrent_1, AlternatorCurrent_2, DiscreteInputs, DiscreteOutputs);
			}

			return Data;
        }

		protected override void Parse (string[] values)
		{
			// Assign fields
			Timestamp = values[0].AsInt(); //int.Parse(values[0], CultureInfo.InvariantCulture);
			DateTime = values [1].AsDateTime ().Add (values [2].AsTimeSpan ());//ParseDateTime(values[1] + " " + values[2]);
			OilTemperature = values [3].AsShort (); //ParseInt16(values[3]);
			OilPressure = values[4].AsShort();//ParseInt16(values[4]);
			EngineRPM = values[5].AsShort();//ParseInt16(values[5]);
			EngineManifold = values.AsFloat(6);
			EngineTIT = values.AsFloat(7);
			EGT_1 = values[8].AsFloat();
			EGT_2 = values.AsFloat(9);
			EGT_3 = values.AsFloat(10);
			EGT_4 = values.AsFloat(11);
			EGT_5 = values.AsFloat(12);
			EGT_6 = values.AsFloat(13);
			CHT_1 = values.AsFloat(14);
			CHT_2 = values.AsFloat(15);
			CHT_3 = values.AsFloat(16);
			CHT_4 = values.AsFloat(17);
			CHT_5 = values.AsFloat(18);
			CHT_6 = values.AsFloat(19);
			EnginePercentPower = values.AsFloat(20);
			FuelFlow = values.AsFloat(21);
			FuelUsed = values.AsFloat(22);
			FuelRemaining = values.AsFloat(23);
			// don't used item 24
			FuelEconomy = values.AsFloat(25);
			AlternatorCurrent_1 = ParseByte(values[26]);
			AlternatorCurrent_2 = ParseByte(values[27]);
			BatteryCurrent = ParseUInt16(values[28]);
			BusVoltage_1 = values.AsFloat(29);
			BusVoltage_2 = values.AsFloat(30);
			DiscreteInputs = Hexadecimal<byte>.Parse(values[31]);
			DiscreteOutputs = Hexadecimal<byte>.Parse(values[32]);
		}
    }
}