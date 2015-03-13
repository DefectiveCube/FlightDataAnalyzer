using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneTest
{
    public class Field
    {

    }

    public class TestField : Field
    {
        public string System { get; set; }
        public string Field { get; set; }
        public Type Type { get; set; }

        public int CompareTo(TestField other)
        {
            throw new NotImplementedException();
        }
    }

    public class ConstantField : Field
    {

    }
}
