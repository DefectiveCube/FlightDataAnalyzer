using System;

namespace XPlaneGenConsole.Data
{
    public struct Kelvin : ITemperature
    {
        public const float MinValue = 0.0f;

        internal float value;

        internal Kelvin(float value)
        {
            if (value < MinValue || float.IsNaN(value) || float.IsInfinity(value))
            {
                throw new ArgumentOutOfRangeException();
            }

            this.value = value;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Single;
        }

        public int CompareTo(float value)
        {
            return this.value.CompareTo(value);
        }

        public bool Equals(float value)
        {
            return this.value == value;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(value);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(value);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(value);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(value);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(value);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(value);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(value);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(value);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(value);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(value);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(value);
        }

        public string ToString(IFormatProvider provider)
        {
            return value.ToString();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(value, conversionType);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(value);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(value);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(value);
        }

        //
        // Operators
        //
        public static bool operator ==(Kelvin left, Kelvin right)
        {
            return left.value == right.value;
        }

        public static bool operator >(Kelvin left, Kelvin right)
        {
            return left.value > right.value;
        }

        public static bool operator >=(Kelvin left, Kelvin right)
        {
            return left.value >= right.value;
        }

        public static bool operator !=(Kelvin left, Kelvin right)
        {
            return left.value != right.value;
        }

        public static bool operator <(Kelvin left, Kelvin right)
        {
            return left.value < right.value;
        }

        public static bool operator <=(Kelvin left, Kelvin right)
        {
            return left.value <= right.value;
        }

        //
        // implicit conversions
        //
        public static implicit operator Kelvin(int value)
        {
            if (value < MinValue)
            {
                throw new ArgumentOutOfRangeException();
            }

            var temp = new Kelvin();
            temp.value = Convert.ToSingle(value);

            return temp;
        }

        public static implicit operator Kelvin(float value)
        {
            if (value < MinValue)
            {
                throw new ArgumentOutOfRangeException();
            }

            return new Kelvin(value);
        }

        public static implicit operator Kelvin(double value)
        {
            if (value < MinValue)
            {
                throw new ArgumentOutOfRangeException();
            }

            return new Kelvin((float)value);
        }

        public static implicit operator float (Kelvin value)
        {
            return value.value;
        }
    }
}
