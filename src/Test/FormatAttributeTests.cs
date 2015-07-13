using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FDA.Attributes;
using UnitsNet;
using UnitsNet.Units;

namespace Test
{
    [TestClass]
    public class FormatAttributeTests
    {
        [TestMethod]
        [TestCategory("Attributes")]
        public void Acceleration()
        {
            TestValuesOf<AccelerationUnit>();
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void Angle()
        {
            TestValuesOf<AngleUnit>();
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void Duration()
        {
            TestValuesOf<DurationUnit>();
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void ElectricCurrent()
        {
            TestValuesOf<ElectricCurrentUnit>();
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void ElectricPotential()
        {
            TestValuesOf<ElectricPotentialUnit>();
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void ElectricResistance()
        {
            TestValuesOf<ElectricResistanceUnit>();
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void Frequency()
        {
            TestValuesOf<FrequencyUnit>();
        }


        [TestMethod]
        [TestCategory("Attributes")]
        public void Length()
        {
            TestValuesOf<LengthUnit>();
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void Mass()
        {
            TestValuesOf<MassUnit>();
        }


        [TestMethod]
        [TestCategory("Attributes")]
        public void Pressure()
        {
            TestValuesOf<PressureUnit>();
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void Ratio()
        {
            TestValuesOf<RatioUnit>();
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void RotationalSpeed()
        {
            TestValuesOf<RotationalSpeedUnit>();
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void Speed()
        {
            TestValuesOf<SpeedUnit>();
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void Torque()
        {
            TestValuesOf<TorqueUnit>();
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void Temperature()
        {
            TestValuesOf<TemperatureUnit>();
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void Volume()
        {
            TestValuesOf<VolumeUnit>();
        }

        private void TestValuesOf<T>()
        {
            FormatAttribute attr;

            var attrType = typeof(FormatAttribute);
            var t = typeof(T);

            // Obtain enum values
            var arr = (T[])Enum.GetValues(t);
            var items = Array.ConvertAll(arr, item => new { Name = item.ToString(), Field = item, Value = Convert.ToInt32(item) });

            // Find the ctor that accepts one parameters of type T, string, string
            var ctor = attrType.GetConstructor(new[] { t, typeof(string), typeof(string) });

            foreach (var item in items)
            {
                if(!item.Name.ToLowerInvariant().Equals("undefined"))
                {
                    attr = (FormatAttribute)ctor.Invoke(new Object[] { item.Field, "", "" });

                    DefinedUnit(attr, t, item.Value);
                    //CustomizedUnit(attr, t, "x * 3.0");
                }
            }
        }

        private void DefinedUnit(FormatAttribute attr, Type type, int unitValue)
        {
            attr.UnitEnumType.IsNotNull();
            attr.UnitEnumType.AreEqual(type);

            attr.Unit.AreEqual(unitValue);
            attr.UnitName.IsNotNullOrEmpty();
            attr.Conversion.IsNullOrEmpty();
            attr.IsDefinedUnit.IsTrue();
            attr.IsCustomized.IsFalse();
        }

        private void CustomizedUnit(FormatAttribute attr, Type type, string conversion)
        {

        }

        private void PrimitiveUnit(FormatAttribute attr, NumberStyles style)
        {
            attr.Style.AreEqual(style);
            attr.Conversion.IsNull();
            attr.UnitName.IsNull();
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void HexNumber()
        {
            PrimitiveUnit(new FormatAttribute(NumberStyles.HexNumber), NumberStyles.HexNumber);            
        }
    }
}
