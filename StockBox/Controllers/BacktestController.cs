using System;
using System.Linq;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;
using StockBox.Data.Scraper;
using StockBox.Models;
using StockBox.Services;
using StockBox.Setups;
using StockBox.States;
using StockBox.Validation;


namespace StockBox.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    public class BacktestController : SbControllerBase
    {
        public BacktestController(ISbService service, StateMachine stateMachine) : base(service, stateMachine)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setups"></param>
        /// <param name="profiles"></param>
        public override void ScanSetup(SetupList setups, SymbolProfileList profiles)
        {
            var symbol = profiles.First();
            // to scan backtest behavior, we need to iterate through the
            // data, while matching the appropriate setup from the
            // current state of the profile

            // when a setup has been processed, push it here and when we pull
            // a setup from the primary list to test agains the symbol, we can
            // check if it is in here already, so we don't have to both running
            // it through again.
            SetupList cachedSetups = new SetupList();

            var factory = new FrameListFactory(new SbScraper(), new DeedleBacktestAdapter());

            // create the range dataset, and add indicators as needed
            var backtestDataFrames = factory.CreateBacktestData(symbol.Symbol);


            // need to get a deep set of data

            // and iterate through the keys of that dataset

            // window..?
            foreach (Setup setup in setups)
                _results.AddRange(ProcessSetup(setup, profiles.FindBySetup(setup)));
        }

        protected override ValidationResultList PerformSetupAction(Setup setup)
        {
            throw new NotImplementedException();
        }

        protected override ValidationResultList ProcessSetup(Setup setup, SymbolProfileList relatedProfiles)
        {
            throw new NotImplementedException();
        }
    }
}
