using System;

namespace FDA
{
	public class XPlaneHeader
	{
		// 1 or 2
		public byte Version{ get; set; }

		// A or I
		public XPlaneFileEnding Ending{ get; set; }

		// ACFT
		public string Aircraft { get; set; }

		// TAIL
		public string Tail { get; set; }

		// TIME
		// DATE
		public DateTime Start{ get; set; }

		// PRES
		public double Pressure { get; set; }

		// TEMP
		public double Temperature { get; set; }

		// WIND
		public int WindDirection{ get; set; }
		public int WindSpeed{ get; set; }

		// CALI ???
	}
}