﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;
using StockBox.Data.Scraper;
using StockBox.Interpreter.Scanner;
using StockBox.Models;
using StockBox.Rules;
using StockBox.Services;
using StockBox.Setups;
using StockBox.States;


namespace StockBox_IntegrationTests
{

    [TestClass]
    public class SB_FrameList_Tests
    {

        [TestMethod]
        public void SB_FrameList_01_FrameListFactoryCreatesLargeEnoughDataSetFromRulesInput_DAILYONLY()
        {
            var rules = new RuleList() {
                new Rule("close > open"),
                new Rule("CLOSE > 60")
            };

            // setup and process the rules via the service
            var setup = new Setup(rules, new UserDefinedState("start"), new StockBox.RiskProfiles.RiskProfile());
            var activeService = new ActiveService(new Scanner(), new Parser());
            setup.Process(activeService);

            // analyze the expressions found in the rulelist
            var expAnalyzer = new ExpressionAnalyzer(setup.Rules.Expressions);
            expAnalyzer.Scan();

            // create the factory and give it the analyzed combinations
            var factory = new FrameListFactory(new SbScraper(), new DeedleAdapter());
            var frameList = factory.Create(expAnalyzer.Combos, new Symbol("MSFT"));

            // find the daily framelist
            var daily = frameList.FindByFrequency(StockBox.Associations.Enums.EFrequency.eDaily);

            // depending on time of day, we might get 4 OR 5?
            // TODO - Needs additional testing.
            Assert.IsTrue(daily.Length >= 4);
        }

        [TestMethod]
        public void SB_FrameList_02_FrameListFactoryCreatesLargeEnoughDataSetFromRulesInput_WEEKLYONLY()
        {
            var rules = new RuleList() {
                new Rule("weekly close > weekly open"),
                new Rule("weekly CLOSE > 60")
            };

            // setup and process the rules via the service
            var setup = new Setup(rules, new UserDefinedState("start"), new StockBox.RiskProfiles.RiskProfile());
            var activeService = new ActiveService(new Scanner(), new Parser());
            setup.Process(activeService);

            // analyze the expressions found in the rulelist
            var expAnalyzer = new ExpressionAnalyzer(setup.Rules.Expressions);
            expAnalyzer.Scan();

            // create the factory and give it the analyzed combinations
            var factory = new FrameListFactory(new SbScraper(), new DeedleAdapter());
            var frameList = factory.Create(expAnalyzer.Combos, new Symbol("MSFT"));

            // find the daily framelist
            var weekly = frameList.FindByFrequency(StockBox.Associations.Enums.EFrequency.eWeekly);

            // depending on time of day, we might get 4 OR 5?
            // TODO - Needs additional testing.
            Assert.IsTrue(weekly.Length >= 4);
        }

        [TestMethod]
        public void SB_FrameList_03_FrameListFactoryCreatesLargeEnoughDataSetFromRulesInput_MONTHLYONLY()
        {
            var rules = new RuleList() {
                new Rule("Monthly close > monthly open"),
                new Rule("MONTHLY CLOSE > 60")
            };

            // setup and process the rules via the service
            var setup = new Setup(rules, new UserDefinedState("start"), new StockBox.RiskProfiles.RiskProfile());
            var activeService = new ActiveService(new Scanner(), new Parser());
            setup.Process(activeService);

            // analyze the expressions found in the rulelist
            var expAnalyzer = new ExpressionAnalyzer(setup.Rules.Expressions);
            expAnalyzer.Scan();

            // create the factory and give it the analyzed combinations
            var factory = new FrameListFactory(new SbScraper(), new DeedleAdapter());
            var frameList = factory.Create(expAnalyzer.Combos, new Symbol("MSFT"));

            // find the daily framelist
            var monthly = frameList.FindByFrequency(StockBox.Associations.Enums.EFrequency.eMonthly);

            // depending on time of day, we might get 4 OR 5?
            // TODO - Needs additional testing.
            Assert.IsTrue(monthly.Length >= 4);
        }

        [TestMethod]
        public void SB_FrameList_03_FrameListFactoryCreatesLargeEnoughDataSetFromRulesInput_MONTHLYWEEKLYDAILYCOMPLEX()
        {
            var rules = new RuleList() {
                new Rule("Monthly close > weekly open"),
                new Rule("CLOSE > 60")
            };

            // setup and process the rules via the service
            var setup = new Setup(rules, new UserDefinedState("start"), new StockBox.RiskProfiles.RiskProfile());
            var activeService = new ActiveService(new Scanner(), new Parser());
            setup.Process(activeService);

            // analyze the expressions found in the rulelist
            var expAnalyzer = new ExpressionAnalyzer(setup.Rules.Expressions);
            expAnalyzer.Scan();

            // create the factory and give it the analyzed combinations
            var factory = new FrameListFactory(new SbScraper(), new DeedleAdapter());
            var frameList = factory.Create(expAnalyzer.Combos, new Symbol("MSFT"));

            // find the daily framelist
            var monthly = frameList.FindByFrequency(StockBox.Associations.Enums.EFrequency.eMonthly);
            var weekly = frameList.FindByFrequency(StockBox.Associations.Enums.EFrequency.eWeekly);
            var daily = frameList.FindByFrequency(StockBox.Associations.Enums.EFrequency.eDaily);

            // depending on time of day, we might get 4 OR 5?
            // TODO - Needs additional testing.
            Assert.IsTrue(monthly.Length >= 4);
            Assert.IsTrue(weekly.Length >= 4);
            Assert.IsTrue(daily.Length >= 4);
        }

        [TestMethod]
        public void SB_FrameList_04_FrameListFactoryCreatesLargeEnoughDataSetFromRulesInput_Indicators()
        {
            var rules = new RuleList() {
                new Rule("Close > SMA(25)"),
            };

            // setup and process the rules via the service
            var setup = new Setup(rules, new UserDefinedState("start"), new StockBox.RiskProfiles.RiskProfile());
            var activeService = new ActiveService(new Scanner(), new Parser());
            setup.Process(activeService);

            // analyze the expressions found in the rulelist
            var expAnalyzer = new ExpressionAnalyzer(setup.Rules.Expressions);
            expAnalyzer.Scan();

            // create the factory and give it the analyzed combinations
            var factory = new FrameListFactory(new SbScraper(), new DeedleAdapter());
            var frameList = factory.Create(expAnalyzer.Combos, new Symbol("MSFT"));

            // find the daily framelist
            var daily = frameList.FindByFrequency(StockBox.Associations.Enums.EFrequency.eDaily);

            // depending on time of day, we might get 4 OR 5?
            // TODO - Needs additional testing.
            Assert.IsTrue(daily.Length >= 50);
        }
    }
}