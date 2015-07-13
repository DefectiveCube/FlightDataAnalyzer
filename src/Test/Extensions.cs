using System;
//using System.Diagnostics;
//using System.Globalization;
//using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    internal static class Extensions
    {
        /// <summary>
        /// Verifies the condition is true
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void IsTrue(this bool source, string message = "")
        {
            Assert.IsTrue(source, message);
        }

        /// <summary>
        /// Verifities the condition is false
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void IsFalse(this bool source, string message = "")
        {
            Assert.IsFalse(source, message);
        }

        public static void IsNull(this Object source)
        {
            Assert.IsNull(source);
        }

        public static void IsNotNull(this Object source)
        {
            Assert.IsNotNull(source);
        }

        public static void AreEqual<T>(this T left, T right)
        {
            Assert.AreEqual<T>(left, right);
        }

        public static void AreNotEqual<T>(this T left, T right)
        {
            Assert.AreNotEqual<T>(left, right);
        }


        public static void IsNullOrEmpty(this string source)
        {
            Assert.IsTrue(string.IsNullOrEmpty(source));
        }

        public static void IsNotNullOrEmpty(this string source)
        {
            Assert.IsFalse(string.IsNullOrEmpty(source));
        }
    }
}