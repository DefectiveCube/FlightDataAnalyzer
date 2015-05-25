using System;
using System.Globalization;
using XPlaneGenConsole;
using UnitsNet;
using UnitsNet.Units;

[assembly: Datapoint(typeof(Prototype.SystemDatapoint))]
[assembly: Model()]

namespace Prototype
{
    [CsvRecord(FIELDS_COUNT)]
    public sealed class SystemDatapoint : BinaryDatapoint
    {
        public const int BYTES_COUNT = 105;
        public const int FIELDS_COUNT = 38;

        public override BinaryDatapoint Create()
        {
            throw new System.NotImplementedException();
        }

        [CsvField(3)]
        [Format(TemperatureUnit.DegreeCelsius)]
        [Storage(-1, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Temperature")]
        public Temperature AirTemperature { get; set; }

        [CsvField(4)]
        [Format(RatioUnit.Percent)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Deviation")]
        public Ratio LocalizerDeviation { get; set; }

        [CsvField(5)]
        [Format(RatioUnit.Percent)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Deviation")]
        public Ratio GlideslopeDeviation { get; set; }

        [CsvField(6)]
        [Format(NumberStyles.Integer ^ NumberStyles.AllowLeadingSign)]
        [Storage(-1)]
        public bool FlightDirector { get; set; }

        [CsvField(8)]
        [Format(SpeedUnit.Knot)]
        [Storage(-1, typeof(byte))]
        public Speed Groundspeed { get; set; }

        [CsvField(9)]
        [Format(AngleUnit.Degree)]
        [Storage(-1, typeof(float))]
        public Angle GroundTrack { get; set; }

        [CsvField(10)]
        [Format(LengthUnit.Kilometer, "1.852 * x")]
        [Storage(-1, typeof(float))]
        public Length CrossTrackDeviation { get; set; }

        [CsvField(11)]
        [Format(LengthUnit.Foot)]
        [Storage(-1, typeof(short))]
        public Length VerticalDeviation { get; set; }

        [CsvField(12)]
        [Format(PressureUnit.Undefined)]
        [Storage(-1,typeof(short))]
        public Pressure AltimeterSetting { get; set; }

        [CsvField(13)]
        [Format(LengthUnit.Foot)]
        [Storage(-1, typeof(short))]
        public Length AltBug { get; set; }

        // ft/min
        [CsvField(14)]
        [Storage(-1)]
        public short VSIBug { get; set; }

        [CsvField(15)]
        [Format(AngleUnit.Degree)]
        [Storage(-1, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Angle HdgBug { get; set; }

        [CsvField(16)]
        [Format(NumberStyles.Integer ^ NumberStyles.AllowLeadingSign)]
        [Storage(-1)]
        [Graph(GraphData.Discrete)]
        [Group("Misc.")]
        public bool DisplayMode { get; set; }

        [CsvField(17)]
        [Format(NumberStyles.Integer ^ NumberStyles.AllowLeadingSign)]
        [Storage(-1)]
        [Graph(GraphData.Discrete)]
        [Group("Misc.")]
        public byte NavigationMode { get; set; }

        [CsvField(18)]
        public string ActiveWaypoint { get; set; }

        [CsvField(19)]
        [Format(NumberStyles.Integer ^ NumberStyles.AllowLeadingSign)]
        [Storage(-1)]
        [Graph(GraphData.Discrete)]
        [Group("Misc.")]
        public byte GPSSelect { get; set; }

        [CsvField(20)]
        [Format(AngleUnit.Degree)]
        [Storage(-1, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Angle NavAidBrg { get; set; }

        [CsvField(21)]
        [Format(AngleUnit.Degree)]
        [Storage(-1, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Angle OBS { get; set; }

        [CsvField(22)]
        [Format(AngleUnit.Degree)]
        [Storage(-1, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Angle DesiredTrack { get; set; }

        [CsvField(23)]
        [Format(FrequencyUnit.Kilohertz)]
        [Storage(-1, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Frequency NavFreq { get; set; }

        [CsvField(24)]
        [Storage(-1)]
        public byte CrsSelect { get; set; }

        [CsvField(25)]
        [Storage(-1)]
        public byte NavType { get; set; }

        [CsvField(26)]
        [Format(AngleUnit.Degree)]
        [Storage(-1, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Angle CourseDeviation { get; set; }

        [CsvField(27)]
        [Format(LengthUnit.Meter)]
        [Storage(-1, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Length GPSAltitude { get; set; }

        [CsvField(28)]
        [Format(LengthUnit.Kilometer, "1.852 * x")]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Length DistanceToActiveWaypoint { get; set; }

        [CsvField(29)]
        public byte GPSState { get; set; }

        [CsvField(30)]
        [Format(LengthUnit.Meter)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Length GPSHorizontalProtLimit { get; set; }

        [CsvField(31)]
        [Format(LengthUnit.Meter)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Length GPSVerticalProtLimit { get; set; }

        [CsvField(32)]
        [Format(LengthUnit.Meter)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Length HPL_SBAS { get; set; }

        [CsvField(33)]
        [Format(LengthUnit.Meter)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Length VPL_SBAS { get; set; }

        [CsvField(34)]
        [Format(LengthUnit.Meter)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Length HFQM { get; set; }

        [CsvField(35)]
        [Format(LengthUnit.Meter)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Length VFQM { get; set; }

        [CsvField(36)]
        [Format(AngleUnit.Degree)]
        [Storage(-1, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Angle FmsCourse { get; set; }

        [CsvField(37)]
        [Format(AngleUnit.Degree)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Angle MagVariance { get; set; }
    }
}