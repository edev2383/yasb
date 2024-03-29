﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Actions;
using StockBox.Actions.Responses;
using StockBox.Controllers;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;
using StockBox.Data.Scraper;
using StockBox.Interpreter;
using StockBox.Interpreter.Scanner;
using StockBox.Models;
using StockBox.RiskProfiles;
using StockBox.Rules;
using StockBox.Services;
using StockBox.Setups;
using StockBox.States;
using StockBox_TestArtifacts.Mocks;
using StockBox_TestArtifacts.Helpers;
using StockBox.Actions.Adapters;
using StockBox_TestArtifacts.Presets.StockBox.Rules;
using StockBox.Data.SbFrames.Providers;

namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_Controllers_Tests
    {

        [TestMethod, Description("Create and assert the most simple DomainController possible is not null")]
        public void SB_Controllers_01_DomainControllerCanBeCreated()
        {
            var activeService = new ActiveService(new Scanner(), new Parser());
            var stateMachine = new StateMachine(new StateList(), new ActiveState());
            var controller = new DomainController(activeService, stateMachine, new FrameListFactory(new SbScraper(), new ForwardTestingDataProvider()));
            Assert.IsNotNull(controller);
        }

        [TestMethod, Description("A complete setup successfully transitions a Symbol from `start` state to `end` state")]
        public void SB_Controllers_02_ProvidedSymbolEntersTheCorrectState()
        {

            var startState = new UserDefinedState("start");
            var endState = new UserDefinedState("end");
            var stateList = new StateList()
            {
                startState,
                endState,
            };
            var transition = new Transition(startState, endState);
            var stateMachine = new StateMachine(stateList, startState);
            stateMachine.AddTransition(transition);

            var ruleList = new Pattern() {
                new Rule("Close < Yesterday Close"),
            };

            var setup = new Setup(ruleList, startState, new RiskProfile());
            setup.AddAction(new Move(new MockMoveActionAdapter(), "end"));

            var symbols = new SymbolProfileList()
            {
                new SymbolProfile(new Symbol("AMD"), startState),
            };

            var activeService = new ActiveService(new Scanner(), new Parser());
            var framelistFactory = new MockFrameListFactory(null, new ForwardTestingDataProvider());
            framelistFactory.DataTarget_Daily = EFile.eAmdDaily;
            framelistFactory.DataTarget_Weekly = EFile.eAmdWeekly;
            framelistFactory.DataTarget_Monthly = EFile.eAmdMonthly;

            var domainController = new DomainController(activeService, stateMachine, framelistFactory);
            domainController.ScanSetup(setup, symbols);

            var results = domainController.GetResults().GetHasValidationObjectsOfType<ActionResponse>();
            var response = results.First().ValidationObject as ActionResponse;

            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual("Symbol AMD was moved to end", response.Message);
        }

        [TestMethod, Description("A complete setup successfully backtests multiple setups")]
        public void SB_Controllers_03_BacktestReturnsExpectedResults()
        {

            var riskProfile = new RiskProfile()
            {
                TotalBalance = 10000,
                TotalRiskPercent = .02,
                StopLossDollars = 200,
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
                new Rule("Close > Yesterday Close"),
            };

            var watchToPrimeSetup = new Setup(watchToPrimeRuleList, watchState, riskProfile);
            watchToPrimeSetup.AddAction(new Move(new BacktestMoveActionAdapter(), "primed"));

            var primedToActiveRuleList = new Pattern()
            {
                new Rule("Close > Yesterday Close"),
            };

            var primeToActiveSetup = new Setup(primedToActiveRuleList, primedState, riskProfile);
            primeToActiveSetup.AddAction(new Buy(new BacktestBuyActionAdapter()));

            var activeToInactiveRuleList = new Pattern()
            {
                new Rule("Close > Yesterday's Close"),
                new Rule("Close > 2 day ago close"),
            };

            var activeToInactiveSetup = new Setup(activeToInactiveRuleList, activeState, riskProfile);
            activeToInactiveSetup.AddAction(new Sell(new BacktestSellActionAdapter()));

            var inactiveToWatchRuleList = new Pattern()
            {
                new Rule("Close > 90"),
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
            var framelistFactory = new MockFrameListFactory(null, new BackwardTestingDataProvider());
            framelistFactory.DataTarget_Daily = EFile.eAmdDaily;
            framelistFactory.DataTarget_Weekly = EFile.eAmdWeekly;
            framelistFactory.DataTarget_Monthly = EFile.eAmdMonthly;

            var domainController = new BacktestController(activeService, stateMachine, framelistFactory);
            domainController.ScanSetups(setupList, symbols);

            var results = domainController.GetResults().GetHasValidationObjectsOfType<ActionResponse>();

            var positionSummary = domainController.PositionSummary;
            Assert.IsNotNull(positionSummary);
        }


        [TestMethod]
        public void SB_Controllers_04_SimpleTutorialTestSetup()
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
                new ActivePendingState(), // these MUST BE added, although they are only temporarily used
                new InactivePendingState(), // these MUST BE added, although they are only temporarily used
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

            var watchToPrimeRuleList = PresetPattern.ThreeDayPullBack();

            var watchToPrimeSetup = new Setup(watchToPrimeRuleList, watchState, riskProfile);
            watchToPrimeSetup.AddAction(new Move(new BacktestMoveActionAdapter(), primedState));

            var primedToActiveRuleList = PresetPattern.SimpleTwoDayReversal();

            var primeToActiveSetup = new Setup(primedToActiveRuleList, primedState, riskProfile);
            primeToActiveSetup.AddAction(new Buy(new BacktestBuyActionAdapter()));

            var activeToInactiveRuleList = PresetPattern.TwoClosesUnderTheSMA10();

            var activeToInactiveSetup = new Setup(activeToInactiveRuleList, activeState, riskProfile);
            activeToInactiveSetup.AddAction(new Sell(new BacktestSellActionAdapter()));

            var inactiveToWatchRuleList = new Pattern()
            {
                new Rule("SMA(20) > SMA(50)"),
                new Rule("Monthly SMA(5) > Monthly SMA(40)"),
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
            var framelistFactory = new MockFrameListFactory(null, new BackwardTestingDataProvider());
            framelistFactory.DataTarget_Daily = EFile.eAmdDaily;
            framelistFactory.DataTarget_Weekly = EFile.eAmdWeekly;
            framelistFactory.DataTarget_Monthly = EFile.eAmdMonthly;

            var domainController = new BacktestController(activeService, stateMachine, framelistFactory);
            domainController.ScanSetups(setupList, symbols);

            var results = domainController.GetResults().GetHasValidationObjectsOfType<ActionResponse>();

            var positionSummary = domainController.PositionSummary;
            Assert.IsNotNull(positionSummary);
        }
    }
}
