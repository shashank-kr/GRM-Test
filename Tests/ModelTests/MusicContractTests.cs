using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.ModelTests
{
    /// <summary>
    /// Summary description for MusicContractTests
    /// </summary>
    [TestClass]
    public class MusicContractTests
    {
        public MusicContractTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext _testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestCanGetSetProperties()
        {
            var now = DateTime.Now;
            var musicContract = new MusicContract()
            {
                Artist = "Foo Fighters",
                Title = "Echoes, Silence, Patience and Grace",
                StartDate = now,
                EndDate = now.AddYears(1),
                Usages = new string[] {"Download", "Streaming", "Vinyl"}
            };
            Assert.AreEqual(musicContract.Artist, "Foo Fighters");
            Assert.AreEqual(musicContract.Title, "Echoes, Silence, Patience and Grace");
            Assert.IsTrue(musicContract.StartDate.HasValue);
            Assert.IsTrue(musicContract.EndDate.HasValue);
            Assert.AreEqual(musicContract.Usages.Length, 3);
            Assert.AreEqual(musicContract.Usages[1], "Streaming");
        }

        [TestMethod]
        public void TestToStringMethod()
        {
            var now = new DateTime(2012, 2, 1);
            var nextYear = new DateTime(2013, 2, 1);
            var musicContract = new MusicContract()
            {
                Artist = "The Bangles",
                Title = "Walk like an Egyptian",
                StartDate = now,
                EndDate = nextYear,
                Usages = new string[] { "Download", "Streaming", "Vinyl" }
            };
            var musicContractNoEndDate = new MusicContract()
            {
                Artist = "The Eagles",
                Title = "Hotel California",
                StartDate = now,
                EndDate = null,
                Usages = new string[] { "Download", "Streaming", "Vinyl" }
            };
            
            Assert.AreEqual(musicContract.ToString(), "The Bangles|Walk like an Egyptian|Download,Streaming,Vinyl|1 Feb 2012|1 Feb 2013");
            Assert.AreEqual(musicContractNoEndDate.ToString(), "The Eagles|Hotel California|Download,Streaming,Vinyl|1 Feb 2012|");
        }
    }
}
