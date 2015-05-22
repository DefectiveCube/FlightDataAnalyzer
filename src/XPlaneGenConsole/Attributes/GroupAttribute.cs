using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public class GroupAttribute : Attribute
    {
        public readonly string Group;

        public GroupAttribute(string name)
        {
            Group = name;
        }
    }
}
