using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Base.Tokens;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.Indicators;
using StockBox.Data.SbFrames;
using StockBox.Data.SbFrames.Providers;
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
            var rules = new Pattern() {
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
            var factory = new FrameListFactory(new SbScraper(), new ForwardTestingDataProvider());
            var frameList = factory.Create(expAnalyzer.Combos, new Symbol("MSFT")) as SbFrameList;

            // find the daily framelist
            var daily = frameList.FindByFrequency(StockBox.Associations.Enums.EFrequency.eDaily);

            // depending on time of day, we might get 4 OR 5?
            // TODO - Needs additional testing.
            Assert.IsTrue(daily.Length >= 4);
        }

        [TestMethod]
        public void SB_FrameList_02_FrameListFactoryCreatesLargeEnoughDataSetFromRulesInput_WEEKLYONLY()
        {
            var rules = new Pattern() {
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
            var factory = new FrameListFactory(new SbScraper(), new ForwardTestingDataProvider());
            var frameList = factory.Create(expAnalyzer.Combos, new Symbol("MSFT")) as SbFrameList;

            // find the daily framelist
            var weekly = frameList.FindByFrequency(StockBox.Associations.Enums.EFrequency.eWeekly);

            // depending on time of day, we might get 4 OR 5?
            // TODO - Needs additional testing.
            Assert.IsTrue(weekly.Length >= 4);
        }

        [TestMethod]
        public void SB_FrameList_03_FrameListFactoryCreatesLargeEnoughDataSetFromRulesInput_MONTHLYONLY()
        {
            var rules = new Pattern() {
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
            var factory = new FrameListFactory(new SbScraper(), new ForwardTestingDataProvider());
            var frameList = factory.Create(expAnalyzer.Combos, new Symbol("MSFT")) as SbFrameList;

            // find the daily framelist
            var monthly = frameList.FindByFrequency(StockBox.Associations.Enums.EFrequency.eMonthly);

            // depending on time of day, we might get 4 OR 5?
            // TODO - Needs additional testing.
            Assert.IsTrue(monthly.Length >= 4);
        }

        [TestMethod]
        public void SB_FrameList_03_FrameListFactoryCreatesLargeEnoughDataSetFromRulesInput_MONTHLYWEEKLYDAILYCOMPLEX()
        {
            var rules = new Pattern() {
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
            var factory = new FrameListFactory(new SbScraper(), new ForwardTestingDataProvider());
            var frameList = factory.Create(expAnalyzer.Combos, new Symbol("MSFT")) as SbFrameList;

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
            // indicators with indices require double the data to ensure that
            // we are able to compute the full range of data.
            // The DateTimeRangeHelper takes the max of Column-Index vs Indicator
            // Indices, but there are probably edge-cases we need to account for
            var rules = new Pattern() {
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
            var factory = new FrameListFactory(new SbScraper(), new ForwardTestingDataProvider());
            var frameList = factory.Create(expAnalyzer.Combos, new Symbol("MSFT")) as SbFrameList;

            // find the daily framelist
            var daily = frameList.FindByFrequency(StockBox.Associations.Enums.EFrequency.eDaily);

            Assert.IsTrue(daily.Length >= 50);
        }

        [TestMethod]
        public void SB_FrameList_05_FramseListFactoryCreateHistoricalData()
        {
            var symbol = new Symbol("AMD");

            var factory = new FrameListFactory(new SbScraper(), new ForwardTestingDataProvider());
            var framelist = factory.CreateBacktestData(symbol) as SbFrameList;

            var foundDaily = framelist.FindByFrequency(StockBox.Associations.Enums.EFrequency.eDaily);
            var foundWeekly = framelist.FindByFrequency(StockBox.Associations.Enums.EFrequency.eWeekly);
            var foundMonthly = framelist.FindByFrequency(StockBox.Associations.Enums.EFrequency.eMonthly);

            Assert.IsNotNull(foundDaily);
            Assert.IsTrue(foundDaily.Length > 0);
            Assert.IsNotNull(foundWeekly);
            Assert.IsTrue(foundWeekly.Length > 0);
            Assert.IsNotNull(foundMonthly);
            Assert.IsTrue(foundMonthly.Length > 0);
        }

        [TestMethod]
        public void SB_FrameList_06_FrameListFactoryCreateHistoricalData_AddIndicatorsByDomainCombos()
        {
            var framelist = CreateBacktestDataFrameList();
            var combos = new DomainCombinationList();
            combos.Add(new DomainCombination(2, new Token(TokenType.eDaily, "", null, 0, 0), "SMA", new int[1] { 25 }));
            combos.Add(new DomainCombination(2, new Token(TokenType.eWeekly, "", null, 0, 0), "SMA", new int[1] { 25 }));
            combos.Add(new DomainCombination(2, new Token(TokenType.eMonthly, "", null, 0, 0), "SMA", new int[1] { 25 }));

            var factory = new FrameListFactory(new SbScraper(), new ForwardTestingDataProvider());
            factory.AddIndicators(framelist, combos);

            var foundDaily = framelist.FindByFrequency(StockBox.Associations.Enums.EFrequency.eDaily);
            var foundWeekly = framelist.FindByFrequency(StockBox.Associations.Enums.EFrequency.eWeekly);
            var foundMonthly = framelist.FindByFrequency(StockBox.Associations.Enums.EFrequency.eMonthly);

            Assert.IsTrue(foundDaily.Inidcators.ContainsItem(new SimpleMovingAverage("SMA", 25)));
            Assert.IsTrue(foundWeekly.Inidcators.ContainsItem(new SimpleMovingAverage("SMA", 25)));
            Assert.IsTrue(foundMonthly.Inidcators.ContainsItem(new SimpleMovingAverage("SMA", 25)));
        }

        private SbFrameList CreateBacktestDataFrameList(string symbol = "AMD")
        {
            var sym = new Symbol(symbol);
            var factory = new FrameListFactory(new SbScraper(), new ForwardTestingDataProvider());
            var framelist = factory.CreateBacktestData(sym) as SbFrameList;
            return framelist;
        }
    }
}
