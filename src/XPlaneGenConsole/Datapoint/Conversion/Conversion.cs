using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public class Conversion<T, U>
        where T : TextDatapoint, new()
        where U : BinaryDatapoint, new()
    {
        private List<Delegate> delegates;

        public Conversion()
        {
            delegates = new List<Delegate>();
        }

        public void Add(Delegate del)
        {
            Func<string, int, int> f = Extensions.AsInt;
            Func<string, string, DateTime> g = Extensions.AsDateTime;



            delegates.Add(f);
            delegates.Add(g);
        }
    }
}
