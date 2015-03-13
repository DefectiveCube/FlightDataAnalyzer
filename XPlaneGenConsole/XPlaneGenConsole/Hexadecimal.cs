using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    internal class Hexadecimal<T> where T : struct
    {
        private static MethodInfo ParseMethod;
        private static Type[] acceptableTypes = new Type[] { typeof(byte), typeof(short), typeof(int), typeof(long), typeof(sbyte), typeof(ushort), typeof(uint), typeof(ulong) };

        public static T Parse(string input)
        {
            if (!acceptableTypes.Contains(typeof(T)))
            {
                //throw new InvalidTypeException("Invalid Type. Only integral types are allowed");
                throw new Exception();
            }

            if (ParseMethod == null)
            {
                ParseMethod = typeof(T).GetMethod("TryParse", new Type[] { typeof(string), typeof(NumberStyles), typeof(IFormatProvider), typeof(T).MakeByRefType() });
            }

            if (input.Trim().ToUpperInvariant().StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                input = input.Substring(2);
            }

            T value;

            object[] args = new object[] { input, NumberStyles.HexNumber, CultureInfo.CurrentCulture, null };
            bool result = (bool)ParseMethod.Invoke(null, args);

            if (result)
            {
                value = (T)args[3];
                return value;
            }
            else
            {
                //Debug.WriteLine("[Hexadecimal.Parse] Warning: treating '" + input + "' as '" + default(T) + "'");
                return default(T);
            }
        }
    }
}
