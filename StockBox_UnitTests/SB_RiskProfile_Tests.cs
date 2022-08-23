using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.RiskProfiles;
using StockBox_UnitTests.Accessors;
using System;


namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_RiskProfile_Tests
    {
        [TestMethod]
        public void SB_RiskProfile_01_RiskProfileCanBeCreated()
        {
            Assert.IsNotNull(new RiskProfile());
        }

        [TestMethod]
        public void SB_RiskProfile_02_TestProfileCalculations()
        {
            var rpa = RiskProfile_Accessor.CreateSample_RiskDollars();
            var calculatedShareCount = rpa.CalculateTotalShares(20).Shares;

            Assert.AreEqual(10, calculatedShareCount);
        }

        [TestMethod]
        public void SB_RiskProfile_03_TestProfileCalculations()
        {
            var rpa = RiskProfile_Accessor.CreateSample_RiskPercent(12);
            var calculatedShareCount = rpa.CalculateTotalShares(20).Shares;

            Assert.AreEqual(60, calculatedShareCount);
        }

        [TestMethod]
        public void SB_RiskProfile_04_TestProfileCalculations()
        {
            // with no risk set, the calculation throws an exception which
            // results in an automatic 0 return value for Shares and a
            // failure result in the VRL
            var rpa = RiskProfile_Accessor.CreateSample_NoTotalRiskSet();
            var calc = rpa.CalculateTotalShares(20);
            Assert.AreEqual(0, calc.Shares);
            Assert.IsTrue(calc.Results.HasFailures);
        }
    }
}
