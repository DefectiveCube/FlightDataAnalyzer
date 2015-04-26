using System;
using System.Globalization;

namespace XPlaneGenConsole
{
    public interface ITemperature : IConvertible, IComparable<float>, IEquatable<float>
    { }
}