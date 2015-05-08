using XPlaneGenConsole;

[assembly: Datapoint(typeof(SystemDatapoint))]

namespace Prototype
{
    public sealed class SystemDatapoint : BinaryDatapoint
    {
        public const int BYTES_COUNT = 105;
        public const int FIELDS_COUNT = 38;

        public SystemDatapoint()
        {

        }

        public byte AirTemperature { get; set; }

        public float LocalizerDeviation { get; set; }

        public float GlideslopeDeviation { get; set; }

        public bool FlightDirector { get; set; }

        public byte Groundspeed { get; set; }

        public short GroundTrack { get; set; }

        public float CrossTrackDeviation { get; set; }

        public short VerticalDeviation { get; set; }

        public float AltimeterSetting { get; set; }

        public short AltBug { get; set; }

        public short VSIBug { get; set; }

        public short HdgBug { get; set; }

        public byte NavigationMode { get; set; }

        public string ActiveWaypoint { get; set; }

        public byte GPSSelect { get; set; }

        public short NavAidBrg { get; set; }

        public short OBS { get; set; }

        public short DesiredTrack { get; set; }

        public int NavFreq { get; set; }

        public byte CrsSelect { get; set; }

        public byte NavType { get; set; }

        public short CourseDeviation { get; set; }

        public short GPSAltitude { get; set; }

        public float DistanceToActiveWaypoint { get; set; }

        public byte GPSState { get; set; }

        public float GPSHorizontalProtLimit { get; set; }

        public float GPSVerticalProtLimit { get; set; }

        public float HPL_SBAS { get; set; }

        public float VPL_SBAS { get; set; }

        public float HFQM { get; set; }

        public float VFQM { get; set; }

        public short FmsCourse { get; set; }

        public float MagVariance { get; set; }
    }
}