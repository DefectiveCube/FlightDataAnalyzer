using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using UnitsNet.Units;

namespace FDA
{
    public class StorageAttribute : Attribute
    {
        public readonly int Index;
        public readonly Type Type;

        public StorageAttribute(int index)
        {
            Index = index;
        }

        public StorageAttribute(int index, Type type) : this(index)
        {
            Type = type;
        }

        public static bool IsValidType(Type type)
        {
            switch (type.Name)
            {
                case "Byte":
                case "Int16":
                case "Int32":
                case "Int64":
                case "Single":
                case "Double":
                case "DateTime":
                case "TimeSpan":
                    return true;
                default:
                    return false;
            }
        }
    }
}
