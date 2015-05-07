using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using UnitsNet.Units;

namespace XPlaneGenConsole
{
	[CsvRecord()]
    [Serializable()]
	public sealed class FlightDatapoint : BinaryDatapoint
	{
		public const int FIELDS_COUNT = 30;
		public const int BYTES_COUNT = 87;

		public FlightDatapoint()
		{
           
		}

        [CsvField(3)]
        [Format(AccelerationUnit.Undefined, "9.81 * x", "#.###")]
        [Storage(3,typeof(float))]
		[Graph(GraphData.Continuous)]
        [Group("Acceleration")]
        public Acceleration NormalAcceleration { get; set; }

        [CsvField(4)]
        [Format(AccelerationUnit.Undefined, "9.81 * x", "#.###")]
        [Storage(4, typeof(float))]
		[Graph(GraphData.Continuous)]
        [Group("Acceleration")]
        public Acceleration LongitudinalAcceleration { get; set; }

        [CsvField(5)]
        [Format(AccelerationUnit.Undefined, "9.81 * x", "#.###")]
        [Storage(5, typeof(float))]
		[Graph(GraphData.Continuous)]
        [Group("Acceleration")]
        public Acceleration LateralAcceleration { get; set; }

        [CsvField(6)]
        [Storage(-1)]
		[Graph(GraphData.Discrete)]
        [Group("Flags")]
		public bool ADAHRUsed { get; set; }

