using System;

namespace FDA.Attributes
{
    /// <summary>
    /// Indicates a relationship between property and one-or-many CSV fields
    /// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class CsvFieldAttribute : Attribute
    {
        /// <summary>
        /// The index of the CSV field
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// The type for the CSV field
        /// </summary>
        public Type Type { get; private set; }

        public CsvFieldAttribute(int index)
        {
            Index = index;
        }

        // This constructor is REQUIRED when two fields are mapped to the same property
        public CsvFieldAttribute(int index, Type type) : this(index)
        {
            Type = type;
        }
    }
}
