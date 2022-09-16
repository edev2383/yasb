using StockBox.Actions;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.Context;
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
    /// DomainController runs all provided setups and symbols as a snapshot in
    /// time, with the zero index being the most recent datetime key
    /// </summary>
    public class DomainController : IValidationResultsListProvider
    {

        private readonly ISbService _service;
        private readonly StateMachine _stateMachine;

        private ValidationResultList _results = new ValidationResultList();

        public DomainController(ISbService service, StateMachine stateMachine)
        {
            _service = service;
            _stateMachine = stateMachine;
        }

        public void ScanSetup(Setup setup, SymbolProfileList profiles)
        {
            ScanSetup(new SetupList(setup), profiles);
        }

        public void ScanSetup(SetupList setups, SymbolProfileList profiles)
        {
            foreach (Setup s in setups)
                _results.AddRange(ProcessSetup(s, profiles.FindBySetup(s)));
        }

        private ValidationResultList ProcessSetup(Setup setup, SymbolProfileList relatedProfiles)
        {
            var ret = new ValidationResultList();
            foreach (SymbolProfile sp in relatedProfiles)
            {
                var innerVr = new ValidationResultList();

                // create a local setup clone and add the current SymbolProfile
                var localSetup = setup.Clone();
                localSetup.SymbolProfile = sp;

                // create a local statemachine to be used within this block,
                // for this local transaction only
                var localSm = _stateMachine.CreateWithStateAndTransitions();

                // apply the current state from the setup (this could also
                // come from the given SymbolProfile, dealer's choice..)
                localSm.SetCurrentState(localSetup.OriginState);

                // pass the service to the Setup, which will break all of the
                // rule statements into an Expression list
                localSetup.Process(_service);

                // analyze the expression list to get details about the needed
                // dataset
                var expressionAnalyzer = new ExpressionAnalyzer(localSetup.Rules.Expressions);
                expressionAnalyzer.Scan();

                // create the desired framelist from the provided context
                // provider and the analyzed domain combos
                var factory = new FrameListFactory(new SbScraper(), new DeedleAdapter());
                var frameList = factory.Create(expressionAnalyzer.Combos, sp.Symbol);

                // pass the interpreter, injected w/ the created framelist to
                // the setup and run the evaluation
                var evalResult = localSetup.Evaluate(new SbInterpreter(frameList));

                // if there are no errors to the evaluation, try to transition
                // to the newest state
                if (evalResult.Success)
                {
                    innerVr.AddRange(localSm.TryNextState(localSetup.Action.TransitionState));
                    if (innerVr.Success)
                        innerVr.AddRange(PerformSetupAction(localSetup));
                }

                ret.AddRange(innerVr);
                ret.AddRange(evalResult);
            }
            return ret;
        }

        private ValidationResultList PerformSetupAction(Setup setup)
        {
            var vr = new ValidationResultList();
            vr.Add(new ValidationResult(setup.Action != null, "Setup MUST HAVE an action"));
            if (vr.Success)
                setup.Action.PerformAction();
            return vr;
        }

        public ValidationResultList GetResults()
        {
            return _results;
        }
    }
}
