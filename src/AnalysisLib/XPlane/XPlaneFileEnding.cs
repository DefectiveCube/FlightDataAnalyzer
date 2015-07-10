using System;

namespace FDA
{
	/// <summary>
	/// XPlane FDR requires the line-ending types to be specified on the 2nd line (either 'A' or 'I')
	/// </summary>
	public enum XPlaneFileEnding
	{
		IBM		= 73,
		APPLE 	= 65
	}
}