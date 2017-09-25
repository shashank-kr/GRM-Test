using System;
using System.Security.Cryptography.X509Certificates;
using Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.ModelTests
{
    [TestClass]
    public class DistributionPartnerTests
    {
        [TestMethod]
        public void TestCanGetSetProperties()
        {
            var distributionPartner = new DistributionPartner()
            {
                Partner = "Spotify",
                Usages = new string[] {"Streaming"}
            };

            Assert.IsNotNull(distributionPartner);
            Assert.AreEqual(distributionPartner.Partner, "Spotify");
            Assert.AreEqual(distributionPartner.Usages[0], "Streaming");
        }

        [TestMethod]
        public void TestToStringMethod()
        {
            var distributionPartner = new DistributionPartner()
            {
                Partner = "Spotify",
                Usages = new string[] { "Streaming", "Vinyl" }
            };
            Assert.AreEqual(distributionPartner.ToString(), "Spotify|Streaming,Vinyl");
        }
    }
}
