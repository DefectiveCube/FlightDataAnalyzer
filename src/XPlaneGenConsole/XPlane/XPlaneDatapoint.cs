using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
	[CSVFile(CSVDirection.Out,false)]
    public class XPlaneDatapoint : BinaryDatapoint
    {
		public XPlaneDatapoint() : base(0)
		{

		}
        /*
        COMM

        WARN
        TEXT
        MARK
        EVNT

        DATA
        */

		[CSV(0)]
		public TimeSpan Duration{ get; set; }

		public int Temperature{ get; set; }

		public float Longitude{ get; set; }

		public float Latitude{ get; set; }

		public int HeightMeanSeaLevel{ get; set; }

		public int HeightRadioAltimeter{ get; set; }

		public float AileronDeflection{ get; set; }

		public float ElevatorDeflection{ get; set; }

		public float RudderDeflection{ get; set; }

		public float Pitch{ get; set; }

		/// <summary>
		/// Roll. In degrees (-180 to +180)
		/// </summary>
		public float Roll { get; set; }

		/// <summary>
		/// Heading. In degrees (0 to 360 (exclusive))
		/// </summary>
		public float Heading { get; set; }

		/// <summary>
		/// Knots Indicated Air Speed
		/// </summary>
		public int IndicatedSpeed{ get; set; }

		public int VerticalSpeed{ get; set; }

		public int IndicatedSlip{ get; set; }

		// Turn Deg

		public float Mach{ get; set; }

		public short AngleOfAttack { get; set; }

		public bool Stall{ get; set; }

		/*
		flap rqst
		flap actul
		slat ratio
		sbrk ratio
		gear handl*/

		public bool IsNoseGearDown{ get; set; }

		public bool IsLeftGearDown{ get; set; }

		public bool IsRightGearDown{ get; set; }

		/*elev trim
		NAV-1 frq
		NAV-2 frq
		NAV-1 type
		NAV-2 type
		OBS-1 deg
		OBS-2 deg
		DME-1 nm
		DME-2 nm
		NAV-1 h-def
		NAV-2 h-def
		NAV-1 ntf
		NAV-2 ntf
		NAV-1 v-def
		NAV-2 v-def
		OM over
		MM over
		IM over*/

		public bool FlightDirectorEnabled{ get; set; }

		public float FlightDirectorPitch{ get; set; }

		public float FlightDirectorRoll { get; set; }

		public bool KTMAC { get; set; }

		public int ThrottleMode{ get; set; }

		public int HeadingMode{ get; set; }

		public int AltimeterMode{ get; set; }

		public int HNavMode{ get; set; }

		public int GlslpMode { get; set; }

		public int BackMode{ get; set; }

		public int SpeedSelect{ get; set; }

		public int HeadingSelect{ get; set; }

		public int VerticalSpeedSelect{ get; set; }

		public int AltimeterSelect{ get; set; }

		public float Barometer{ get; set; }

		/*
		DH ft
		*/

		public bool MasterCaution{ get; set; }

		public bool MasterWarning{ get; set; }

		public bool GPWS{ get; set; }

		// Mmode 0-4
		public byte Mmode{ get; set; }

		// Mrang 0-6
		public byte Mrang{ get; set; }

		public sbyte ThrottleRatio{ get; set; }

		/*
		 * prop cntrl
		 * prop cntrl
		 * prop cntrl
		 * prop cntrl
		 */ 

		public int PropellerRPM{ get; set; }

		public int PropellerDegree{ get; set; }

		/// <summary>
		/// N1 is the low-pressure spool. Percentage
		/// </summary>
		/// <remarks>
		/// source: http://www.airliners.net/aviation-forums/tech_ops/read.main/159683/
		/// </remarks>
		public byte N1{ get; set; }

		/// <summary>
		/// N2 is the high-pressure spool. Percentage
		/// </summary>
		/// <remarks>
		/// source: http://www.airliners.net/aviation-forums/tech_ops/read.main/159683/
		/// </remarks>
		public byte N2 { get; set; }

		// MPR inch
		public int MPressureRatio{ get; set; }

		// EPR ind
		public int EnginePressureRatio{ get; set; }

		/// <summary>
		/// Torque. Per Engine. ft/lbs
		/// </summary>
		public int Torque{ get; set; }

		/// <summary>
		/// Fuel Flow. Per Engine. Pounds per Hour
		/// </summary>
		public int FuelFlow{ get; set; }

		/// <summary>
		/// Turbine Inlet Temp. Per Engine. In Celsius
		/// </summary>
		public int TurboInletTemp{ get; set; }

		/// <summary>
		/// Exhaust Gas Temperature. Per Engine. In Celsius
		/// </summary>
		public int ExhaustGasTemp{ get; set; }

		/// <summary>
		/// Cylinder Head Temperature. Per Engine. In Celsius
		/// </summary>
		public int CylinderHeadTemp{ get; set; }


		internal override byte[] GetBytes ()
		{
			throw new NotImplementedException ();
		}

		internal override void SetBytes ()
		{
			throw new NotImplementedException ();
		}
	}
}