using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDA
{
    public class GraphAttribute : Attribute
    {
        public readonly GraphData DataType;

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
