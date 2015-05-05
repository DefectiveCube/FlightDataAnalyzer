using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public class StorageAttribute : Attribute
    {
        /// <summary>
        /// Determines which primitive type to use for serialization
        /// </summary>
        /// <param name="type"></param>
        public StorageAttribute(Type type)
        {

        }

        public StorageAttribute(int offset)
        {

        }

        public StorageAttribute(Type type, int offset)
        {

        }
    }
}
