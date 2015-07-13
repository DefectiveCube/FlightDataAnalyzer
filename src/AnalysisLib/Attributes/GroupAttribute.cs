using System;

namespace FDA.Attributes
{
    /// <summary>
    /// Indicates a group that the target property belongs to
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class GroupAttribute : Attribute
    {
        public string Group { get; private set; }

        public GroupAttribute(string name)
        {
            Group = name;
        }
    }
}
