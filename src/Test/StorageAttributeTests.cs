using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FDA.Attributes;

namespace Test
{
    [TestClass]
    public class StorageAttributeTests
    {
        [TestMethod]
        [TestCategory("Attributes")]
        public void InferredType()
        {
            var attr = new StorageAttribute(1);

            Assert.AreEqual(attr.Index, 1);
            Assert.IsNull(attr.Type);
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void ExplicitType()
        {
            var attr = new StorageAttribute(1, typeof(byte));

            Assert.AreEqual(attr.Index, 1);
            Assert.IsNotNull(attr.Type);
            Assert.AreEqual(attr.Type, typeof(byte));
        }
    }
}
