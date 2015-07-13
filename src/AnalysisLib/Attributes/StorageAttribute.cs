using System;
using UnitsNet;
using UnitsNet.Units;

namespace FDA.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class StorageAttribute : Attribute
    {
        /// <summary>
        /// The index in the order in which the target property is written
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// The type that the target property is serialized as
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// Defines cardinality for persisting a property. Type is defined elsewhere
        /// </summary>
        /// <param name="index">Cardinality value</param>
        public StorageAttribute(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            Index = index;
        }

        /// <summary>
        /// Defines cardinality for persisting a property
        /// </summary>
        /// <param name="index">Cardinality value</param>
        /// <param name="type">Type to use for serialization</param>
        public StorageAttribute(int index, Type type) : this(index)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (!IsValidType(type))
            {
                throw new Exception("type");
            }

            Type = type;
        }

        // NOTE: Is this complete?
        public static bool IsValidType(Type type)
        {
            Console.WriteLine(type.IsPrimitive);

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
