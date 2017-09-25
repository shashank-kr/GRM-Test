using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GRMApplication;

namespace Tests.ApplicationTests
{
    [TestClass]
    public class AppTests
    {
        [TestMethod]
        public void TestWitValidDate()
        {
            PrivateType pObj = new PrivateType(typeof(GRMApplication.Program));
            DateTime expectedDate = (DateTime)pObj.InvokeStatic("ParseDate", "1st March 2017");
            DateTime actualDate = new DateTime(2017,3,1);
            Assert.AreEqual(expectedDate, actualDate);
        }

        [TestMethod]
        [ExpectedException(typeof (FormatException))]
        public void TestWithInvalidDate()
        {
            PrivateType pObj = new PrivateType(typeof(GRMApplication.Program));
            DateTime expectedDate = (DateTime)pObj.InvokeStatic("ParseDate", "43rd Irrelivembruary 2017");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestWithEmptyDate()
        {
            PrivateType pObj = new PrivateType(typeof(GRMApplication.Program));
            DateTime expectedDate = (DateTime)pObj.InvokeStatic("ParseDate", "");
        }
    }
}
