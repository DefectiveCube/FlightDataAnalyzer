using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public abstract class CsvDatapoint<T> : TextDatapoint
        where T : CsvDatapoint<T>, new()
    {
        public static CsvFactory<T> Factory = new CsvFactory<T>();

        protected static int Key;

        protected static Random r = new Random();

        public int Fields { get; protected set; }

        public abstract void Load(string value);

        public abstract void Load(string[] values);

        public virtual Action<BinaryDatapoint, string[]> GetParser()
        { throw new NotSupportedException(); }
    }

    public sealed class XmlDatapoint : TextDatapoint
    {

    }

    public sealed class JsonDatapoint : TextDatapoint
    {

    }
}
