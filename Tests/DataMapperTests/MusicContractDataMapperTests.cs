using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Domain.DataMapper;
using Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.DataMapperTests
{
    /// <summary>
    /// These tests are designed to test the data load mechanism for 
    /// loading data into a collection of MusicContract items. The data is loaded from
    /// a pipe-separated list.
    /// </summary>
    [TestClass]
    public class MusicContractDataMapperTests
    {
        private TestContext testContextInstance;


        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
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
        public void TestCanLoadStream()
        {
            //arrange
            MusicContractDataMapper mapper = new MusicContractDataMapper();
           
            //act
            using (Stream stream = TestHelper.GenerateStreamFromString("Artist|Title|Usages|01 March 2017|28 February 2017"))
            {
                List<string> loadErrors = new List<string>();
                mapper.LoadData(stream, false,out loadErrors);
                stream.Close();
            }
            
            //assert
            Assert.IsTrue(mapper.GetData().Count == 1);
            Assert.IsInstanceOfType(mapper.GetData(), typeof(List<MusicContract>));
            Assert.AreEqual(mapper.GetData()[0].Artist, "Artist");
        }

        [TestMethod]
        public void TestCanLoadStreamNoEndDate()
        {
            //arrange
            MusicContractDataMapper mapper = new MusicContractDataMapper();

            //act
            using (Stream stream = TestHelper.GenerateStreamFromString("Artist|Title|Usages|01 March 2017|"))
            {
                List<string> loadErrors = new List<string>();
                mapper.LoadData(stream, false,out loadErrors);
                stream.Close();
            }

            //assert
            Assert.IsTrue(mapper.GetData().Count == 1);
            Assert.IsInstanceOfType(mapper.GetData(), typeof(List<MusicContract>));
            Assert.AreEqual(mapper.GetData()[0].Artist, "Artist");
            Assert.IsNull(mapper.GetData()[0].EndDate);
        }

        [TestMethod]
        public void TestCanLoadStreamMultipleRows()
        {
            //arrange
            var testString = "Monkey Claw|Iron Horse|digital download, streaming|1st June 2012|" + Environment.NewLine +
                             "Monkey Claw|Motor Mouth|digital download, streaming|1st Mar 2011|" + Environment.NewLine +
                             "Monkey Claw|Christmas Special|streaming|25st Dec 2012|31st Dec 2012";
            //despite having an incorrect date (25st Dec 2012). The parser should correctly determine this to be 25th December
            MusicContractDataMapper mapper = new MusicContractDataMapper();
            //act
            using (Stream stream = TestHelper.GenerateStreamFromString(testString))
            {
                List<string> loadErrors = new List<string>();
                mapper.LoadData(stream, false,out loadErrors);
                stream.Close();
            }

            //assert
            Assert.IsTrue(mapper.GetData().Count == 3);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestLoadStreamWithInvalidDate()
        {
            //arrange
            MusicContractDataMapper mapper = new MusicContractDataMapper();

            //act
            using (Stream stream = TestHelper.GenerateStreamFromString("Artist|Title|Usages|1st Mar 2017|99 Irrelevembuary 2017"))
            {
                List<string> loadErrors = new List<string>();
                mapper.LoadData(stream, false,out loadErrors);
                stream.Close();
            }

            //assert

        }



        [TestMethod]
        [ExpectedException(typeof (InvalidDataException))]
        //Attempt to load the data into the mapper with an invalid number of fields/columns
        //in this test, there is no endDate field
        public void TestLoadStreamWithInvalidNumberOfFields()
        {
            //arrange
            MusicContractDataMapper mapper = new MusicContractDataMapper();

            //act
            using (Stream stream = TestHelper.GenerateStreamFromString("Artist|Title|Usages|01 March 2017"))
            {
                try
                {
                    List<string> loadErrors = new List<string>();
                    mapper.LoadData(stream, false,out loadErrors);
                    stream.Close();
                }
                finally
                {
                    stream.Close();
                }
            }
        }

        [TestMethod]
        //in this test, there is no endDate field
        public void TestLoadStreamWithEmptyEndDate()
        {
            //arrange
            MusicContractDataMapper mapper = new MusicContractDataMapper();

            //act
            using (Stream stream = TestHelper.GenerateStreamFromString("Artist|Title|Usages|01 March 2017|"))
            {
                try
                {
                    List<string> loadErrors = new List<string>();
                    mapper.LoadData(stream, false,out loadErrors);
                    stream.Close();
                }
                finally
                {
                    stream.Close();
                }
            }
            //assert
            Assert.IsNull(mapper.GetData()[0].EndDate);
        }

        [TestMethod]
        public void TestMusicContractDataMapperDateParser()
        {
            //arrange
            PrivateObject pObj = new PrivateObject(typeof(MusicContractDataMapper)); //access the private method ParseDate in MusicContractDataMapper
            //test for st's e.g. 1st, 21st, 31st
            DateTime expectedDate1_1 = DateTime.Parse("01 March 2017");
            DateTime expectedDate1_2 = DateTime.Parse("31 March 2017");

            DateTime expectedDate2_1 = DateTime.Parse("02 March 2017");
            DateTime expectedDate2_2 = DateTime.Parse("22 March 2017");
            DateTime expectedDate3_1 = DateTime.Parse("03 March 2017");
            DateTime expectedDate3_2 = DateTime.Parse("23 March 2017");
            DateTime expectedDate4_1 = DateTime.Parse("04 March 2017");
            DateTime expectedDate4_2 = DateTime.Parse("14 March 2017");
            DateTime expectedDate4_3 = DateTime.Parse("24 March 2017");
            //act
            //"st" dates (1st, 21st)
            DateTime parsedDate1_1 = (DateTime) pObj.Invoke("ParseDate", "01 March 2017");
            DateTime parsedDate1_2 = (DateTime) pObj.Invoke("ParseDate", "1st March 2017");
            DateTime parsedDate1_3 = (DateTime) pObj.Invoke("ParseDate", "1st Mar 2017");
            DateTime parsedDate1_4 = (DateTime) pObj.Invoke("ParseDate", "1 March 2017");

            DateTime parsedDate1_5 = (DateTime) pObj.Invoke("ParseDate", "31st March 2017");
            DateTime parsedDate1_6 = (DateTime) pObj.Invoke("ParseDate", "31st Mar 2017");

            //"nd" dates (2nd, 22nd)
            DateTime parsedDate2_1 = (DateTime)pObj.Invoke("ParseDate", "2nd March 2017");
            DateTime parsedDate2_2 = (DateTime)pObj.Invoke("ParseDate", "22nd March 2017");

            //"rd" dates (3rd, 23rd)
            DateTime parsedDate3_1 = (DateTime)pObj.Invoke("ParseDate", "3rd March 2017");
            DateTime parsedDate3_2 = (DateTime)pObj.Invoke("ParseDate", "23rd March 2017");

            //"th" dates (4th, 14th, 24th)
            DateTime parsedDate4_1 = (DateTime)pObj.Invoke("ParseDate", "4th March 2017");
            DateTime parsedDate4_2 = (DateTime)pObj.Invoke("ParseDate", "14th March 2017");
            DateTime parsedDate4_3 = (DateTime)pObj.Invoke("ParseDate", "24th March 2017");

            //assert
            //1st, 31st
            Assert.AreEqual(parsedDate1_1, expectedDate1_1);
            Assert.AreEqual(parsedDate1_2, expectedDate1_1);
            Assert.AreEqual(parsedDate1_3, expectedDate1_1);
            Assert.AreEqual(parsedDate1_4, expectedDate1_1);
            Assert.AreEqual(parsedDate1_5, expectedDate1_2);
            Assert.AreEqual(parsedDate1_6, expectedDate1_2);
            //2nd, 22nd
            Assert.AreEqual(parsedDate2_1, expectedDate2_1);
            Assert.AreEqual(parsedDate2_2, expectedDate2_2);
            //3rd, 23rd
            Assert.AreEqual(parsedDate3_1, expectedDate3_1);
            Assert.AreEqual(parsedDate3_2, expectedDate3_2);
            //"th" dates (4th, 14th, 24th)
            Assert.AreEqual(parsedDate4_1, expectedDate4_1);
            Assert.AreEqual(parsedDate4_2, expectedDate4_2);
            Assert.AreEqual(parsedDate4_3, expectedDate4_3);


        }
    }
}
