using System;
using StockBox.Actions;
using StockBox.Data.SbFrames;
using StockBox.Interpreter;
using StockBox.Models;
using StockBox.RiskProfiles;
using StockBox.Rules;
using StockBox.Services;
using StockBox.States;
using StockBox.Validation;


namespace StockBox.Setups
{

    /// <summary>
    /// Class <c>Setup</c> tracks the provided RuleList and related Actions/Risk
    /// Profile
    /// </summary>
    public class Setup : IValidationResultsListProvider
    {
        /// <summary>
        /// Identifying Guid 
        /// </summary>
        public Guid? Token { get; set; }

        /// <summary>
        /// The rules that determine a given setup
        /// </summary>
        public Pattern Rules { get { return _rules; } }

        /// <summary>
        /// Explicitly defined origin state
        /// </summary>
        public StateBase OriginState { get { return _originState; } }
        private readonly StateBase _originState;

        /// <summary>
        /// All actions associated with a setup. The general assumption is that
        /// there will be a single action, but it's more likely that there will
        /// be multiple. Example: User's setup evaluates to `true` and performs
        /// an Move and an Alert. 
        /// </summary>
        public SbActionList Actions
        {
            get
            {
                if (_actions == null)
                    _actions = new SbActionList();
                return _actions;
            }
        }

        /// <summary>
        /// An action is an encapsulation of behavior called when a Setup
        /// process returns true/Success
        /// </summary>
        public ISbAction Action { get { return _actions.First; } }

        /// <summary>
        /// The profile of risk associated with a given setup
        /// </summary>
        public RiskProfile RiskProfile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SymbolProfile SymbolProfile { get; set; }

        private SbActionList _actions = new SbActionList();
        private readonly Pattern _rules;

        public Setup()
        {
        }

        public Setup(Pattern rules)
        {
            _rules = rules;
        }

        public Setup(Pattern rules, RiskProfile profile) : this(rules)
        {
            RiskProfile = profile;
        }

        public Setup(Pattern rules, StateBase originState, RiskProfile profile)
        {
            _rules = rules;
            _originState = originState;
            RiskProfile = profile;
        }

        public Setup(Setup source) : this(source._rules.Clone(), source._originState.Clone(), source.RiskProfile.Clone())
        {
            _actions = source._actions;
        }

        /// <summary>
        /// Evaluate the _rules List<Expr>
        /// </summary>
        /// <param name="interpreter"></param>
        /// <returns></returns>
        public ValidationResultList Evaluate(SbInterpreter interpreter)
        {
            return _rules.Evalute(interpreter);
        }


        /// <summary>
        /// Use the provided service to Process the Setup's _rules and turn the
        /// Rule.Statements into domain Expressions. These Expressions can then
        /// be analyzed and Evaluated 
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public void Process(ISbService service)
        {
            _rules.ProcessRules(service);
        }

        public void AddSymbol(SymbolProfile symbol)
        {
            SymbolProfile = symbol;
            foreach (var action in _actions)
                action.Symbol = symbol;
        }

        public void AddAction(SbActionBase action)
        {
            if (RiskProfile != null)
                action.RiskProfile = RiskProfile;
            _actions.Add(action);
        }

        public Setup Clone()
        {
            return new Setup(this);
        }

        public ValidationResultList GetResults()
        {
            return _rules.GetResults();
        }

        public bool IdentifiesAs(Setup item)
        {
            foreach (var rule in Rules)
                if (!item.Rules.ContainsItem(rule))
                    return false;
            if (!OriginState.Equals(item.OriginState)) return false;
            return true;
        }

        /// <summary>
        /// Perform all related actions within a setup.
        ///
        /// TODO - Need to perform the actions and return them to the
        /// Controller in a way that preserves the overall status
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <returns></returns>
        public ValidationResultList PerformActions(DataPoint dataPoint)
        {
            var ret = new ValidationResultList();
            foreach (ISbAction action in _actions)
                // add the ActionResponse object to the ret list. ActionResponse
                // is an IValidationResultProvider, so the VRList knows how to
                // parse it as a result
                ret.Add(action.PerformAction(dataPoint));
            return ret;
        }
    }
}
