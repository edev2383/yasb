using System;
using System.Collections.Generic;
using System.Linq;
using StockBox.Actions;
using StockBox.Actions.Adapters;
using StockBox.Actions.Responses;
using StockBox.Associations;
using StockBox.Data.SbFrames;
using StockBox.Data.SbFrames.Providers;
using StockBox.Interpreter;
using StockBox.Interpreter.Scanner;
using StockBox.Models;
using StockBox.Positions;
using StockBox.Services;
using StockBox.Setups;
using StockBox.States;
using StockBox.Validation;


namespace StockBox.Controllers
{

    /// <summary>
    /// Class <c>BacktestController</c> analyzes setups against a single Symbol
    /// over historical data, resulting in a ValidationResultList _results
    /// object that can be further analyzed for specific outcomes.
    /// </summary>
    public class BacktestController : SbControllerBase
    {

        public PositionList Positions { get; set; } = new PositionList();
        public PositionSummary PositionSummary { get; set; }

        private ValidationResultList _interpeterExceptions = new ValidationResultList();

        public BacktestController(ISbService service, StateMachine stateMachine, ISbFrameListProvider frameListProvider) : base(service, stateMachine, frameListProvider)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setups"></param>
        /// <param name="profiles"></param>
        public override void ScanSetups(SetupList setups, SymbolProfileList profiles)
        {
            _results.AddRange(ProcessSetups(setups, profiles.First()));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="setups"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        protected override ValidationResultList ProcessSetups(SetupList setups, SymbolProfile symbol)
        {
            var ret = new ValidationResultList();

            // when a setup has been processed, push it here and when we pull
            // a setup from the primary list to test against the symbol, we can
            // check if it is in here already, so we don't have to bother to run
            // it through again.
            SetupList cachedSetups = new SetupList();

            // create the range dataset, and add indicators as needed
            var backtestDataFrames = _frameListProvider.CreateBacktestData(symbol.Symbol) as SbFrameList;

            // we always iterate against the daily, but we need to normalize the
            // weekly and monthly as we traverse the list
            var daily = backtestDataFrames.FindByFrequency(Associations.Enums.EFrequency.eDaily);

            // expose the adapter and create the while loop
            var adapter = daily.GetProvider() as BackwardTestingDataProvider;

            ret.Add(new ValidationResult(adapter != null, "Adapter is not null - is of type 'DeedleBacktestAdapter'"));
            if (ret.HasFailures) return ret;

            adapter.IterateWindow();

            while (!adapter.IsAtEnd())
            {
                bool riskExit = false;
                Position localPosition = Positions.GetCurrentPosition();
                SbFrame localDailyFrame = null;

                // create a local result list to track each iteration
                var innerVr = new ValidationResultList();

                // search the provided setups for the symbol's current state,
                // this should most often be a beginning state, I would think,
                // but for testing specific actions, it can be anything
                var foundSetups = setups.FindAllByState(symbol.State);

                // If there are no setups found w/ the current state, break the
                // loop because the symbol cannot transition if it cannot find
                // a related setup for actions
                innerVr.Add(new ValidationResult(foundSetups.Count > 0, "A Setup was found in the provided list"));
                if (innerVr.HasFailures)
                {
                    ret.AddRange(innerVr);
                    break;
                }

                // Backtesting should be relatively focused and specific, so
                // setups with overlapping states will be ignored and only the
                // first found will be used for the back testing
                var currSetup = foundSetups.First();
                currSetup.AddSymbol(symbol);

                // as we process setups and get the expressions from their rules
                // we cache them in a separate list, so we can know which setups
                // have been processed. We don't need the rules again (for the
                // purposes of analyzing the expressions)
                if (!cachedSetups.ContainsItem(currSetup))
                {
                    // process the setup and break the Rules into expressions
                    currSetup.Process(_service);

                    // analyze the expression for the DomainCombinations
                    var expressionAnalyzer = new ExpressionAnalyzer(currSetup.Rules.Expressions);
                    expressionAnalyzer.Scan();

                    // add indicators to the SbFrameList based on the combos
                    // found by the analyzer
                    _frameListProvider.AddIndicators(backtestDataFrames, expressionAnalyzer.Combos);

                    // toss a clone of the setup in the cache
                    cachedSetups.Add(currSetup.Clone());
                }

                // prepare the daily frame, because we will use that to denote
                // our 'current' price of the stock being tested
                localDailyFrame = backtestDataFrames.FindByFrequency(Associations.Enums.EFrequency.eDaily);

                // create a local StateMachine to be used for transitions
                var localStateMachine = _stateMachine.CreateWithStateAndTransitions();
                localStateMachine.SetCurrentState(symbol.State);

                if (localPosition != null)
                {
                    var riskExitResult = currSetup.RiskProfile.ValidateRiskExit(localPosition, localDailyFrame.FirstDataPoint());
                    riskExit = riskExitResult.Success;
                }

                // if risk exit is true, we don't care about state, or setup or
                // rules or anything. It immediately attempts to sell
                if (riskExit)
                {
                    var sellAction = new Sell(new BacktestSellActionAdapter());
                    sellAction.Symbol = symbol;
                    sellAction.RiskProfile = currSetup.RiskProfile;
                    var riskResponse = sellAction.PerformAction(localDailyFrame.FirstDataPoint());
                    HandleResponse(riskResponse, riskExit);
                    ret.Add(new ValidationResult(EResult.eInfo, "RiskExitPerformed", riskResponse));
                }
                else
                {
                    var interpreter = new SbInterpreter(backtestDataFrames);
                    // Finally... evaluate the setup against the adapter's window
                    var evalResult = currSetup.Evaluate(interpreter);
                    if (evalResult.Success)
                    {
                        // try a state transition, if it's successful, apply to
                        // the current symbol
                        innerVr.AddRange(localStateMachine.TryNextState(currSetup.Action.TransitionState, ref symbol));

                        // if successful, perform the action within the setup
                        if (innerVr.Success)
                        {
                            // if there is a state transition, it will initially
                            // be handled by StateMachine.TryNextState, and any
                            // additional transitions during Backtesting will be
                            // handled by the Action's adapter
                            var vr = PerformSetupActions(currSetup, localDailyFrame.FirstDataPoint());
                            innerVr.AddRange(vr);
                            HandleResponses(vr.GetValidationObjects<ActionResponse>());
                        }
                    }
                    else
                    {
                        // temporary debugging visiblility into interpreter
                        // exceptions
                        _interpeterExceptions.AddRange(interpreter.GetExceptions());
                    }

                    // Add all data to the return object to be added to the
                    // _results
                    ret.AddRange(evalResult);
                    ret.AddRange(interpreter.GetExceptions());
                    ret.AddRange(innerVr);
                }

                // call IterateWindow to move the dataset
                adapter.IterateWindow();
                // normalize Daily/Weekly/Monthly adapters
                backtestDataFrames.Normalize();
            }

            // Create a summary Report of the transactions
            PositionSummary = Positions.CreateSummary();
            return ret;
        }

        protected override ValidationResultList ProcessSetup(Setup setup, SymbolProfileList relatedProfiles)
        {
            throw new NotImplementedException();
        }

        private void HandleResponses(List<ActionResponse> responses)
        {
            foreach (var item in responses)
                HandleResponse(item);
        }

        private void HandleResponse(ActionResponse response, bool isRiskExit = false)
        {
            if (response is BuyActionResponse)
            {
                var transaction = response.Source as Transaction;
                if (transaction != null && transaction.Type == StockBox.Positions.Helpers.ETransactionType.eBuy)
                {
                    var newPosition = new Position(Guid.NewGuid(), transaction.Symbol);
                    newPosition.AddBuy(transaction);
                    Positions.Add(newPosition);
                }
            }

            if (response is SellActionResponse)
            {
                var transaction = response.Source as Transaction;
                if (transaction != null && transaction.Type == StockBox.Positions.Helpers.ETransactionType.eSell)
                {
                    var foundPosition = Positions.GetCurrentPosition();
                    if (foundPosition != null)
                    {
                        foundPosition.AddSell(transaction);
                        foundPosition.RiskExitPerformed = isRiskExit;
                    }
                }
            }
        }
    }
}
