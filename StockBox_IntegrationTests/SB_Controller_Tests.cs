using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Controllers;
using StockBox.Interpreter.Scanner;
using StockBox.Models;
using StockBox.Rules;
using StockBox.Services;
using StockBox.Setups;
using StockBox.States;

namespace StockBox_IntegrationTests
{

    [TestClass]
    public class SB_Controller_Tests
    {
        [TestMethod]
        public void SB_DomainController_01_Tests()
        {
            var rules = new RuleList() {
                new Rule("close > open"),
                new Rule("CLOSE > 60")
            };

            var setup = new Setup(rules, new UserDefinedState("start"), new StockBox.RiskProfiles.RiskProfile());

            var profiles = new SymbolProfileList()
            {
                new SymbolProfile(new Symbol("MSFT"), new UserDefinedState("start")),
            };

            var activeService = new ActiveService(new Scanner(), new Parser());

            var stateLists = new StateList()
            {
                new UserDefinedState("start"),
                new UserDefinedState("end"),
            };

            var stateMachine = new StateMachine(stateLists, new UserDefinedState("start"));
            stateMachine.AddTransition(new Transition(new UserDefinedState("start"), new UserDefinedState("end")));

            var controller = new DomainController(activeService, stateMachine);

            controller.ScanSetup(setup, profiles);

            Assert.IsTrue(controller.GetResults().Success);
        }
    }
}
