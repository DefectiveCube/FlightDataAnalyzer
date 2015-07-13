using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDA.Attributes;
using UnitsNet;
using UnitsNet.Units;

namespace FDA
{
    public sealed class XPlaneDatapoint : XPlaneBaseDatapoint
    {

        /*public override BinaryDatapoint Create()
        {
            return new XPlaneDatapoint();
        }*/

		[CsvField(0)]
		[Format(DurationUnit.Second)]
		[Graph(GraphData.Continuous)]
		public TimeSpan Duration{ get; set; }

		[CsvField(1)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
		[Graph(GraphData.Continuous)]
		public Temperature Temperature{ get; set; }

		[CsvField(2)]
		[Format(AngleUnit.Degree)]
		[Graph(GraphData.Continuous)]
		public Angle Longitude{ get; set; }       

		[CsvField(3)]
		[Format(AngleUnit.Degree)]
		[Graph(GraphData.Continuous)]
		public Angle Latitude{ get; set; }

        /// <summary>
        /// Height about mean sea level, in true feet. 
        /// </summary>
		[CsvField(4)]
        [Format(LengthUnit.Foot)]
		[Graph(GraphData.Continuous)]
		public Length HeightMeanSeaLevel{ get; set; }

		[CsvField(5)]
		public int HeightRadioAltimeter{ get; set; }
		
		[CsvField(6)]
		[Format(RatioUnit.DecimalFraction)]
		[Graph(GraphData.Continuous)]
        public Ratio AileronDeflection{ get; set; }

		[CsvField(7)]
		[Format(RatioUnit.DecimalFraction)]
		[Graph(GraphData.Continuous)]
		public Ratio ElevatorDeflection { get; set; }

		[CsvField(8)]
		[Format(RatioUnit.DecimalFraction)]
		[Graph(GraphData.Continuous)]
		public Ratio RudderDeflection { get; set; }

		[CsvField(9)]
        [Format(AngleUnit.Degree)]
		[Graph(GraphData.Continuous)]
        public Angle Pitch{ get; set; }

        /// <summary>
        /// Roll. In degrees (-180 to +180)
        /// </summary>
		[CsvField(10)]
        [Format(AngleUnit.Degree)]
		[Graph(GraphData.Continuous)]
        public Angle Roll { get; set; }

        /// <summary>
        /// Heading. In degrees (0 to 360)
        /// </summary>
		[CsvField(11)]
        [Format(AngleUnit.Degree)]
		[Graph(GraphData.Continuous)]
        public Angle Heading { get; set; }

		/// <summary>
		/// Knots Indicated Air Speed
		/// </summary>
		[CsvField(12)]
		[Format(SpeedUnit.Knot)]
		[Graph(GraphData.Continuous)]
		public Speed IndicatedSpeed{ get; set; }

        /// <summary>
        /// Indicated Vertical speed. Feet per minute
        /// </summary>
        /// <returns></returns>
		[CsvField(13)]
		[Format(SpeedUnit.FootPerSecond, "60 * x")]
		[Graph(GraphData.Continuous)]
		public Speed VerticalSpeed { get; set; }

		[CsvField(14)]
		[Graph(GraphData.Continuous)]
		public int IndicatedSlip{ get; set; }

		// Turn Deg

		[CsvField(15)]
		[Graph(GraphData.Continuous)]
		public float Mach{ get; set; }

		[CsvField(16)]
        [Format(AngleUnit.Degree)]
		[Graph(GraphData.Continuous)]
        public Angle AngleOfAttack { get; set; }

		[CsvField(17)]
		[Graph(GraphData.Discrete)]
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
		[CsvField(18)]
		[Graph(GraphData.Discrete)]
        public int SpeedBrakeRatio { get;set; }

            /*
		gear handl*/

		[CsvField(19)]
		[Graph(GraphData.Discrete)]
		public bool IsNoseGearDown{ get; set; }

		[CsvField(20)]
		[Graph(GraphData.Discrete)]
		public bool IsLeftGearDown{ get; set; }

		[CsvField(21)]
		[Graph(GraphData.Discrete)]
		public bool IsRightGearDown{ get; set; }

        /*elev trim*/
        /// <summary>
        /// 5-Digit Integer.
        /// </summary>
        /// <returns></returns>
		[CsvField(22)]
		[Format(FrequencyUnit.Hertz)]
		[Graph(GraphData.Discrete)]
        public Frequency Nav1_Frequency { get; set; }
		
        /// <summary>
        /// 5-Digit Integer
        /// </summary>
        /// <returns></returns>
		[CsvField(23)]
		[Format(FrequencyUnit.Hertz)]
		[Graph(GraphData.Discrete)]
		public Frequency Nav2_Frequency { get; set; }

        /// <summary>
        /// None = 0
        /// NDB = 2
        /// VOR = 3
        /// LOC = 5
        /// ILS = 10
        /// </summary>
        /// <returns></returns>
		[CsvField(24)]
		[Graph(GraphData.Discrete)]
        public NavType Nav1_Type { get; set; }

		[CsvField(25)]
		[Graph(GraphData.Discrete)]
        public NavType Nav2_Type { get; set; }

        /// <summary>
        /// Degrees : 0 to 360
        /// </summary>
        /// <returns></returns>
		[CsvField(26)]
        [Format(AngleUnit.Degree)]
		[Graph(GraphData.Continuous)]
        public Angle OBS1 { get; set; }

        /// <summary>
        /// In Degrees. 0 to 360
        /// </summary>
        /// <returns></returns>
		[CsvField(27)]
        [Format(AngleUnit.Degree)]
		[Graph(GraphData.Continuous)]
        public Angle OBS2 { get; set; }

        /// <summary>
        /// 0.0: no DME found
        /// > 0.0: receiving data
        /// </summary>
        /// <returns></returns>
		[CsvField(28)]
        public float DME1 { get; set; }

		[CsvField(29)]
        public float DME2 { get; set; }

        /// <summary>
        /// -2.5 to 2.5. Localizer Deflection
        /// </summary>
        /// <returns></returns>
		[CsvField(30)]
		[Format(RatioUnit.DecimalFraction)]
		[Graph(GraphData.Continuous)]
        public Ratio Nav1_HorizontalDeflection { get; set; }

        /// <summary>
        /// -2.5 to 2.5. Localizer Deflection
        /// </summary>
        /// <returns></returns>
		[CsvField(31)]
		[Format(RatioUnit.DecimalFraction)]
		[Graph(GraphData.Continuous)]
        public Ratio Nav2_HorizontalDeflection { get; set; }

        /// <summary>
        /// 0 : Nav
        /// 1 : To
        /// 2 : From
        /// </summary>
        /// <returns></returns>
		[CsvField(32)]
		[Graph(GraphData.Discrete)]
        public NavMode Nav1_Mode { get; set; }

		[CsvField(33)]
		[Graph(GraphData.Discrete)]
        public NavMode Nav2_Mode { get; set; }

        /// <summary>
        /// -2.5 to 2.5. Glideslope Deflection
        /// </summary>
        /// <returns></returns>
		[CsvField(34)]
		[Graph(GraphData.Continuous)]
		public Ratio Nav1_VerticalDeflection { get; set; }

		[CsvField(35)]
		[Graph(GraphData.Continuous)]
        public Ratio Nav2_VerticalDeflection { get; set; }

		[CsvField(36)]
        public bool OM { get; set; }

		[CsvField(37)]
        public bool MM { get; set; }

		[CsvField(38)]
        public bool IM { get; set; }

		[CsvField(39)]
		[Graph(GraphData.Discrete)]
		public bool FlightDirectorEnabled{ get; set; }

		[CsvField(40)]
        [Format(AngleUnit.Degree)]
		[Graph(GraphData.Continuous)]
        public Angle FlightDirectorPitch{ get; set; }

		[CsvField(41)]
        [Format(AngleUnit.Degree)]
		[Graph(GraphData.Continuous)]
		public Angle FlightDirectorRoll { get; set; }

		[CsvField(42)]
		[Graph(GraphData.Discrete)]
		public bool KTMAC { get; set; }

		[CsvField(43)]
		[Graph(GraphData.Discrete)]
		public int ThrottleMode{ get; set; }

		[CsvField(44)]
		[Graph(GraphData.Discrete)]
		public int HeadingMode{ get; set; }

		[CsvField(45)]
		[Graph(GraphData.Discrete)]
		public int AltimeterMode{ get; set; }

		[CsvField(46)]
		[Graph(GraphData.Discrete)]
		public int HNavMode{ get; set; }

		[CsvField(47)]
		[Graph(GraphData.Discrete)]
		public int GlslpMode { get; set; }

		[CsvField(48)]
		[Graph(GraphData.Discrete)]
		public int BackMode{ get; set; }

		[CsvField(49)]
		[Graph(GraphData.Discrete)]
		public int SpeedSelect{ get; set; }

		[CsvField(50)]
		[Graph(GraphData.Discrete)]
		public int HeadingSelect{ get; set; }

		[CsvField(51)]
		[Graph(GraphData.Discrete)]
		public int VerticalSpeedSelect{ get; set; }

		[CsvField(52)]
		[Graph(GraphData.Discrete)]
		public int AltimeterSelect{ get; set; }

		[CsvField(53)]
        [Format(PressureUnit.Psi)]
		public Pressure Barometer{ get; set; }

        /// <summary>
        /// Decision height, dialed into the radio alt., feet AGL
        /// </summary
		[CsvField(54)]
        [Format(LengthUnit.Foot)]
		[Graph(GraphData.Discrete)]
        public Length DecisionHeight { get; set; }

		[CsvField(55)]
		[Graph(GraphData.Discrete)]
		public bool MasterCaution{ get; set; }

		[CsvField(56)]
		[Graph(GraphData.Discrete)]
		public bool MasterWarning{ get; set; }

		[CsvField(57)]
		[Graph(GraphData.Discrete)] // ???
		public bool GPWS{ get; set; }

		// Mmode 0-4
		[CsvField(58)]
		[Graph(GraphData.Discrete)]
		public byte MapMode{ get; set; }

		// Mrang 0-6
		[CsvField(59)]
		[Graph(GraphData.Discrete)]
		public byte MapRange{ get; set; }

		[CsvField(60)]
        [Format(RatioUnit.Percent)]
		public Ratio ThrottleRatio{ get; set; }

		[CsvField(61)]
        public int PropellerRPMCommand { get; set; }
        /*
		 * prop cntrl
		 * prop cntrl
		 * prop cntrl
		 * prop cntrl
		 */

		[CsvField(62)]
        [Format(RotationalSpeedUnit.RevolutionPerMinute)]
        public RotationalSpeed PropellerRPM { get; set; }

		[CsvField(63)]
        [Format(AngleUnit.Degree)]
		public Angle PropellerDegree{ get; set; }

		/// <summary>
		/// N1 is the low-pressure spool. Percentage
		/// </summary>
		/// <remarks>
		/// source: http://www.airliners.net/aviation-forums/tech_ops/read.main/159683/
		/// </remarks>
		[CsvField(64)]
		[Format(RatioUnit.Percent)]
        public Ratio N1{ get; set; }

		/// <summary>
		/// N2 is the high-pressure spool. Percentage
		/// </summary>
		/// <remarks>
		/// source: http://www.airliners.net/aviation-forums/tech_ops/read.main/159683/
		/// </remarks>
		[CsvField(65)]
		[Format(RatioUnit.Percent)]
        public Ratio N2 { get; set; }

		// MPR inch
		[CsvField(66)]
		public Ratio MPressureRatio{ get; set; }

		// EPR ind
		[CsvField(67)]
		public Ratio EnginePressureRatio{ get; set; }

		/// <summary>
		/// Torque. Per Engine. ft/lbs
		/// </summary>
		[CsvField(68)]
		[Format(TorqueUnit.Newtonmeter, "0.73756 * x")]
        public Torque Torque{ get; set; }

		/// <summary>
		/// Fuel Flow. Per Engine. Pounds per Hour
		/// </summary>
		//[Format(MassUnit.Pound)]
		//[Format(DurationUnit.Hour)]
		[CsvField(69)]
        public Ratio FuelFlow{ get; set; }
					
        /// <summary>
        /// Turbine Inlet Temp. Per Engine. In Celsius
        /// </summary>
		[CsvField(70)]
        [Format(TemperatureUnit.DegreeCelsius)]
        public Temperature TurboInletTemp{ get; set; }

        /// <summary>
        /// Exhaust Gas Temperature. Per Engine. In Celsius
        /// </summary>
		[CsvField(71)]
        [Format(TemperatureUnit.DegreeCelsius)]
        public Temperature ExhaustGasTemp { get; set; }

		/// <summary>
		/// Cylinder Head Temperature. Per Engine. In Celsius
		/// </summary>
		[CsvField(72)]
		[Format(TemperatureUnit.DegreeCelsius)]
        public Temperature CylinderHeadTemp { get; set; }
	}
}