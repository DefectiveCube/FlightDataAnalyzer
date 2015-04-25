using System;
using System.Linq;

namespace XPlaneGenConsole
{
	public interface IDatapointParse
	{
		void Parse(string[] values);
	}
}