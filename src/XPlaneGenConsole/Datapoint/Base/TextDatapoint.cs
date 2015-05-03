using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace XPlaneGenConsole
{
	public abstract class TextDatapoint : Datapoint<TextDatapoint>
	{
        public virtual IEnumerable<string> Value { get { return new string[] { }; } }
	}
}