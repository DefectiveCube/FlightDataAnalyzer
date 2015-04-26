using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole.Data
{
    public struct Range
    {
        public readonly float Minimum;
        public readonly float Maximum;

        public Range(float min = default(float),float max = default(float))
        {
            Minimum = min;
            Maximum = max;
        }
    }

    public struct Range<T>
        where T : struct
    {
        public readonly T Minimum;
        public readonly T Maximum;

        public Range(T min = default(T), T max = default(T))
        {
            Minimum = min;
            Maximum = max;
        }

        public bool Contains(T value, bool minInclusive = true, bool maxInclusive = true)
        {
            return false;
        }
    }
}
