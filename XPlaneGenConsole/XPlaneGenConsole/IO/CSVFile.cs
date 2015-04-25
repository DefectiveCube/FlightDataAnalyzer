using System;

namespace XPlaneGenConsole
{
	[AttributeUsage(AttributeTargets.Class)]
	public class CSVFile : Attribute
	{
		public CSVFile (CSVDirection dir = CSVDirection.Out,bool allowEmptyFields = false, int fields = 0)
		{
				
		}
	}
}

