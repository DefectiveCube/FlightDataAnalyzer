using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
	public sealed class SystemDatapoint : BinaryDatapoint, IDatapointParse
	{
		public new const int BYTES_COUNT = 105;
		public new const int FIELDS_COUNT = 38;

		public SystemDatapoint() : base(BYTES_COUNT)
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

		internal override byte[] GetBytes()
		{
			if (IsValid)
			{
				Data.BlockCopy (0, sizeof(long), DateTime.Ticks);
				Data.BlockCopy (8, sizeof(int), Flight, Timestamp, NavFreq);
				Data.BlockCopy (20, sizeof(float), LocalizerDeviation, GlideslopeDeviation, CrossTrackDeviation, AltimeterSetting,
					DistanceToActiveWaypoint, GPSHorizontalProtLimit, GPSVerticalProtLimit, HPL_SBAS, VPL_SBAS, HFQM, VFQM, MagVariance);
				Data.BlockCopy (68, sizeof(short), GroundTrack, VerticalDeviation, AltBug, VSIBug, HdgBug, NavAidBrg, OBS, DesiredTrack, 
					CourseDeviation, GPSAltitude, FmsCourse);
				Data.BlockCopy (90, sizeof(bool), FlightDirector);
				Data.BlockCopy (91, sizeof(byte), AirTemperature, Groundspeed, NavigationMode, GPSSelect, CrsSelect, NavType, GPSState);

				// If ActiveWaypoint is NOT null or whitespace, then copy the up to 8 bytes
				// else copy 8 bytes (all 0's) to overwrite any pre-existing data
				var check = !string.IsNullOrWhiteSpace (ActiveWaypoint);
				byte[] waypoint = check ? Encoding.UTF8.GetBytes (ActiveWaypoint) : new byte[8];
				Buffer.BlockCopy (waypoint, 0, Data, 97, waypoint.Length < 8 ? waypoint.Length : 8);
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

		public void Parse (string[] values)
		{
			Timestamp = values [0].AsInt ();
			DateTime = values [1].AsDateTime ().Add (values [2].AsTimeSpan ());
			AirTemperature = values [3].AsByte ();
			LocalizerDeviation = values [4].AsFloat ();
			GlideslopeDeviation = values [5].AsFloat ();
			FlightDirector = values [6].Trim () != "0";
			//AutopilotMode - column 7 is not used
			Groundspeed = values [8].AsByte ();
			GroundTrack = values [9].AsShort ();
			CrossTrackDeviation = values [10].AsFloat ();
			VerticalDeviation = values [11].AsShort ();
			AltimeterSetting = values [12].AsFloat ();
			AltBug = values [13].AsShort ();
			VSIBug = values [14].AsShort ();
			HdgBug = values [15].AsShort ();
			NavigationMode = values [17].AsByte ();
			ActiveWaypoint = values [18].Trim ();
			GPSSelect = values [19].AsByte ();
			NavAidBrg = values [20].AsShort ();
			OBS = values [21].AsShort ();
			DesiredTrack = values [22].AsShort ();
			NavFreq = values [23].AsInt ();
			CrsSelect = values [24].AsByte ();
			NavType = values [25].AsByte ();
			CourseDeviation = values [26].AsShort ();
			GPSAltitude = values [27].AsShort ();
			DistanceToActiveWaypoint = values [28].AsFloat ();
			GPSState = values [29].AsByte (); 
			GPSHorizontalProtLimit = values [30].AsFloat ();
			GPSVerticalProtLimit = values [31].AsFloat ();
			HPL_SBAS = values [32].AsFloat ();
			VPL_SBAS = values [33].AsFloat ();
			HFQM = values [34].AsFloat ();
			VFQM = values [35].AsFloat ();
			FmsCourse = values [36].AsShort ();
			MagVariance = values [37].AsFloat ();
		}
	}
}