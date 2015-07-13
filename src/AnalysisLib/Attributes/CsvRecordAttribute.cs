using System;

namespace FDA.Attributes
{
    /// <summary>
    /// Indicates that a class represents a CSV record
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
	public sealed class CsvRecordAttribute : Attribute
	{
        /// <summary>
        /// The number of mapped CSV fields
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
		public CsvRecordAttribute (int count){
            Count = count;
		}
	}
}