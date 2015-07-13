using System;
using FDA;
using FDA.Attributes;

namespace Prototype
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Datapoint 
        : BinaryDatapoint, IComparable<Datapoint>
    {
        [Storage(1)]
        public int Flight { get; set; }

        [CsvField(0)]
        [Storage(2)]
        public int Timestamp { get; set; }

        [CsvField(1, typeof(DateTime))]
        [CsvField(2, typeof(TimeSpan))]
        [Storage(0, typeof(long))]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// True, if datapoint has usable data
        /// </summary>
        public bool IsValid { get; set; }

        public int CompareTo(Datapoint dp)
        {
            if (dp == null) return 1;

            var t = Timestamp.CompareTo(dp.Timestamp);
            var d = DateTime.CompareTo(dp.DateTime);

            if (t == 0) return 0; // datetime does not matter here

            if (d == 0)
            {
                return t;
            }

            // TODO: handle differences

            /*if (d != t)
            {
                throw new ArgumentOutOfRangeException("timestamp and datetime");
            }*/

            return d;
        }
    }
}