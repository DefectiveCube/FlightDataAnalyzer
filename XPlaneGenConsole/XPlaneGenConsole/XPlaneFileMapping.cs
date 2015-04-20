using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public class XPlaneFileMapping
    {

    }

    public class XPlaneFileMapping<T, U, V> : XPlaneFileMapping
        where T : Datapoint<T>
        where U : Datapoint<U>
        where V : Datapoint<V>
    {
        
        
    }
}
