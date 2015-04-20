using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public class FlightDatapoint : Datapoint<FlightDatapoint>
    {
        public new const int FIELDS_COUNT = 30;
        public new const int BYTES_COUNT = 87;

        public const int LONG_BLOCK_SIZE = 1 * sizeof(long);
        public const int INT_BLOCK_SIZE = 2 * sizeof(int);
        public const int FLOAT_BLOCK_SIZE = 14 * sizeof(float);
        public const int SHORT_BLOCK_SIZE = 2 * sizeof(short);
        public const int BOOL_BLOCK_SIZE = 1 * sizeof(bool);
        public const int BYTE_BLOCK_SIZE = 10 * sizeof(byte);

        public FlightDatapoint() { }

        public FlightDatapoint(byte[] data)
        {
            Data = data;
            SetBytes();
        }


        public override int Timestamp { get; set; }

        public override DateTime DateTime { get; set; }

        public float NormalAcceleration { get; set; }

        public float LongitudinalAcceleration { get; set; }

        public float LateralAcceleration { get; set; }

        public bool ADAHRUsed { get; set; }

        public byte AHRSSStatus { get; set; }

        public float Heading { get; set; }

        public float Pitch { get; set; }

        public float Roll { get; set; }

        public float FlightDirectorPitch { get; set; }

        public float FlightDirectorRoll { get; set; }

        public float HeadingRate { get; set; }

        public short PressureAltitude { get; set; }

        public byte IndicatedAirspeed { get; set; }

        public byte TrueAirspeed { get; set; }

        public short VerticalSpeed { get; set; }

        public float GPSLatitude { get; set; }

        public float GPSLongitude { get; set; }

        public float BodyYawRate { get; set; }

        public float BodyPitchRate { get; set; }

        public float BodyRollRate { get; set; }

        public byte MagStatus { get; set; }

        public byte IRUStatus { get; set; }

        public byte MPUStatus { get; set; }

        public byte ADCStatus { get; set; }

        public byte AHRSSeq { get; set; }

        public byte ADCSeq { get; set; }

        public byte AHRSStartupMode { get; set; }

        internal override byte[] Data { get; set; }

        public override int Flight { get; set; }

        internal override byte[] GetBytes()
        {
            this.Data = new byte[BYTES_COUNT];

            if (this.IsValid)
            {
                //8-bytes
                long[] longBlock = new long[] { DateTime.Ticks };

                // 4-bytes
                int[] intBlock = new int[] { Flight, Timestamp };
                float[] fltBlock = new float[] { NormalAcceleration, LongitudinalAcceleration, LateralAcceleration, Heading, Pitch, Roll, FlightDirectorPitch, FlightDirectorRoll,
                    HeadingRate, GPSLatitude, GPSLongitude, BodyYawRate, BodyPitchRate, BodyRollRate };

                // 2-bytes
                short[] shtBlock = new short[] { PressureAltitude, VerticalSpeed };

                // 1-bytes
                bool[] boolBlock = new bool[] { ADAHRUsed };
                byte[] byteBlock = new byte[] { AHRSSStatus, IndicatedAirspeed, TrueAirspeed, MagStatus, IRUStatus, MPUStatus, ADCStatus, AHRSSeq, ADCSeq, AHRSStartupMode };

                Buffer.BlockCopy(longBlock, 0, Data, 0, LONG_BLOCK_SIZE);
                Buffer.BlockCopy(intBlock, 0, Data, 8, INT_BLOCK_SIZE);
                Buffer.BlockCopy(fltBlock, 0, Data, 16, FLOAT_BLOCK_SIZE);
                Buffer.BlockCopy(shtBlock, 0, Data, 72, SHORT_BLOCK_SIZE);
                Buffer.BlockCopy(boolBlock, 0, Data, 76, BOOL_BLOCK_SIZE);
                Buffer.BlockCopy(byteBlock, 0, Data, 77, BYTE_BLOCK_SIZE);
            }
            else
            {
                return new byte[] { };
            }

            return Data;
        }

        [Flags]
        public enum FieldsEnum
        {
            None,
            Flight,
            Timestamp,
            NormalAcceleration,
            LongitudinalAcceleration,
            Heading,
            Pitch,
            Roll,
            FlightDirectorPitch,
            FlightDirectorRoll,
            HeadingRate,
            GPSLatitude,
            GPSLongitude,
            BodyYawRate,
            BodyPitchRate,
            BodyRollRate,
            PressureAltitude,
            VerticalSpeed,
            AHRSSStatus,
            IndicatedAirspeed,
            TrueAirspeed,
            MagStatus,
            IRUStatus,
            MPUStatus,
            ADCStatus,
            AHRSSeq,
            ADCSeq,
            AHRSStartupMode
        }

        internal override void SetBytes()
        {
            if (Data.Length == BYTES_COUNT)
            {
                this.IsValid = true;

                this.DateTime = new DateTime(BitConverter.ToInt64(Data, 0));
                this.Flight = BitConverter.ToInt32(Data, 8);
                this.Timestamp = BitConverter.ToInt32(Data, 12);
                this.NormalAcceleration = BitConverter.ToSingle(Data, 16);
                this.LongitudinalAcceleration = BitConverter.ToSingle(Data, 20);
                this.LateralAcceleration = BitConverter.ToSingle(Data, 24);
                this.Heading = BitConverter.ToSingle(Data, 28);
                this.Pitch = BitConverter.ToSingle(Data, 32);
                this.Roll = BitConverter.ToSingle(Data, 36);
                this.FlightDirectorPitch = BitConverter.ToSingle(Data, 40);
                this.FlightDirectorRoll = BitConverter.ToSingle(Data, 44);
                this.HeadingRate = BitConverter.ToSingle(Data, 48);
                this.GPSLatitude = BitConverter.ToSingle(Data, 52);
                this.GPSLongitude = BitConverter.ToSingle(Data, 56);
                this.BodyYawRate = BitConverter.ToSingle(Data, 60);
                this.BodyPitchRate = BitConverter.ToSingle(Data, 64);
                this.BodyRollRate = BitConverter.ToSingle(Data, 68);
                this.PressureAltitude = BitConverter.ToInt16(Data, 72);
                this.VerticalSpeed = BitConverter.ToInt16(Data, 74);
                this.ADAHRUsed = BitConverter.ToBoolean(Data, 76);
                this.AHRSSStatus = Data[77];
                this.IndicatedAirspeed = Data[78];
                this.TrueAirspeed = Data[79];
                this.MagStatus = Data[80];
                this.IRUStatus = Data[81];
                this.MPUStatus = Data[82];
                this.ADCStatus = Data[83];
                this.AHRSSeq = Data[84];
                this.ADCSeq = Data[85];
                this.AHRSStartupMode = Data[86];
            }
            else
            {
                this.IsValid = false;
            }
        }

        /// <summary>
        /// Uses a byte array to set the backing byte array
        /// </summary>
        /// <param name="data"></param>
        public override void Load(byte[] data)
        {
            Data = data;
            SetBytes();
        }

        /// <summary>
        /// Uses a string to load values
        /// </summary>
        /// <param name="value"></param>
        public override void Load(string value)
        {
            string[] values = value.Split(new char[] { ',' });

            Load(values);
        }

        public override void Load(string[] values)
        {
            // Two conditions to verify a valid row
            // 1. There must a be specific amount of CSV fields per record (there is a constant value (SIZE) defined in each type of datapoint)
            // 2. If any fields after the 3rd element are NOT string.empty AND NOT "-" then that row will be processed

            //IsValid = values.Length == FIELDS_COUNT && values.All(v => !string.IsNullOrEmpty(v) && v.Equals("-");
            IsValid = values.Length == FIELDS_COUNT && IsEmptyRow(values, 2);

            // If the row is 4 fields long, then that is a new flight
            if (!IsValid)
            {
                if (values.Length == 4)
                {
                    Flight = KEY = R.Next();
                    FlightTimes.Add(ParseDateTime(values[1] + " " + values[2]));
                }
                
                return; // no further information to add
            }

            // Assign value to flight
            Flight = KEY;

            Timestamp = int.Parse(values[0], CultureInfo.InvariantCulture);
            DateTime = ParseDateTime(values[1] + " " + values[2]);
            NormalAcceleration = ParseFloat(values[3]);
            LongitudinalAcceleration = ParseFloat(values[4]);
            LateralAcceleration = ParseFloat(values[5]);
            ADAHRUsed = values[6].Equals("0") || string.IsNullOrWhiteSpace(values[6]); // can this be improved?
            AHRSSStatus = Hexadecimal<byte>.Parse(values[7]);
            Heading = ParseFloat(values[8]);
            Pitch = ParseFloat(values[9]);
            Roll = ParseFloat(values[10]);
            FlightDirectorPitch = ParseFloat(values[11]);
            FlightDirectorRoll = ParseFloat(values[12]);
            HeadingRate = ParseFloat(values[13]);
            PressureAltitude = ParseInt16(values[14]);
            IndicatedAirspeed = ParseByte(values[15]);
            TrueAirspeed = ParseByte(values[16]);
            VerticalSpeed = ParseInt16(values[17]);
            GPSLatitude = ParseFloat(values[18]);
            GPSLongitude = ParseFloat(values[19]);
            BodyYawRate = ParseFloat(values[20]);
            BodyPitchRate = ParseFloat(values[21]);
            BodyRollRate = ParseFloat(values[22]);
            IRUStatus = Hexadecimal<byte>.Parse(values[23]);
            MPUStatus = Hexadecimal<byte>.Parse(values[24]);
            ADCStatus = Hexadecimal<byte>.Parse(values[25]);
            AHRSSeq = Hexadecimal<byte>.Parse(values[26]);
            ADCSeq = ParseByte(values[27]);
            AHRSStartupMode = ParseByte(values[28]);


            GetBytes();
        }
    }
}
