using StockBox.Associations;
using StockBox.Data.Context;
using StockBox.Data.SbFrames;
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
    public abstract class SbControllerBase : ISbController, IValidationResultsListProvider
    {

        /// <summary>
        /// The ISbService is responsible for Scanning and Parsing the rule
        /// statements into Expressions
        /// </summary>
        protected readonly ISbService _service;
        /// <summary>
        /// The StateMachine defines all valid user transitions and will confirm
        /// the new state of any SymbolProfile
        /// </summary>
        protected readonly StateMachine _stateMachine;

        protected readonly ISbFrameListProvider _frameListProvider;
        /// <summary>
        /// Aggregated results of all actions
        /// </summary>
        protected ValidationResultList _results = new ValidationResultList();

        public SbControllerBase(ISbService service, StateMachine stateMachine, ISbFrameListProvider frameListProvider)
        {
            _service = service;
            _stateMachine = stateMachine;
            _frameListProvider = frameListProvider;
        }

        /// <summary>
        /// Test the given Setup against the provided SymbolProfileList
        /// </summary>
        /// <param name="setup"></param>
        /// <param name="profiles"></param>
        public void ScanSetup(Setup setup, SymbolProfileList profiles)
        {
            ScanSetups(new SetupList(setup), profiles);

        }

        public abstract void ScanSetups(SetupList setups, SymbolProfileList profiles);
        protected abstract ValidationResultList ProcessSetup(Setup setup, SymbolProfileList relatedProfiles);
        protected abstract ValidationResultList ProcessSetups(SetupList setups, SymbolProfile symbol);

        /// <summary>
        /// Perform the action contained within the Setup. This includes, but
        /// is not limited to Move(), Buy(), Sell() actions. 
        /// </summary>
        /// <param name="setup"></param>
        /// <returns></returns>
        protected virtual ValidationResultList PerformSetupAction(Setup setup, DataPoint dataPoint)
        {
            var vr = new ValidationResultList();
            vr.Add(new ValidationResult(setup.Action != null, "Setup MUST HAVE an action"));
            if (vr.Success)
            {
                var actionResponse = setup.Action.PerformAction(dataPoint);
                vr.Add(new ValidationResult(actionResponse.IsSuccess, setup.ToString(), actionResponse));
            }
            return vr;
        }

        public ValidationResultList GetResults()
        {
            return _results;
        }
    }
}
