using System;
using System.Text;
using System.Collections.Generic;
using Domain.DataMapper;
using Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repository;

namespace Tests.RepositoryTests
{
    /// <summary>
    /// Summary description for GrmRepositoryTests
    /// </summary>
    [TestClass]
    public class GrmRepositoryTests
    {
        public GrmRepositoryTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;
        private Mock<MusicContractDataMapper> _mockedMusicContractDataMapper= new Mock<MusicContractDataMapper>(); 
        private Mock<DistributionPartnerDataMapper> _mockedDistributionPartnerDataMapper= new Mock<DistributionPartnerDataMapper>(); 

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

        [TestInitialize]
        public void Initialise()
        {
            List<DistributionPartner> lstDistributionPartners = new List<DistributionPartner>();
            List<MusicContract> lstMusicContracts = new List<MusicContract>();

            lstDistributionPartners.Add(new DistributionPartner()
            {
                Partner = "iTunes",
                Usages = new string[] {"digital download"}
            });
            lstDistributionPartners.Add(new DistributionPartner()
            {
                Partner = "YouTube",
                Usages = new string[] {"streaming"}
            });

            lstMusicContracts.Add(new MusicContract() { Artist = "Tinie Tempah", Title = "Frisky (Live from SoHo)", Usages = new string[] { "digital download","streaming" }, StartDate = new DateTime(2012,2,1), EndDate = null});
            lstMusicContracts.Add(new MusicContract() { Artist = "Tinie Tempah", Title = "Miami 2 Ibiza", Usages = new string[] {"digital download"}, StartDate = new DateTime(2012, 2, 1), EndDate = null });
            lstMusicContracts.Add(new MusicContract() { Artist = "Tinie Tempah", Title = "Till I'm Gone", Usages = new string[] {"digital download" }, StartDate = new DateTime(2012, 8, 1), EndDate = null });
            lstMusicContracts.Add(new MusicContract() { Artist = "Monkey Claw",  Title = "Black Mountain", Usages = new string[] { "digital download" }, StartDate = new DateTime(2012, 2, 1), EndDate = null });
            lstMusicContracts.Add(new MusicContract() { Artist = "Monkey Claw",  Title = "Iron Horse", Usages = new string[] { "digital download" }, StartDate = new DateTime(2012, 6, 1), EndDate = null });
            lstMusicContracts.Add(new MusicContract() { Artist = "Monkey Claw",  Title = "Motor Mouth", Usages = new string[] { "digital download", "streaming" }, StartDate = new DateTime(2012, 3, 1), EndDate = null });
            lstMusicContracts.Add(new MusicContract() { Artist = "Monkey Claw",  Title = "Christmas Special", Usages = new string[] {"streaming"}, StartDate = new DateTime(2012,12,25), EndDate = new DateTime( 2012, 12, 31)});

            _mockedDistributionPartnerDataMapper.Setup(dist => dist.GetData()).Returns(lstDistributionPartners);
            _mockedMusicContractDataMapper.Setup(dist => dist.GetData()).Returns(lstMusicContracts);
        }

        [TestMethod]
        public void TestCanInstansiateRepository()
        {
            GRMRepository repo = new GRMRepository(_mockedDistributionPartnerDataMapper.Object, _mockedMusicContractDataMapper.Object);
            Assert.IsNotNull(repo);
        }


        [TestMethod]
        public void TestRepoQuery()
        {
            GRMRepository repo = new GRMRepository(_mockedDistributionPartnerDataMapper.Object, _mockedMusicContractDataMapper.Object);
            var results = repo.Query("iTunes", new DateTime(2012, 2, 1));
            Assert.IsTrue(results.Count == 3);
        }

        [TestMethod]
        public void TestRepoQueryShouldReturnNoResults()
        {
            GRMRepository repo = new GRMRepository(_mockedDistributionPartnerDataMapper.Object, _mockedMusicContractDataMapper.Object);
            var results = repo.Query("youtube", new DateTime(2012, 1, 1));
            Assert.IsTrue(results.Count == 0);
        }

        [TestMethod]
        public void TestRepoQueryDateBeforeContractStart()
        {
            GRMRepository repo = new GRMRepository(_mockedDistributionPartnerDataMapper.Object, _mockedMusicContractDataMapper.Object);
            var results = repo.Query("youtube", new DateTime(2012, 12, 20));
            Assert.IsTrue(results.Count == 2);
        }

        [TestMethod]
        public void TestRepoQueryDateAfterContractEnd()
        {
            GRMRepository repo = new GRMRepository(_mockedDistributionPartnerDataMapper.Object, _mockedMusicContractDataMapper.Object);
            var results = repo.Query("youtube", new DateTime(2013, 1, 1));
            Assert.IsTrue(results.Count == 2);//return all with usage = streaming except monkey claw xmas special
        }

        [TestMethod]
        public void TestRepoQueryDateBetweenContractDates()
        {
            GRMRepository repo = new GRMRepository(_mockedDistributionPartnerDataMapper.Object, _mockedMusicContractDataMapper.Object);
            var results = repo.Query("youtube", new DateTime(2012, 12, 26));
            Assert.IsTrue(results.Count == 3);
        }
    }
}
