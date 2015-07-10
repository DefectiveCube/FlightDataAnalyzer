using System;

namespace FDA
{
	public class CsvRecordAttribute : Attribute
	{
        public readonly int Count;

/*		public CsvRecordAttribute ()
		{
		}*/

		public CsvRecordAttribute (int count){
            Count = count;
		}
	}
}