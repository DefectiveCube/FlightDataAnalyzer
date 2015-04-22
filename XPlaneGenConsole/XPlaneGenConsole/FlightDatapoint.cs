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

        public FlightDatapoint() 
		{ 
			Data = new byte[BYTES_COUNT];
		}

		public FlightDatapoint(byte[] data) : base()
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
            if (IsValid)
            {
				// Irrelevant Note: I hereby claim my proficiency in being a lazy programmer with the following use of an extension method + params + generics

				Data.BlockCopy (0, sizeof(long), DateTime.Ticks);
				Data.BlockCopy (8, sizeof(int), Flight, Timestamp);
				Data.BlockCopy (16, sizeof(float), NormalAcceleration, LongitudinalAcceleration, LateralAcceleration, Heading, Pitch, Roll, FlightDirectorPitch, FlightDirectorRoll,
					HeadingRate, GPSLatitude, GPSLongitude, BodyYawRate, BodyPitchRate, BodyRollRate);
				Data.BlockCopy (72, sizeof(short), PressureAltitude, VerticalSpeed);
				Data.BlockCopy (76, sizeof(bool), ADAHRUsed);
				Data.BlockCopy (77, sizeof(byte), AHRSSStatus, IndicatedAirspeed, TrueAirspeed, MagStatus, IRUStatus, MPUStatus, ADCStatus, AHRSSeq, ADCSeq, AHRSStartupMode);
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
			IsValid = Data.Length == BYTES_COUNT;

			if (!IsValid) {
				return;
			}

			this.DateTime = new DateTime (BitConverter.ToInt64 (Data, 0));
			this.Flight = Data.GetInt32 (8);
			this.Timestamp = Data.GetInt32 (12);
			this.NormalAcceleration = Data.GetSingle (16);
			this.LongitudinalAcceleration = Data.GetSingle (20);
			this.LateralAcceleration = Data.GetSingle (24);
			this.Heading = Data.GetSingle (28);
			this.Pitch = Data.GetSingle (32);
			this.Roll = Data.GetSingle (36);
			this.FlightDirectorPitch = Data.GetSingle (40);
			this.FlightDirectorRoll = Data.GetSingle (44);
			this.HeadingRate = Data.GetSingle (48);
			this.GPSLatitude = Data.GetSingle (52);
			this.GPSLongitude = Data.GetSingle (56);
			this.BodyYawRate = Data.GetSingle (60);
			this.BodyPitchRate = Data.GetSingle (64);
			this.BodyRollRate = Data.GetSingle (68);
			this.PressureAltitude = BitConverter.ToInt16 (Data, 72);
			this.VerticalSpeed = BitConverter.ToInt16 (Data, 74);
			this.ADAHRUsed = BitConverter.ToBoolean (Data, 76);
			this.AHRSSStatus = Data [77];
			this.IndicatedAirspeed = Data [78];
			this.TrueAirspeed = Data [79];
			this.MagStatus = Data [80];
			this.IRUStatus = Data [81];
			this.MPUStatus = Data [82];
			this.ADCStatus = Data [83];
			this.AHRSSeq = Data [84];
			this.ADCSeq = Data [85];
			this.AHRSStartupMode = Data [86];
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
			string[] values = value.Split (',');

            Load(values);
        }

        public override void Load(string[] values)
		{
			// Two conditions to verify a valid row
			// 1. There must a be specific amount of CSV fields per record (there is a constant value (SIZE) defined in each type of datapoint)
			// 2. All fields after the 3rd element should be defined. "-" signifies a null value

			IsValid = values.Length == FIELDS_COUNT && !values.Skip (3).All (v => string.IsNullOrEmpty (v) || v.Equals ("-"));

			// If the row is 4 fields long, then that is a new flight
			if (!IsValid) {
				if (values.Length == 4) {
					Flight = KEY = R.Next ();
					//FlightTimes.Add (ParseDateTime (values [1] + " " + values [2]));
				}               

				// Assign value to flight
				Flight = KEY;

                return;
			}

            Flight = KEY;

            Parse(values);
        }


        public override Task LoadAsync(byte[] data)
        {
            return Task.Factory.StartNew(() => Load(data));
        }

        public override async Task LoadAsync(string value)
        {
            string[] values = value.Split(new char[] { ',' });

            await LoadAsync(values);
        }

        public override async Task LoadAsync(string[] values)
        {
            //IsValid = values.Length == FIELDS_COUNT && values.All(v => !string.IsNullOrEmpty(v) && v.Equals("-"));
            IsValid = values.Length == FIELDS_COUNT && IsEmptyRow(values, 2);

            // If the row is 4 fields long, then that is a new flight
            if (!IsValid)
            {
                if (values.Length == 4)
                {
                    Flight = KEY = R.Next();
                    //FlightTimes.Add(ParseDateTime(values[1] + " " + values[2]));
                }

                return; // no further information to add
            }

            // Assign value to flight
            Flight = KEY;

            await Task.Factory.StartNew(() => Parse(values));
        }

        public void Parse(string[] values)
        {
				Timestamp = values.AsInt (0);
				DateTime = values [1].AsDateTime ().Add (values [2].AsTimeSpan ());				// NOTE: DateTime.ParseExact is terribly slow!
				NormalAcceleration = values.AsFloat (3);
				LongitudinalAcceleration = values.AsFloat (4);
				LateralAcceleration = values.AsFloat (5);
				ADAHRUsed = values [6].Equals ("0") || string.IsNullOrWhiteSpace (values [6]); // can this be improved?
				AHRSSStatus = (byte)values[7].GetHexBytes().FirstOrDefault();
				Heading = values.AsFloat (8);
				Pitch = values.AsFloat (9);
				Roll = values.AsFloat (10);
				FlightDirectorPitch = values.AsFloat (11);
				FlightDirectorRoll = values.AsFloat (12);
				HeadingRate = values.AsFloat (13);
				PressureAltitude = values.AsShort (14);
				IndicatedAirspeed = ParseByte (values [15]);
				TrueAirspeed = ParseByte (values [16]);
				VerticalSpeed = values.AsShort (17);
				GPSLatitude = values.AsFloat (18);
				GPSLongitude = values.AsFloat (19);
				BodyYawRate = values.AsFloat (20);
				BodyPitchRate = values.AsFloat (21);
				BodyRollRate = values.AsFloat (22);
				IRUStatus = (byte)values [23].GetHexBytes ().FirstOrDefault ();
				MPUStatus = (byte)values[24].GetHexBytes().FirstOrDefault();
				ADCStatus = (byte)values[25].GetHexBytes().FirstOrDefault();
				AHRSSeq = (byte)values[26].GetHexBytes().FirstOrDefault();
				ADCSeq = ParseByte (values [27]);
				AHRSStartupMode = ParseByte (values [28]);

				GetBytes ();
        }
	}
}