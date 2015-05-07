using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CsvFieldAttribute : Attribute
    {
        public readonly int Index;
        public readonly Type Type;

		public CsvFieldAttribute(){


		}
        public CsvFieldAttribute(int index)
        {
            Index = index;
        }

        public CsvFieldAttribute(int index, Type type) : this(index)
        {
            Type = type;
        }
    }
}
