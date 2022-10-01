using System;
using System.Linq;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;
using StockBox.Data.Scraper;
using StockBox.Interpreter;
using StockBox.Interpreter.Scanner;
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

        protected override ValidationResultList ProcessSetups(SetupList setups, SymbolProfile symbol)
        {
            var ret = new ValidationResultList();
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

            // we always iterate against the daily, but we need to normalize the
            // weekly and monthly as we traverse the list
            var daily = backtestDataFrames.FindByFrequency(Associations.Enums.EFrequency.eDaily);

            // expose the adapter and create the while loop
            var adapter = daily.GetAdapter() as DeedleBacktestAdapter;
            while (!adapter.IsAtEnd())
            {
                // create a local VRL
                var innerVr = new ValidationResultList();

                // search the provided setups for the symbol's current state,
                // this should most often be a beginning state, I would think,
                // but for testing specific actions, it can be anything
                var foundSetups = setups.FindAllByState(symbol.State);

                // If there are no setups found w/ the current state, break the
                // loop because the symbol cannot transition if it cannot find
                // a related setup for actions
                innerVr.Add(new ValidationResult(foundSetups.Count > 0, "A Setup was found in the provided list"));
                if (innerVr.HasFailures) break;

                // Backtesting should be relatively focused and specific, so
                // setups with overlapping states will be ignored and only the
                // first found will be used for the back testing
                var currSetup = foundSetups.First();

                // as we process setups and get the expressions from their rules
                // we cache them in a separate list, so we can know which setups
                // have been processed. We don't need the rules again (for the
                // purposes of analyzing the expressions
                if (!cachedSetups.ContainsItem(currSetup))
                {
                    // process the setup and break the Rules into expressions
                    currSetup.Process(_service);

                    // analyze the expression for the DomainCombinations
                    var expressionAnalyzer = new ExpressionAnalyzer(currSetup.Rules.Expressions);
                    expressionAnalyzer.Scan();

                    // add indicators to the SbFrameList based on the combos
                    // found by the analyzer
                    factory.AddIndicators(backtestDataFrames, expressionAnalyzer.Combos);

                    // toss a clone of the setup in the cache
                    cachedSetups.Add(currSetup.Clone());
                }

                // create a local StateMachine to be used for transitions
                var localStateMachine = _stateMachine.CreateWithStateAndTransitions();
                localStateMachine.SetCurrentState(symbol.State);

                // Finally... evaluate the setup against the adapter's window
                var evalResult = currSetup.Evaluate(new SbInterpreter(backtestDataFrames));
                if (evalResult.Success)
                {
                    // try a state transition
                    innerVr.AddRange(localStateMachine.TryNextState(currSetup.Action.TransitionState));

                    // if successful, perform the action within the setup
                    if (innerVr.Success)
                        innerVr.AddRange(PerformSetupAction(currSetup));
                }

                // Add all data to the return object, which will be analyzed
                // later
                ret.AddRange(evalResult);
                ret.AddRange(innerVr);

                // call IterateWindow to move the dataset
                adapter.IterateWindow();
                // normalize Daily/Weekly/Monthly adapters
                backtestDataFrames.Normalize();
            }

            return ret;
        }

        protected override ValidationResultList ProcessSetup(Setup setup, SymbolProfileList relatedProfiles)
        {
            throw new NotImplementedException();
        }
    }
}
