using System;

namespace FDA.Attributes
{
    /// <summary>
    /// Specfies that a property should be graphed as continuous, or discrete data
    /// </summary>
    public sealed class GraphAttribute : Attribute
    {
        public GraphData DataType { get; private set; }

        public GraphAttribute(GraphData data)
        {
            DataType = data;
        }
    }

    public enum GraphData
    {
        Continuous,
        Discrete
    }
}
