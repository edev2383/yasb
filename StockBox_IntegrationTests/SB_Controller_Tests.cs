using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Actions;
using StockBox.Actions.Adapters;
using StockBox.Actions.Responses;
using StockBox.Controllers;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;
using StockBox.Data.SbFrames.Providers;
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
            var rules = new Pattern() {
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

            var controller = new DomainController(activeService, stateMachine, new FrameListFactory(new SbScraper(), new ForwardTestingDataProvider()));

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

        [TestMethod, Description("A complete test successfully backtests multiple setups")]
        public void SB_Controllers_02_BacktestReturnsExpectedResults_ComplicatedRules()
        {

            var riskProfile = new RiskProfile()
            {
                TotalBalance = 10000,
                TotalRiskPercent = .08,
                StopLossDollars = 75,
            };

            var watchState = new UserDefinedState("watch");
            var primedState = new UserDefinedState("primed");
            var activeState = new ActiveState();
            var sellState = new InactiveState();
            var endState = new UserDefinedState("recently sold");
            var stateList = new StateList()
            {
                watchState,
                primedState,
                activeState,
                new ActivePendingState(),
                new InactivePendingState(),
                sellState,
                endState,
            };

            var stateMachine = new StateMachine(stateList, watchState);
            stateMachine.AddTransition(new Transition(watchState, primedState));
            // the iniitial transition requires the pending states, however,
            // the BacktestActionAdapters perform an additional transition to
            // the ActiveState and InactiveState states since we don't need to
            // worry about Pending/Error states during backtesting
            stateMachine.AddTransition(new Transition(primedState, new ActivePendingState()));
            stateMachine.AddTransition(new Transition(activeState, new InactivePendingState()));
            stateMachine.AddTransition(new Transition(sellState, watchState));


            var watchToPrimeRuleList = new Pattern() {
                new Rule("SlowSto(14,3) < 20"),
                new Rule("RSI(14) > 20")
            };

            var watchToPrimeSetup = new Setup(watchToPrimeRuleList, watchState, riskProfile);
            watchToPrimeSetup.AddAction(new Move(new BacktestMoveActionAdapter(), "primed"));

            var primedToActiveRuleList = new Pattern()
            {
                new Rule("SlowSto(14,3) x 20"),
                new Rule("RSI(14) > 37.5")
            };

            var primeToActiveSetup = new Setup(primedToActiveRuleList, primedState, riskProfile);
            primeToActiveSetup.AddAction(new Buy(new BacktestBuyActionAdapter()));

            var activeToInactiveRuleList = new Pattern()
            {
                new Rule("80 x SlowSto(14,3)"),
            };

            var activeToInactiveSetup = new Setup(activeToInactiveRuleList, activeState, riskProfile);
            activeToInactiveSetup.AddAction(new Sell(new BacktestSellActionAdapter()));

            var inactiveToWatchRuleList = new Pattern()
            {
                new Rule("SMA(20) > SMA(50)"),
            };

            var inactiveToWatchSetup = new Setup(inactiveToWatchRuleList, sellState, riskProfile);
            inactiveToWatchSetup.AddAction(new Move(new BacktestMoveActionAdapter(), "watch"));

            var setupList = new SetupList() {
                watchToPrimeSetup,
                primeToActiveSetup,
                activeToInactiveSetup,
                inactiveToWatchSetup,
            };

            var symbols = new SymbolProfileList()
            {
                new SymbolProfile(new Symbol("AMD"), watchState),
            };

            var activeService = new ActiveService(new Scanner(), new Parser());
            // MockFrameListFactory loads data from local files, rather than
            // calling the Scraper
            //var framelistFactory = new MockFrameListFactory(null, new DeedleBacktestAdapter());
            //framelistFactory.DataTarget_Daily = EFile.eAmdDaily;
            //framelistFactory.DataTarget_Weekly = EFile.eAmdWeekly;
            //framelistFactory.DataTarget_Monthly = EFile.eAmdMonthly;
            var framelistFactory = new FrameListFactory(null, new BackwardTestingDataProvider());

            var domainController = new BacktestController(activeService, stateMachine, framelistFactory);
            domainController.ScanSetups(setupList, symbols);

            var results = domainController.GetResults().GetHasValidationObjectsOfType<ActionResponse>();

            var positionSummary = domainController.PositionSummary;
            Assert.IsNotNull(positionSummary);
        }

    }
}
