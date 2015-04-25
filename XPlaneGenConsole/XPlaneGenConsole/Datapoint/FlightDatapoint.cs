using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
	public sealed class FlightDatapoint : BinaryDatapoint
	{
		public new const int FIELDS_COUNT = 30;
		public new const int BYTES_COUNT = 87;

		public FlightDatapoint() : base(BYTES_COUNT)
		{
			
		}
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

		public void Parse(string[] values)
		{
				Timestamp = values [0].AsInt ();
				DateTime = values [1].AsDateTime ().Add (values [2].AsTimeSpan ());		
				NormalAcceleration = values[3].AsFloat ();
				LongitudinalAcceleration = values[4].AsFloat ();
				LateralAcceleration = values[5].AsFloat ();
				ADAHRUsed = values [6].Equals ("0") || string.IsNullOrWhiteSpace (values [6]); // can this be improved?
				AHRSSStatus = values[7].GetHexBytes<byte>().FirstOrDefault();
				Heading = values[8].AsFloat ();
				Pitch = values[9].AsFloat ();
				Roll = values[10].AsFloat ();
				FlightDirectorPitch = values[11].AsFloat ();
				FlightDirectorRoll = values[12].AsFloat ();
				HeadingRate = values[13].AsFloat ();
				PressureAltitude = values.AsShort (14);
				IndicatedAirspeed = values[15].AsByte();//ParseByte (values [15]);
				TrueAirspeed = values[16].AsByte();//ParseByte (values [16]);
				VerticalSpeed = values.AsShort (17);
				GPSLatitude = values[18].AsFloat ();
				GPSLongitude = values[19].AsFloat ();
				BodyYawRate = values[20].AsFloat ();
				BodyPitchRate = values[21].AsFloat ();
				BodyRollRate = values[22].AsFloat ();
				IRUStatus = values [23].GetHexBytes<byte> ().FirstOrDefault(); 
				MPUStatus = values [24].GetHexBytes<byte>().FirstOrDefault ();
				ADCStatus = values [25].GetHexBytes<byte>().FirstOrDefault ();
				AHRSSeq = values [26].GetHexBytes<byte>().FirstOrDefault ();
				ADCSeq = values[27].AsByte();//ParseByte (values [27]);
				AHRSStartupMode = values[28].AsByte();//ParseByte (values [28]);
		}
	}
}