using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneWPF.Models
{
    public enum QueryJoinMode
    {
        Left, Right, Inner, Outer
    }

    public class Query
    {
        public void Test()
        {
            this.Join(null, q => q.Name == string.Empty, QueryJoinMode.Inner);
        }

        public string Name { get; set; }

        public void Join(Query query, Predicate<Query> predicate, QueryJoinMode join)
        {

        }
    }
}
