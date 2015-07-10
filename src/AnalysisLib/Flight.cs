using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDA
{
    public class Flight { }

    public class Flight<T> : Flight
        where T : BinaryDatapoint
    {
		public List<T> Data { get; protected set; }

    }

}