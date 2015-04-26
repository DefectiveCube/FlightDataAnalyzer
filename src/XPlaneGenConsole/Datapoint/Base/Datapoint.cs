using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public abstract class Datapoint
    {
        public int Flight { get; protected set; }

        public int Timestamp { get; internal set; }

        public DateTime DateTime { get; internal set; }

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