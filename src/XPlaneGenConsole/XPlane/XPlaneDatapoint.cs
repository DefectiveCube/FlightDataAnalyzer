using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnitsNet;
using UnitsNet.Units;

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

		public TimeSpan Duration{ get; set; }

        [Format(TemperatureUnit.DegreeFahrenheit)]
		public Temperature Temperature{ get; set; }

		public float Longitude{ get; set; }       

		public float Latitude{ get; set; }

        /// <summary>
        /// Height about mean sea level, in true feet. 
        /// </summary>
        [Format(LengthUnit.Foot)]
		public Length HeightMeanSeaLevel{ get; set; }

		public int HeightRadioAltimeter{ get; set; }
		
        public Ratio AileronDeflection{ get; set; }

		public Ratio ElevatorDeflection { get; set; }

		public Ratio RudderDeflection { get; set; }

        [Format(AngleUnit.Degree)]
        public Angle Pitch{ get; set; }

        /// <summary>
        /// Roll. In degrees (-180 to +180)
        /// </summary>
        [Format(AngleUnit.Degree)]
        public Angle Roll { get; set; }

        /// <summary>
        /// Heading. In degrees (0 to 360)
        /// </summary>
        [Format(AngleUnit.Degree)]
        public Angle Heading { get; set; }

		/// <summary>
		/// Knots Indicated Air Speed
		/// </summary>
		public Speed IndicatedSpeed{ get; set; }

        /// <summary>
        /// Indicated Vertical speed. Feet per minute
        /// </summary>
        /// <returns></returns>
		public Speed VerticalSpeed { get; set; }

		public int IndicatedSlip{ get; set; }

		// Turn Deg

		public float Mach{ get; set; }

        [Format(AngleUnit.Degree)]
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
        [Format(AngleUnit.Degree)]
        public Angle OBS1 { get; set; }

        /// <summary>
        /// In Degrees. 0 to 360
        /// </summary>
        /// <returns></returns>
        [Format(AngleUnit.Degree)]
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
        [Format(RatioUnit.Percent)]
        public Ratio Nav1_HorizontalDeflection { get; set; }

        /// <summary>
        /// -2.5 to 2.5. Localizer Deflection
        /// </summary>
        /// <returns></returns>
        [Format(RatioUnit.Percent)]
        public Ratio Nav2_HorizontalDeflection { get; set; }

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

        [Format(AngleUnit.Degree)]
        public Angle FlightDirectorPitch{ get; set; }

        [Format(AngleUnit.Degree)]
		public Angle FlightDirectorRoll { get; set; }

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

        [Format(PressureUnit.Psi)]
		public Pressure Barometer{ get; set; }

        /// <summary>
        /// Decision height, dialed into the radio alt., feet AGL
        /// </summary
        [Format(LengthUnit.Foot)]
        public Length DecisionHeight { get; set; }

		public bool MasterCaution{ get; set; }

		public bool MasterWarning{ get; set; }

		public bool GPWS{ get; set; }

		// Mmode 0-4
		public byte MapMode{ get; set; }

		// Mrang 0-6
		public byte MapRange{ get; set; }

        [Format(RatioUnit.Percent)]
		public Ratio ThrottleRatio{ get; set; }

        public int PropellerRPMCommand { get; set; }
        /*
		 * prop cntrl
		 * prop cntrl
		 * prop cntrl
		 * prop cntrl
		 */

        [Format(RotationalSpeedUnit.RevolutionPerMinute)]
        public RotationalSpeed PropellerRPM { get; set; }

        [Format(AngleUnit.Degree)]
		public Angle PropellerDegree{ get; set; }

		/// <summary>
		/// N1 is the low-pressure spool. Percentage
		/// </summary>
		/// <remarks>
		/// source: http://www.airliners.net/aviation-forums/tech_ops/read.main/159683/
		/// </remarks>
		[Format(RatioUnit.Percent)]
        public Ratio N1{ get; set; }

		/// <summary>
		/// N2 is the high-pressure spool. Percentage
		/// </summary>
		/// <remarks>
		/// source: http://www.airliners.net/aviation-forums/tech_ops/read.main/159683/
		/// </remarks>
		[Format(RatioUnit.Percent)]
        public Ratio N2 { get; set; }

		// MPR inch
		public int MPressureRatio{ get; set; }

		// EPR ind
		public Ratio EnginePressureRatio{ get; set; }

		/// <summary>
		/// Torque. Per Engine. ft/lbs
		/// </summary>
        [Format(TorqueUnit.Undefined)]
        public Torque Torque{ get; set; }

		/// <summary>
		/// Fuel Flow. Per Engine. Pounds per Hour
		/// </summary>
		[Format(MassUnit.Pound)]
        public Ratio FuelFlow{ get; set; }

        /// <summary>
        /// Turbine Inlet Temp. Per Engine. In Celsius
        /// </summary>
        [Format(TemperatureUnit.DegreeCelsius)]
        public Temperature TurboInletTemp{ get; set; }

        /// <summary>
        /// Exhaust Gas Temperature. Per Engine. In Celsius
        /// </summary>
        [Format(TemperatureUnit.DegreeCelsius)]
        public Temperature ExhaustGasTemp { get; set; }

		/// <summary>
		/// Cylinder Head Temperature. Per Engine. In Celsius
		/// </summary>
		[Format(TemperatureUnit.DegreeCelsius)]
        public Temperature CylinderHeadTemp { get; set; }
	}
}