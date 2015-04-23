using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public class FlightDatapoint : Datapoint<FlightDatapoint>
    {
		public static FieldsEnum ParseFieldFlags = FieldsEnum.All;

        public new const int FIELDS_COUNT = 30;
        public new const int BYTES_COUNT = 87;

		public FlightDatapoint() : base(FIELDS_COUNT,BYTES_COUNT)
		{ }

		public FlightDatapoint(byte[] data) : base(FIELDS_COUNT,BYTES_COUNT,data)
		{ }

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
			LateralAcceleration,
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
            AHRSStartupMode,
			All = Flight | Timestamp | NormalAcceleration | LongitudinalAcceleration | Heading | Pitch | Roll | FlightDirectorPitch | FlightDirectorRoll
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
					
		bool HasFlag(FieldsEnum e){
			return ParseFieldFlags.HasFlag (e);
		}

        protected override void Parse(string[] values)
        {
			Timestamp = values.AsInt (0);
			DateTime = values [1].AsDateTime ().Add (values [2].AsTimeSpan ());				// NOTE: DateTime.ParseExact is terribly slow!
		
			//if (HasFlag (FieldsEnum.NormalAcceleration))
				NormalAcceleration = values.AsFloat (3);

			//if (HasFlag (FieldsEnum.LongitudinalAcceleration))
				LongitudinalAcceleration = values.AsFloat (4);
		
			//if (HasFlag (FieldsEnum.LateralAcceleration))
				LateralAcceleration = values.AsFloat (5);

			//if(HasFlag(FieldsEnum.ADAHRUsed))
				ADAHRUsed = values [6].Equals ("0") || string.IsNullOrWhiteSpace (values [6]); // can this be improved?
				
			//if (HasFlag (FieldsEnum.AHRSSStatus))
				AHRSSStatus = (byte)values[7].GetHexBytes().FirstOrDefault();

			//if (HasFlag (FieldsEnum.Heading))
				Heading = values.AsFloat (8);

			//if (HasFlag (FieldsEnum.Pitch))
				Pitch = values.AsFloat (9);

			//if (HasFlag (FieldsEnum.Roll))
				Roll = values.AsFloat (10);

			//if (HasFlag (FieldsEnum.FlightDirectorPitch))
				FlightDirectorPitch = values.AsFloat (11);

			//if (HasFlag (FieldsEnum.FlightDirectorRoll))
				FlightDirectorRoll = values.AsFloat (12);

			//if (HasFlag (FieldsEnum.HeadingRate))
				HeadingRate = values.AsFloat (13);

			//if (HasFlag (FieldsEnum.PressureAltitude))
				PressureAltitude = values.AsShort (14);

			//if (HasFlag (FieldsEnum.IndicatedAirspeed))
				IndicatedAirspeed = ParseByte (values [15]);

			//if (HasFlag (FieldsEnum.TrueAirspeed))
				TrueAirspeed = ParseByte (values [16]);

			//if(HasFlag(FieldsEnum.VerticalSpeed))
				VerticalSpeed = values.AsShort (17);

			//if(HasFlag(FieldsEnum.GPSLatitude))
				GPSLatitude = values.AsFloat (18);

			//if (HasFlag (FieldsEnum.GPSLongitude))
				GPSLongitude = values.AsFloat (19);

			//if(HasFlag(FieldsEnum.BodyYawRate))
				BodyYawRate = values.AsFloat (20);

			//if(HasFlag(FieldsEnum.BodyPitchRate))
				BodyPitchRate = values.AsFloat (21);

			//if (HasFlag (FieldsEnum.BodyRollRate))
				BodyRollRate = values.AsFloat (22);

			//if (HasFlag (FieldsEnum.IRUStatus))
				IRUStatus = (byte)values [23].GetHexBytes ().FirstOrDefault ();

			//if (HasFlag (FieldsEnum.MPUStatus))
				MPUStatus = (byte)values [24].GetHexBytes ().FirstOrDefault ();

			//if (HasFlag (FieldsEnum.ADCStatus))
				ADCStatus = (byte)values [25].GetHexBytes ().FirstOrDefault ();

			//if (HasFlag (FieldsEnum.AHRSSeq))
				AHRSSeq = (byte)values [26].GetHexBytes ().FirstOrDefault ();

			//if (HasFlag (FieldsEnum.ADCSeq))
				ADCSeq = ParseByte (values [27]);

			//if (HasFlag (FieldsEnum.AHRSStartupMode))
				AHRSStartupMode = ParseByte (values [28]);
        }
	}
}