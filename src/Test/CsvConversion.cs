using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FDA;

namespace Test
{
    [TestClass]
    public class CsvConversion
    {
        [TestMethod]
        public void RunsToCompletion()
        {
            //XPlaneGenConsole.CsvConverter<Prototype.EngineDatapoint>.Load(@"", "");


            // File must exist
        }

        [TestMethod]
        public void NullPath()
        {
            try
            {
                CsvConverter<Prototype.EngineDatapoint>.Load(null, null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void InvalidPath()
        {
            try
            {
                CsvConverter<Prototype.EngineDatapoint>.Load(@"....", @"J:\Directory");
                Assert.Fail();
            }
            catch (UnauthorizedAccessException)
            {

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void MissingPath()
        {

        }

        [TestMethod]
        public void UnauthorizedPath()
        {

        }

        [TestMethod]
        public void NullDirectory()
        {
            try
            {

            }
            catch
            {

            }
        }

        [TestMethod]
        public void InvalidDirectory()
        {
            try
            {
                CsvConverter<Prototype.EngineDatapoint>.Load(@"J:\P_ENGINE.csv", "...");
                Assert.Fail();
            }
            catch (UnauthorizedAccessException)
            {

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void MissingDirectory()
        {

        }

        [TestMethod]
        public void ModelMismatch()
        {
            CsvConverter<Prototype.FlightDatapoint>.Load(@"J:\P_ENGINE.csv", "");
        }
    }
}
