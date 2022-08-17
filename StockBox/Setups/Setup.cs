using StockBox.Actions;
using StockBox.RiskProfiles;
using StockBox.Rules;
using StockBox.Services;
using StockBox.Validation;


namespace StockBox.Setups
{

    /// <summary>
    /// Setup tracks the provided RuleList and related Actions/Risk Profile
    /// </summary>
    public class Setup
    {

        /// <summary>
        /// The rules that determine a given setup
        /// </summary>
        public RuleList Rules { get { return _rules; } }

        /// <summary>
        /// All actions associated with a setup
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
        public SbActionBase Action { get { return _actions.First; } }

        /// <summary>
        /// The profile of risk associated with a given setup
        /// </summary>
        public RiskProfile RiskProfile { get; set; }

        private SbActionList _actions = new SbActionList();
        private readonly RuleList _rules;

        public Setup()
        {
        }

        public Setup(RuleList rules)
        {
            _rules = rules;
        }

        public Setup(RuleList rules, RiskProfile profile) : this(rules)
        {
            RiskProfile = profile;
        }

        public ValidationResultList Process(ISbService service)
        {
            return _rules.Process(service);
        }

        public void AddAction(SbActionBase action)
        {
            if (RiskProfile != null)
                action.RiskProfile = RiskProfile;
            _actions.Add(action);
        }
    }
}
