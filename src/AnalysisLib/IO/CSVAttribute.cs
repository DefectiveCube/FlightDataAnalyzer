using System;

namespace FDA
{
    /// <summary>
    /// Use to 
    /// </summary>
    
	[AttributeUsage(AttributeTargets.Property)]
	public class CSVAttribute : Attribute
	{
		public readonly int index;
		public readonly string name;

		public CSVAttribute (int index)
		{
			this.index = index;
		}

		public CSVAttribute(string name)
		{
			this.name = name;
		}
	}
}