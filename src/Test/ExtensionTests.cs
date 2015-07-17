using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitsNet;
using UnitsNet.Units;
using FDA.Extensions;

namespace Test
{
    [TestClass]
    public class ExtensionTests
    {
        [TestMethod]
        public void GetDefaultUnit()
        {
            var passed = true;
            var data = Units.GetUnitEnumTypes().Zip(Units.GetDefaultUnits(), (type, unit) => new { Type = type, Unit = unit });

            foreach (var item in data)
            {
                try
                {
                    Console.Write("Testing {0}", item.Type.Name);
                    DefaultUnitTester(item.Type, item.Unit);
                    Console.WriteLine(" Passed");
                }
                catch (AssertFailedException)
                {
                    Console.WriteLine(" Failed");
                    passed = false;
                }
                catch
                {
                    throw;
                }
            }


            if (!passed)
            {
                throw new AssertFailedException();
            }
        }

        private void DefaultUnitTester(Type type, object value)
        {
            Assert.AreEqual(type.GetDefaultUnit(), value.ToString());
        }
    }
}
