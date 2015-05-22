using System;
using System.Globalization;
using XPlaneGenConsole;
using UnitsNet;
using UnitsNet.Units;

[assembly: Datapoint(typeof(Prototype.SystemDatapoint))]
[assembly: Model()]

namespace Prototype
{
    public sealed class SystemDatapoint : BinaryDatapoint
    {
        public const int BYTES_COUNT = 105;
        public const int FIELDS_COUNT = 38;

        public override BinaryDatapoint Create()
        {
            throw new System.NotImplementedException();
        }

        [Format(TemperatureUnit.DegreeCelsius)]
        [Graph(GraphData.Continuous)]
        [Group("Temperature")]
        public Temperature AirTemperature { get; set; }

        [Format(RatioUnit.Percent)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Deviation")]
        public Ratio LocalizerDeviation { get; set; }

        [Format(RatioUnit.Percent)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Deviation")]
        public float GlideslopeDeviation { get; set; }

        [Format(NumberStyles.Integer ^ NumberStyles.AllowLeadingSign)]
        public bool FlightDirector { get; set; }

        [Format(SpeedUnit.Knot)]
        public byte Groundspeed { get; set; }

        [Format(AngleUnit.Degree)]
        [Storage(-1, typeof(float))]
        public Angle GroundTrack { get; set; }

        [Format(LengthUnit.Kilometer, "1.852 * x")]
        [Storage(-1, typeof(float))]
        public Length CrossTrackDeviation { get; set; }

        [Format(LengthUnit.Foot)]
        [Storage(-1, typeof(short))]
        public Length VerticalDeviation { get; set; }

        [Format(PressureUnit.Undefined)]
        public Pressure AltimeterSetting { get; set; }

        [Format(LengthUnit.Foot)]
        public Length AltBug { get; set; }

        // ft/min
        public short VSIBug { get; set; }

        [Format(AngleUnit.Degree)]
        [Storage(-1, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Angle HdgBug { get; set; }

        [Format(NumberStyles.Integer ^ NumberStyles.AllowLeadingSign)]
        [Graph(GraphData.Discrete)]
        [Group("Misc.")]
        public bool DisplayMode { get; set; }

        [Format(NumberStyles.Integer ^ NumberStyles.AllowLeadingSign)]
        [Graph(GraphData.Discrete)]
        [Group("Misc.")]
        public byte NavigationMode { get; set; }

        public string ActiveWaypoint { get; set; }

        [Format(NumberStyles.Integer ^ NumberStyles.AllowLeadingSign)]
        [Graph(GraphData.Discrete)]
        [Group("Misc.")]
        public byte GPSSelect { get; set; }

        [Format(AngleUnit.Degree)]
        [Storage(-1, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Angle NavAidBrg { get; set; }

        [Format(AngleUnit.Degree)]
        [Storage(-1, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public short OBS { get; set; }

        [Format(AngleUnit.Degree)]
        [Storage(-1, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public short DesiredTrack { get; set; }

        [Format(FrequencyUnit.Kilohertz)]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Frequency NavFreq { get; set; }

        public byte CrsSelect { get; set; }

        public byte NavType { get; set; }

        [Format(AngleUnit.Degree)]
        [Storage(-1, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Angle CourseDeviation { get; set; }

        [Format(LengthUnit.Meter)]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Length GPSAltitude { get; set; }

        [Format(LengthUnit.Kilometer, "1.852 * x")]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Length DistanceToActiveWaypoint { get; set; }

        public byte GPSState { get; set; }

        [Format(LengthUnit.Meter)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public float GPSHorizontalProtLimit { get; set; }

        [Format(LengthUnit.Meter)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public float GPSVerticalProtLimit { get; set; }

        [Format(LengthUnit.Meter)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public float HPL_SBAS { get; set; }

        [Format(LengthUnit.Meter)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public float VPL_SBAS { get; set; }

        [Format(LengthUnit.Meter)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public float HFQM { get; set; }

        [Format(LengthUnit.Meter)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public float VFQM { get; set; }

        [Format(AngleUnit.Degree)]
        [Storage(-1, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public Angle FmsCourse { get; set; }

        [Format(AngleUnit.Degree)]
        [Storage(-1, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Misc.")]
        public float MagVariance { get; set; }
    }
}