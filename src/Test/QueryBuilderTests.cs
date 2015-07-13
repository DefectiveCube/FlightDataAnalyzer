using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FDA;

namespace Test
{
    [TestClass]
    public class QueryBuilderTests
    {
        [TestMethod]
        public void Add()
        {
            var eq = QueryBuilder.Query("s + 1.0").Compile();

            var answer = eq(1.0);

            if (answer != 2.0)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Subtract()
        {
            var eq = QueryBuilder.Query("x - 1.0").Compile();

            var op = eq(2.0);

            if (op != 1.0)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AdditiveAxiom()
        {
            var r = new Random();
            var left = r.NextDouble();
            var right = r.NextDouble();

            var a = QueryBuilder.Query("x").Compile()(left);
            var b = QueryBuilder.Query("y").Compile()(left);
            var c = QueryBuilder.Query("u").Compile()(right);
            var d = QueryBuilder.Query("v").Compile()(right);

            Assert.AreEqual<double>(a,b);
            Assert.AreEqual<double>(c,d);
            Assert.AreEqual<double>(a + c, b + d);
        }

        [TestMethod]
        public void MultiplicativeAxiom()
        {
            var r = new Random();
            var left = r.NextDouble();
            var right = r.NextDouble();

            var a = QueryBuilder.Query("x").Compile()(left);
            var b = QueryBuilder.Query("y").Compile()(left);
            var c = QueryBuilder.Query("u").Compile()(right);
            var d = QueryBuilder.Query("v").Compile()(right);

            Assert.AreEqual<double>(a, b);
            Assert.AreEqual<double>(c, d);
            Assert.AreEqual<double>(a * c, b * d);
        }

        [TestMethod]
        public void ReflexiveAxiom()
        {
            var r = new Random();
            var a = QueryBuilder.Query("x").Compile()(r.NextDouble());

            Assert.AreEqual<double>(a, a);
        }

        [TestMethod]
        public void SymmetricAxiom()
        {
            var r = new Random().NextDouble();

            var a = QueryBuilder.Query("x").Compile()(r);
            var b = QueryBuilder.Query("y").Compile()(r);

            Assert.AreEqual<double>(a, b);
        }

        [TestMethod]
        public void TransitiveAxiom()
        {
            var r = new Random().NextDouble();

            var a = QueryBuilder.Query("x").Compile()(r);
            var b = QueryBuilder.Query("y").Compile()(r);
            var c = QueryBuilder.Query("y").Compile()(r);

            Assert.AreEqual<double>(a, b);
            Assert.AreEqual<double>(b, c);
            Assert.AreEqual<double>(a, c);
        }       
    }
}
