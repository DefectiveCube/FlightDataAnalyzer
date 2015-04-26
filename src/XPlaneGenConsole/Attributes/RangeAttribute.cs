using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RangeAttribute : Attribute
    {
        public RangeAttribute(float min = default(float), float max = default(float))
        {

        }
    }
}
