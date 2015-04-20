using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public class SystemDatapoint : Datapoint<SystemDatapoint>
    {
        public new const int BYTES_COUNT = 105;
        public new const int FIELDS_COUNT = 38;

        public SystemDatapoint() { }

        public SystemDatapoint(byte[] data)
        {
            Data = data;
            SetBytes();
        }

        public override int Timestamp { get; set; }

        public override DateTime DateTime { get; set; }

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

        internal override byte[] Data { get; set; }
        
        public override int Flight { get; set; }

        internal override byte[] GetBytes()
        {
            Data = new byte[BYTES_COUNT];

            if (IsValid)
            {
                //8-bytes
                long[] longBlock = new long[] { DateTime.Ticks };

                //4-bytes
                int[] intBlock = new int[] { Flight, Timestamp, NavFreq };
                float[] fltBlock = new float[] { LocalizerDeviation, GlideslopeDeviation, CrossTrackDeviation, AltimeterSetting,
                    DistanceToActiveWaypoint, GPSHorizontalProtLimit, GPSVerticalProtLimit, HPL_SBAS, VPL_SBAS, HFQM, VFQM, MagVariance };

                //2-bytes
                short[] shtBlock = new short[] { GroundTrack, VerticalDeviation, AltBug, VSIBug, HdgBug, NavAidBrg, OBS, DesiredTrack, CourseDeviation, GPSAltitude, FmsCourse };

                //1-byte
                bool[] boolBlock = new bool[] { FlightDirector };
                byte[] byteBlock = new byte[] { AirTemperature, Groundspeed, NavigationMode, GPSSelect, CrsSelect, NavType, GPSState };

                Buffer.BlockCopy(longBlock, 0, Data, 0, 1 * sizeof(long));
                Buffer.BlockCopy(intBlock, 0, Data, 8, 3 * sizeof(int));
                Buffer.BlockCopy(fltBlock, 0, Data, 20, 12 * sizeof(float));
                Buffer.BlockCopy(shtBlock, 0, Data, 68, 11 * sizeof(short));
                Buffer.BlockCopy(boolBlock, 0, Data, 90, 1);
                Buffer.BlockCopy(byteBlock, 0, Data, 91, 6 * sizeof(byte));

                if (!string.IsNullOrWhiteSpace(ActiveWaypoint))
                {
                    byte[] waypoint = Encoding.UTF8.GetBytes(ActiveWaypoint);

                    Buffer.BlockCopy(waypoint, 0, Data, 97, waypoint.Length < 8 ? waypoint.Length : 8);
                }
            }
            else
            {
                return new byte[] { };
            }

            return Data;
        }

        internal override void SetBytes()
        {
            DateTime = new DateTime(BitConverter.ToInt64(Data, 0));
            this.Flight = BitConverter.ToInt32(Data, 4);
            this.Timestamp = BitConverter.ToInt32(Data, 8);
            this.NavFreq = BitConverter.ToInt32(Data, 12);
            this.LocalizerDeviation = BitConverter.ToSingle(Data, 16);
            this.GlideslopeDeviation = BitConverter.ToSingle(Data, 20);
            this.CrossTrackDeviation = BitConverter.ToSingle(Data, 24);
            this.AltimeterSetting = BitConverter.ToSingle(Data, 28);
            this.DistanceToActiveWaypoint = BitConverter.ToSingle(Data, 32);
            this.GPSHorizontalProtLimit = BitConverter.ToSingle(Data, 36);
            this.GPSVerticalProtLimit = BitConverter.ToSingle(Data, 40);
            this.HPL_SBAS = BitConverter.ToSingle(Data, 44);
            this.VPL_SBAS = BitConverter.ToSingle(Data, 48);
            this.HFQM = BitConverter.ToSingle(Data, 52);
            this.VFQM = BitConverter.ToSingle(Data, 56);
            this.MagVariance = BitConverter.ToSingle(Data, 62);
            this.GroundTrack = BitConverter.ToInt16(Data, 66);
            this.VerticalDeviation = BitConverter.ToInt16(Data, 68);
            this.AltBug = BitConverter.ToInt16(Data, 70);
            this.VSIBug = BitConverter.ToInt16(Data, 72);
            this.HdgBug = BitConverter.ToInt16(Data, 74);
            this.NavAidBrg = BitConverter.ToInt16(Data, 76);
            this.OBS = BitConverter.ToInt16(Data, 78);
            this.DesiredTrack = BitConverter.ToInt16(Data, 80);
            this.CourseDeviation = BitConverter.ToInt16(Data, 82);
            this.GPSAltitude = BitConverter.ToInt16(Data, 84);
            this.FmsCourse = BitConverter.ToInt16(Data, 86);
            this.FlightDirector = BitConverter.ToBoolean(Data, 88);
            this.AirTemperature = Data[89];
            this.Groundspeed = Data[90];
            this.NavigationMode = Data[91];
            this.GPSSelect = Data[92];
            this.CrsSelect = Data[93];
            this.NavType = Data[94];
            this.GPSState = Data[95];
        }

        public override void Load(string value)
        {
            string[] values = value.Split(new char[] { ',' });

            Load(values);
        }

        public override void Load(string[] values)
        {
            // Two conditions to verify a valid row
            // 1. There must a be specific amount of CSV fields per record (defined in each type of datapoint)
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

            Timestamp = int.Parse(values[0], CultureInfo.InvariantCulture);
            DateTime = ParseDateTime(values[1] + " " + values[2]);
            AirTemperature = ParseByte(values[3]);
            LocalizerDeviation = ParseFloat(values[4]);
            GlideslopeDeviation = ParseFloat(values[5]);
            FlightDirector = values[6].Trim() != "0"; // probably needs to be examined further
            //AutopilotMode - column 7 is not used
            Groundspeed = ParseByte(values[8]);
            GroundTrack = ParseInt16(values[9]);
            CrossTrackDeviation = ParseFloat(values[10]);
            VerticalDeviation = ParseInt16(values[11]);
            AltimeterSetting = ParseFloat(values[12]);
            AltBug = ParseInt16(values[13]);
            VSIBug = ParseInt16(values[14]);
            HdgBug = ParseInt16(values[15]);
            NavigationMode = ParseByte(values[17]);
            ActiveWaypoint = values[18].Trim();
            GPSSelect = ParseByte(values[19]);
            NavAidBrg = ParseInt16(values[20]);
            OBS = ParseInt16(values[21]);
            DesiredTrack = ParseInt16(values[22]);
            NavFreq = ParseInt32(values[23]);
            CrsSelect = ParseByte(values[24]);
            NavType = ParseByte(values[25]);
            CourseDeviation = ParseInt16(values[26]);
            GPSAltitude = ParseInt16(values[27]);
            DistanceToActiveWaypoint = ParseFloat(values[28]);
            GPSState = ParseByte(values[29]);
            GPSHorizontalProtLimit = ParseFloat(values[30]);
            GPSVerticalProtLimit = ParseFloat(values[31]);
            HPL_SBAS = ParseFloat(values[32]);
            VPL_SBAS = ParseFloat(values[33]);
            HFQM = ParseFloat(values[34]);
            VFQM = ParseFloat(values[35]);
            FmsCourse = ParseInt16(values[36]);
            MagVariance = ParseFloat(values[37]);

            GetBytes();
        }
    }
}