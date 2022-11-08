using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Actions;
using StockBox.Actions.Adapters;
using StockBox.Controllers;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;
using StockBox.Data.Scraper;
using StockBox.Interpreter.Scanner;
using StockBox.Models;
using StockBox.RiskProfiles;
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
                new Rule("close < open"),
                new Rule("CLOSE > 60")
            };

            var setup = new Setup(rules, new UserDefinedState("start"), new RiskProfile());
            setup.AddAction(new Move(new BacktestMoveActionAdapter(), "end"));
            var profiles = new SymbolProfileList()
            {
                new SymbolProfile(new Symbol("MSFT"), new UserDefinedState("start")),
                new SymbolProfile(new Symbol("TSLA"), new UserDefinedState("start")),
                new SymbolProfile(new Symbol("AMD"), new UserDefinedState("start")),
            };

            var activeService = new ActiveService(new Scanner(), new Parser());

            var stateLists = new StateList()
            {
                new UserDefinedState("start"),
                new UserDefinedState("end"),
            };

            var stateMachine = new StateMachine(stateLists, new UserDefinedState("start"));
            stateMachine.AddTransition(new Transition(new UserDefinedState("start"), new UserDefinedState("end")));

            var controller = new DomainController(activeService, stateMachine, new FrameListFactory(new SbScraper(), new DeedleAdapter()));

            controller.ScanSetup(setup, profiles);

            // This test is incomplete. Since it relies on actual scraped data,
            // and we currently don't have a way to query the scraped data
            // independant of the controller, it will fail sometimes. What we
            // need to do is create an accessor class that will allow us to know
            // the results and then situationally change the assertion

            // The end goal with controller.GetResults(), is to actually get
            // all of the ValidationObjects from the results and from those
            // determine the outcome of our evaluations
            Assert.IsTrue(controller.GetResults().Success);
        }
    }
}
