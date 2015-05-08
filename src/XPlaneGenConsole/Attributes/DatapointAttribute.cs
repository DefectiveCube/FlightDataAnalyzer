using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    [AttributeUsage(AttributeTargets.Assembly,AllowMultiple =true)]
    public class DatapointAttribute : Attribute
    {
        public readonly Type Type;

        public DatapointAttribute(Type DatapointType)
        {
            Type = DatapointType;
        }            
    }
}
