using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDA.Extensions
{
    internal static class ExceptionHandlingExtensions
    {
        public static void ThrowIfNull<T>(this object source, string message = "")
            where T: Exception, new()
        {
            if (source == null)
            {
                throw Activator.CreateInstance<T>();
            }
        }
    }
}