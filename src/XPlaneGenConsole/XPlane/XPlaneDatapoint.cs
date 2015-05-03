using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XPlaneGenConsole.Data;
using XPlaneGenConsole.Attributes;

namespace XPlaneGenConsole
{
    public sealed class XPlaneDatapoint : XPlaneBaseDatapoint
    {
		public XPlaneDatapoint() : base()
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

		public Celsius Temperature{ get; set; }

		public float Longitude{ get; set; }

		public float Latitude{ get; set; }

        /// <summary>
        /// Height about mean sea level, in true feet. 
        /// </summary>
		public int HeightMeanSeaLevel{ get; set; }

		public int HeightRadioAltimeter{ get; set; }
		
        public float AileronDeflection{ get; set; }

		public float ElevatorDeflection{ get; set; }

		public float RudderDeflection{ get; set; }

		public float Pitch{ get; set; }

		/// <summary>
		/// Roll. In degrees (-180 to +180)
		/// </summary>
		public Ratio Roll { get; set; }

		/// <summary>
		/// Heading. In degrees (0 to 360 (exclusive))
		/// </summary>
		public float Heading { get; set; }

		/// <summary>
		/// Knots Indicated Air Speed
		/// </summary>
		public int IndicatedSpeed{ get; set; }

        /// <summary>
        /// Indicated Vertical speed. Feet per minute
        /// </summary>
        /// <returns></returns>
		public int VerticalSpeed{ get; set; }

		public int IndicatedSlip{ get; set; }

		// Turn Deg

		public float Mach{ get; set; }

		public Angle AngleOfAttack { get; set; }

		public bool Stall{ get; set; }

        /*
		flap rqst
		flap actul
		slat ratio
		sbrk ratio
        */
        /// <summary>
        /// 0.0 : Retracted
        /// 1.0 : Extended
        /// 1.5 : Ground-Deployed
        /// </summary>
        /// <returns></returns>
        public int SpeedBrakeRatio { get;set; }

            /*
		gear handl*/

		public bool IsNoseGearDown{ get; set; }

		public bool IsLeftGearDown{ get; set; }

		public bool IsRightGearDown{ get; set; }

        /*elev trim*/
        /// <summary>
        /// 5-Digit Integer.
        /// </summary>
        /// <returns></returns>
        public Frequency Nav1_Frequency { get; set; }
		
        /// <summary>
        /// 5-Digit Integer
        /// </summary>
        /// <returns></returns>
        public Frequency Nav2_Frequency { get; set; }

        /// <summary>
        /// None = 0
        /// NDB = 2
        /// VOR = 3
        /// LOC = 5
        /// ILS = 10
        /// </summary>
        /// <returns></returns>
        public NavType Nav1_Type { get; set; }

        public NavType Nav2_Type { get; set; }

        /// <summary>
        /// Degrees : 0 to 360
        /// </summary>
        /// <returns></returns>
        public Angle OBS1 { get; set; }

        /// <summary>
        /// In Degrees. 0 to 360
        /// </summary>
        /// <returns></returns>
        public Angle OBS2 { get; set; }

        /// <summary>
        /// 0.0: no DME found
        /// > 0.0: receiving data
        /// </summary>
        /// <returns></returns>
        public float DME1 { get; set; }

        public float DME2 { get; set; }

        /// <summary>
        /// -2.5 to 2.5. Localizer Deflection
        /// </summary>
        /// <returns></returns>
        public float Nav1_HorizontalDeflection { get; set; }

        public float Nav2_HorizontalDeflection { get; set; }

        /// <summary>
        /// 0 : Nav
        /// 1 : To
        /// 2 : From
        /// </summary>
        /// <returns></returns>
        public NavMode Nav1_Mode { get; set; }

        public NavMode Nav2_Mode { get; set; }

        /// <summary>
        /// -2.5 to 2.5. Glideslope Deflection
        /// </summary>
        /// <returns></returns>
        public float Nav1_VerticalDeflection { get; set; }

        public float Nav2_VerticalDeflection { get; set; }

        public bool OM { get; set; }

        public bool MM { get; set; }

        public bool IM { get; set; }

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

        /// <summary>
        /// Decision height, dialed into the radio alt., feet AGL
        /// </summary>
        public int DecisionHeight { get; set; }

		public bool MasterCaution{ get; set; }

		public bool MasterWarning{ get; set; }

		public bool GPWS{ get; set; }

		// Mmode 0-4
		public byte MapMode{ get; set; }

		// Mrang 0-6
		public byte MapRange{ get; set; }

		public sbyte ThrottleRatio{ get; set; }

        public int PropellerRPMCommand { get; set; }
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
		public Celsius TurboInletTemp{ get; set; }

		/// <summary>
		/// Exhaust Gas Temperature. Per Engine. In Celsius
		/// </summary>
		public Celsius ExhaustGasTemp{ get; set; }

		/// <summary>
		/// Cylinder Head Temperature. Per Engine. In Celsius
		/// </summary>
		public Celsius CylinderHeadTemp{ get; set; }


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