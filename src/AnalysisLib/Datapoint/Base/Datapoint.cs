using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDA
{
    public abstract class Datapoint
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
    }

    public abstract class Datapoint<T> : Datapoint
        where T : Datapoint<T>
    {

    }
}