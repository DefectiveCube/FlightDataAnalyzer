using System;
using System.Globalization;
using UnitsNet;
using UnitsNet.Units;
using FDA;
using FDA.Attributes;

[assembly: Datapoint(typeof(Prototype.FlightDatapoint))]

namespace Prototype
{
	[CsvRecord(FIELDS_COUNT)]
	public sealed class FlightDatapoint : BinaryDatapoint
	{
		public const int FIELDS_COUNT = 30;
		public const int BYTES_COUNT = 87;

        /*public override BinaryDatapoint Create()
        {
            throw new NotImplementedException();
        }*/

        [CsvField(3)]
        [Format(AccelerationUnit.Undefined, "9.81 * x", "#.###")]
        [Storage(3, typeof(float))]
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
        [Format(NumberStyles.Integer ^ NumberStyles.AllowLeadingSign)]
        [Storage(29)]
        [Graph(GraphData.Discrete)]
        [Group("Flags")]
        public bool ADAHRUsed { get; set; }

        [CsvField(7)]
        [Format(NumberStyles.HexNumber)]
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
        [Group("FlightDirector")]
        public Angle FlightDirectorPitch { get; set; }

        [CsvField(12)]
        [Format(AngleUnit.Degree)]
        [Storage(10, typeof(float))]
		[Graph(GraphData.Continuous)]
        [Group("FlightDirector")]
        public Angle FlightDirectorRoll { get; set; }

        [CsvField(13)]
        [Format(AngleUnit.Degree)]
        [Storage(11, typeof(float))]
		[Graph(GraphData.Continuous)]
        [Group("Rates")]
        public Angle HeadingRate { get; set; }

        [CsvField(14)]
        [Format(LengthUnit.Foot)]
        [Storage(17, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Altitude")]
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
        [Format(NumberStyles.HexNumber)]
        [Storage(22)]
        [Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte MagStatus { get; set; }

        [CsvField(24)]
        [Format(NumberStyles.HexNumber)]
        [Storage(23)]        
        [Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte IRUStatus { get; set; }

        [CsvField(25)]
        [Format(NumberStyles.HexNumber)]
        [Storage(24)]
		[Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte MPUStatus { get; set; }

        [CsvField(26)]
        [Format(NumberStyles.HexNumber)]
        [Storage(25)]
		[Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte ADCStatus { get; set; }

        [CsvField(27)]
        [Format(NumberStyles.HexNumber)]
        [Storage(26)]
		[Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte AHRSSeq { get; set; }

        [CsvField(28)]
        [Format(NumberStyles.HexNumber)]
        [Storage(27)]
		[Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte ADCSeq { get; set; }

        [CsvField(29)]
        [Format(NumberStyles.HexNumber)]
        [Storage(28)]
		[Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte AHRSStartupMode { get; set; }
	}
}