        [CsvField(7)]
        [Storage(19)]
        [Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte AHRSSStatus { get; set; }

        [CsvField(8)]
        [Format(AngleUnit.Degree)]
        [Storage(6, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Vectors")]
        public Angle Heading { get; set; }

        [CsvField(9)]
        [Format(AngleUnit.Degree)]
        [Storage(7, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Vectors")]
        public Angle Pitch { get; set; }

        [CsvField(10)]
        [Format(AngleUnit.Degree)]
        [Storage(8, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Vectors")]
        public Angle Roll { get; set; }

        [CsvField(11)]
        [Format(AngleUnit.Degree)]
        [Storage(9, typeof(float))]
        [Graph(GraphData.Continuous)]
        public Angle FlightDirectorPitch { get; set; }

        [CsvField(12)]
        [Format(AngleUnit.Degree)]
        [Storage(10, typeof(float))]
		[Graph(GraphData.Continuous)]
        public Angle FlightDirectorRoll { get; set; }

        [CsvField(13)]
        [Format(AngleUnit.Degree)]
        [Storage(11, typeof(float))]
		[Graph(GraphData.Continuous)]
        public Angle HeadingRate { get; set; }

        [CsvField(14)]
        [Format(LengthUnit.Foot)]
        [Storage(17, typeof(short))]
        [Graph(GraphData.Continuous)]
        public Length PressureAltitude { get; set; }

        [CsvField(15)]
        [Format(SpeedUnit.Knot)]
        [Storage(20, typeof(byte))]
		[Graph(GraphData.Continuous)]
        [Group("Speed")]
		public Speed IndicatedAirspeed { get; set; }

        [CsvField(16)]
        [Format(SpeedUnit.Knot)]
        [Storage(21, typeof(byte))]
		[Graph(GraphData.Continuous)]
        [Group("Speed")]
        public Speed TrueAirspeed { get; set; }

        [CsvField(17)]
        [Format(SpeedUnit.FootPerSecond)]
        [Storage(18, typeof(short))]
		[Graph(GraphData.Continuous)]
        [Group("Speed")]
        public Speed VerticalSpeed { get; set; }

        [CsvField(18)]
        [Format(AngleUnit.Degree)]
        [Storage(12, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("GPS")]
        public Angle GPSLatitude { get; set; }

        [CsvField(19)]
        [Format(AngleUnit.Degree)]
        [Storage(13, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("GPS")]
        public Angle GPSLongitude { get; set; }

        [CsvField(20)]
        [Format(AngleUnit.Degree)]
        [Storage(14, typeof(float))]
		[Graph(GraphData.Continuous)]
        [Group("Body Rates")]
        public Angle BodyYawRate { get; set; }

        [CsvField(21)]
        [Format(AngleUnit.Degree)]
        [Storage(15, typeof(float))]
		[Graph(GraphData.Continuous)]
        [Group("Body Rates")]
        public Angle BodyPitchRate { get; set; }

        [CsvField(22)]
        [Format(AngleUnit.Degree)]
        [Storage(16, typeof(float))]
		[Graph(GraphData.Continuous)]
        [Group("Body Rates")]
        public Angle BodyRollRate { get; set; }

        [CsvField(23)]
        [Storage(22)]
        [Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte MagStatus { get; set; }

        [CsvField(24)]
        [Storage(23)]
        [Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte IRUStatus { get; set; }

        [CsvField(25)]
        [Storage(24)]
		[Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte MPUStatus { get; set; }

        [CsvField(26)]
        [Storage(25)]
		[Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte ADCStatus { get; set; }

        [CsvField(27)]
        [Storage(26)]
		[Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte AHRSSeq { get; set; }

        [CsvField(28)]
        [Storage(27)]
		[Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte ADCSeq { get; set; }

        [CsvField(29)]
        [Storage(28)]
		[Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte AHRSStartupMode { get; set; }

		internal override byte[] GetBytes()
		{
            if (IsValid)
            {
                // Irrelevant Note: I hereby claim my proficiency in being a lazy programmer with the following use of an extension method + params + generics


/*                Data.BlockCopy(0, sizeof(long), DateTime.Ticks);
                Data.BlockCopy(8, sizeof(int), Flight, Timestamp);

                //NormalAcceleration.
                /*Data.BlockCopy (16, sizeof(float), NormalAcceleration, LongitudinalAcceleration, LateralAcceleration, Heading, Pitch, Roll, FlightDirectorPitch, FlightDirectorRoll,
					HeadingRate, GPSLatitude, GPSLongitude, BodyYawRate, BodyPitchRate, BodyRollRate);
				Data.BlockCopy (72, sizeof(short), PressureAltitude, VerticalSpeed);
				
				Data.BlockCopy (77, sizeof(byte), AHRSSStatus, IndicatedAirspeed, TrueAirspeed, MagStatus, IRUStatus, MPUStatus, ADCStatus, AHRSSeq, ADCSeq, AHRSStartupMode);
			
                Data.BlockCopy(76, sizeof(bool), ADAHRUsed);*/
            }

			return Data;
		}

		internal override void SetBytes()
		{
			IsValid = Data.Length == BYTES_COUNT;

			if (!IsValid) {
				return;
			}

			DateTime = new DateTime (BitConverter.ToInt64 (Data, 0));
			Flight = Data.GetInt32 (8);
			Timestamp = Data.GetInt32 (12);
            NormalAcceleration = Acceleration.FromMeterPerSecondSquared(9.81 * Data.GetSingle(16));
            LongitudinalAcceleration = Acceleration.FromMeterPerSecondSquared(9.81 * Data.GetSingle(20));
            LateralAcceleration = Acceleration.FromMeterPerSecondSquared(9.81 * Data.GetSingle(24));
            Heading = Angle.FromDegrees(Data.GetSingle(28));
            Pitch = Angle.FromDegrees(Data.GetSingle(32));
            Roll = Angle.FromDegrees(Data.GetSingle(36));
            FlightDirectorPitch = Angle.FromDegrees(Data.GetSingle(40));
            FlightDirectorRoll = Angle.FromDegrees(Data.GetSingle(44));
            HeadingRate = Angle.FromDegrees(Data.GetSingle(48));
            GPSLatitude = Angle.FromDegrees(Data.GetSingle(52));
            GPSLongitude = Angle.FromDegrees(Data.GetSingle(56));
            BodyYawRate = Angle.FromDegrees(Data.GetSingle(60));
            BodyPitchRate = Angle.FromDegrees(Data.GetSingle(64));
            BodyRollRate = Angle.FromDegrees(Data.GetSingle(68));
            PressureAltitude = Length.FromFeet(Data.GetSingle(72));
            VerticalSpeed = Speed.FromFeetPerSecond(Data.GetSingle(76) / 60);
            ADAHRUsed = BitConverter.ToBoolean(Data, 76);
            AHRSSStatus = Data[77];
            IndicatedAirspeed = Speed.FromKnots(Data[78]);
            TrueAirspeed = Speed.FromKnots(Data[79]);
            MagStatus = Data [80];
			IRUStatus = Data [81];
			MPUStatus = Data [82];
			ADCStatus = Data [83];
			AHRSSeq = Data [84];
			ADCSeq = Data [85];
			AHRSStartupMode = Data [86];
		}
	}
}