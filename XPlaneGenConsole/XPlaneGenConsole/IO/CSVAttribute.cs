using System;

namespace XPlaneGenConsole
{
	[AttributeUsage(AttributeTargets.Property)]
	public class CSVAttribute : Attribute
	{
		public readonly int Index;
		public readonly string Name;

		public CSVAttribute (int index)
		{
			Index = index;
		}

		public CSVAttribute(string name)
		{
			Name = name;
		}
	}
}