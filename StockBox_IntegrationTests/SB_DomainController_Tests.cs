using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Models;
using StockBox.Rules;
using StockBox.Setups;

namespace StockBox_IntegrationTests
{

    [TestClass]
    public class SB_DomainController_Tests
    {
        [TestMethod]
        public void SB_DomainController_01_Tests()
        {
            var rules = new RuleList() {
                new Rule("close > open"),
                new Rule("CLOSE > 60")
            };

            var setup = new Setup(rules);

            var profiles = new SymbolProfileList();
            var symbol = new Symbol("MSFT");

            Assert.IsTrue(false);
        }
    }
}
