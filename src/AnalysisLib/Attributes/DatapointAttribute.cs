using System;

namespace FDA.Attributes
{
    /// <summary>
    /// Specifies one or many types of model datapoints that are contained within the target assembly
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class DatapointAttribute : Attribute
    {
        public Type Type { get; private set; }

        public DatapointAttribute(Type DatapointType)
        {
            Type = DatapointType;
        }
    }
}