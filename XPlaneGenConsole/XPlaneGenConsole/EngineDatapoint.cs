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

		private const int LONG_BLOCK_SIZE = 1 * sizeof(long);
		private const int INT_BLOCK_SIZE = 2 * sizeof(int);
		private const int FLOAT_BLOCK_SIZE = 21 * sizeof(float);
		private const int SHORT_BLOCK_SIZE = 3 * sizeof(short);
		private const int USHORT_BLOCK_SIZE = sizeof(ushort);
		private const int BYTE_BLOCK_SIZE = 4 * sizeof(byte);

        static EngineDatapoint()
		{
			FlightTimes = new ConcurrentBag<DateTime> ();
		}

        public EngineDatapoint() { }
        public EngineDatapoint(byte[] data)
        {
            this.Data = data;
            SetBytes();
        }

        public override int Timestamp { get; set; }

        public override DateTime DateTime { get; set; }

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

        internal override byte[] Data { get; set; }

        public override int Flight { get; set; }

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
            this.Data = new byte[BYTES_COUNT];

			if (this.IsValid) {
				//8-bytes
				long[] longBlock = new long[] { DateTime.Ticks };

				//4-bytes
				int[] intBlock = new int[] { Flight, Timestamp };
				float[] fltBlock = new float[] {
					EngineManifold,
					EngineTIT,
					CHT_1,
					CHT_2,
					CHT_3,
					CHT_4,
					CHT_5,
					CHT_6,
					EGT_1,
					EGT_2,
					EGT_3,
					EGT_4,
					EGT_5,
					EGT_6,
					EnginePercentPower,
					FuelFlow,
					FuelUsed,
					FuelRemaining,
					FuelEconomy,
					BusVoltage_1,
					BusVoltage_2
				};

				//2-bytes
				short[] shtBlock = new short[] { OilTemperature, OilPressure, EngineRPM };
				ushort[] ushtBlock = new ushort[] { BatteryCurrent };

				//1-byte         
				byte[] byteBlock = new byte[] {
					AlternatorCurrent_1,
					AlternatorCurrent_2,
					DiscreteInputs,
					DiscreteOutputs
				};

				Buffer.BlockCopy (longBlock, 0, this.Data, 0, LONG_BLOCK_SIZE);
				Buffer.BlockCopy (intBlock, 0, this.Data, 8, INT_BLOCK_SIZE);
				Buffer.BlockCopy (fltBlock, 0, this.Data, 16, FLOAT_BLOCK_SIZE);
				Buffer.BlockCopy (shtBlock, 0, this.Data, 100, SHORT_BLOCK_SIZE);
				Buffer.BlockCopy (ushtBlock, 0, this.Data, 106, USHORT_BLOCK_SIZE);
				Buffer.BlockCopy (byteBlock, 0, this.Data, 108, BYTE_BLOCK_SIZE);
			}
            else
            {
                //Debug.WriteLine("Invalid Row");
                return new byte[] { };
            }

            return this.Data;
        }

        public override void Load(string value)
        {
            string[] values = value.Split(new char[] { ',' });

            Load(values);
        }

        public override void Load(string[] values)
        {
            // Two conditions to process a valid row
            // 1. There must a be specific amount of CSV fields per record (statically defined in each type of datapoint<T>)
            // 2. If any fields past the 3rd column are NOT string.empty AND NOT "-" then that row will be processed
            IsValid = values.Length == FIELDS_COUNT && IsEmptyRow(values, 2);

            // If the row is 4 fields long, then that is a new flight
            if (!IsValid)
            {
                if (values.Length == 4)
                {
                    Flight = KEY = R.Next();
                    FlightTimes.Add(ParseDateTime(values[1] + " " + values[2]));
                }

                // There is no further data to add
                return;
            }

            // Assign value to flight
            Flight = KEY;

            // Assign fields
            Timestamp = int.Parse(values[0], CultureInfo.InvariantCulture);
            DateTime = ParseDateTime(values[1] + " " + values[2]);
            OilTemperature = ParseInt16(values[3]);
            OilPressure = ParseInt16(values[4]);
            EngineRPM = ParseInt16(values[5]);
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