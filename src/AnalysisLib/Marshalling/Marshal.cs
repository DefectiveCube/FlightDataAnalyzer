using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FDA.Marshalling
{
    public class Marshal
    {
        AppDomain Caller;
        string DomainName;

        public Marshal()
        {
            Caller = Thread.GetDomain();
            DomainName = Caller.FriendlyName;

            //Caller.CreateInstanceAndUnwrap("", "");
        }
    }
}
