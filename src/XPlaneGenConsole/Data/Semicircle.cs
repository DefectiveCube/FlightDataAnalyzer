using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole.Data
{
    public struct Semicircle
    {
        public readonly Range Range;

        internal Semicircle(int a = 0)
        {
            this.Range = new Range(-180.0f, 180.0f);
        }
    }

    public struct Circle
    {

    }
}
