﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public class StorageAttribute : Attribute
    {
        public StorageAttribute(int offset)
        {

        }

        public StorageAttribute(int offset, Type type) : this(offset)
        {

        }
    }
}
