using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XPlaneGenConsole
{
    public abstract class XPlaneBaseDatapoint : BinaryDatapoint
    {
        protected XPlaneBaseDatapoint()
        {

        }

        public readonly string Label;
    }
}