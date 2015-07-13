using System;

namespace FDA
{
    // WARNING: incomplete

    /// <summary>
    /// Stores a float (4-byte) as a short (2-byte) value
    /// </summary>
    public struct FixedPoint
    {
        public bool IsSigned;
        public float value;

        public static FixedPoint operator +(FixedPoint a, FixedPoint b)
        {
            return a;
        }

        public static FixedPoint operator -(FixedPoint a, FixedPoint b)
        {
            return a;
        }

        public static implicit operator float(FixedPoint a)
        {
            return a.value;
        }

        public static implicit operator double(FixedPoint a)
        {
            return a.value;
        }
    }
}
