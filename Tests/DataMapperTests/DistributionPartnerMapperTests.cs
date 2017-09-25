using System;
using System.Collections.Generic;
using System.IO;
using Domain.DataMapper;
using Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.DataMapperTests
{
    [TestClass]
    public class DistributionPartnerMapperTests
    {
        [TestMethod]
        //Attempt to load the data into the mapper with an invalid number of fields/columns
        //in this test, there is no endDate field
        public void TestLoadStreamWithInvalidNumberOfFields()
        {
            //arrange
            var mapper = new DistributionPartnerDataMapper();
            List<string> loadErrors = new List<string>();
            //act
            using (Stream stream = TestHelper.GenerateStreamFromString("Too|Many|Columns|Foo"))
            {
                try
                {
                    mapper.LoadData(stream, false,out loadErrors);
                    stream.Close();
                }
                finally
                {
                    stream.Close();
                }
            }
            //assert
            Assert.IsTrue(loadErrors.Count == 1);
            Assert.IsTrue(loadErrors[0].Contains("Too|Many|Columns|Foo"));
        }

        [TestMethod]
        public void TestCanLoadFromStream()
        {
            //arrange
            DistributionPartnerDataMapper mapper = new DistributionPartnerDataMapper();
            List<string> loadErrors = new List<string>();
            //act
            using (Stream stream = TestHelper.GenerateStreamFromString("ITunes|digital download"))
            {
                
                mapper.LoadData(stream, false, out loadErrors);
                stream.Close();
            }

            //assert
            Assert.IsTrue(mapper.GetData().Count == 1);
            Assert.IsInstanceOfType(mapper.GetData(), typeof(List<DistributionPartner>));
            Assert.AreEqual(mapper.GetData()[0].Partner, "ITunes");
            Assert.AreEqual(mapper.GetData()[0].Usages[0], "digital download");
            Assert.AreEqual(loadErrors.Count, 0);
        }

        [TestMethod]
        public void TestCanLoadFromStreamMultipleUsage()
        {
            //arrange
            DistributionPartnerDataMapper mapper = new DistributionPartnerDataMapper();

            //act
            using (Stream stream = TestHelper.GenerateStreamFromString("ITunes|digital download, vinyl")) // note whitespace after "vinyl"
            {
                List<string> loadErrors = new List<string>();
                mapper.LoadData(stream, false,out loadErrors);
                stream.Close();
            }

            //assert
            Assert.IsTrue(mapper.GetData().Count == 1);
            Assert.IsInstanceOfType(mapper.GetData(), typeof(List<DistributionPartner>));
            Assert.AreEqual(mapper.GetData()[0].Partner, "ITunes");
            Assert.AreEqual(mapper.GetData()[0].Usages.Length, 2);
            Assert.AreEqual(mapper.GetData()[0].Usages[1], "vinyl");
            Assert.AreNotEqual(mapper.GetData()[0].Usages[1], " vinyl");
        }
    }
}
/*
Partner|Usage

ITunes|digital download

YouTube|streaming
*/