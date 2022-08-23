using StockBox.Actions;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.Context;
using StockBox.Data.SbFrames;
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
    public class DomainController
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

                // create a local statemachine to be used within this block,
                // for this local transaction only
                var localSm = _stateMachine.CreateWithStateAndTransitions();

                // apply the current state from the profile (this could also
                // come from the setup..)
                localSm.SetCurrentState(sp.State);

                // pass the service to the Setup, which will break all of the
                // rule statements into an Expression list
                setup.Process(_service);

                // analyze the expression list to get details about the needed
                // dataset
                var expressionAnalyzer = new ExpressionAnalyzer(setup.Rules.Expressions);
                expressionAnalyzer.Scan();

                // create the desired framelist from the provided context
                // provider and the analyzed domain combos
                var factory = new FrameListFactory(new CallContext(""), new DeedleAdapter());
                var frameList = factory.Create(expressionAnalyzer.Combos);

                //
                var evalResult = setup.Evaluate(new SbInterpreter(frameList));
                if (evalResult.Success)
                    innerVr.AddRange(localSm.TryNextState(setup.Action.TransitionState));

                if (innerVr.Success)
                    innerVr.AddRange(PerformSetupAction(setup));
            }
            return ret;
        }

        private ValidationResultList PerformSetupAction(Setup setup)
        {
            var ret = new ValidationResultList();
            var action = setup.Action.PerformAction();
            return ret;
        }
    }
}
