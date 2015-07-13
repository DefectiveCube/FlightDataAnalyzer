using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDA.Attributes
{
    /// <summary>
    /// Indicates an Assembly contains datapoint models
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ModelAttribute : Attribute
    {
    }
}
