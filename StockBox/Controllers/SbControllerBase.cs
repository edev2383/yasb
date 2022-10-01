﻿using StockBox.Models;
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
        /// <summary>
        /// Aggregated results of all actions
        /// </summary>
        protected ValidationResultList _results = new ValidationResultList();

        public SbControllerBase(ISbService service, StateMachine stateMachine)
        {
            _service = service;
            _stateMachine = stateMachine;
        }

        /// <summary>
        /// Test the given Setup against the provided SymbolProfileList
        /// </summary>
        /// <param name="setup"></param>
        /// <param name="profiles"></param>
        public void ScanSetup(Setup setup, SymbolProfileList profiles)
        {
            ScanSetup(new SetupList(setup), profiles);

        }

        public abstract void ScanSetup(SetupList setups, SymbolProfileList profiles);
        protected abstract ValidationResultList ProcessSetup(Setup setup, SymbolProfileList relatedProfiles);
        protected abstract ValidationResultList ProcessSetups(SetupList setups, SymbolProfile symbol);
        protected abstract ValidationResultList PerformSetupAction(Setup setup);

        public ValidationResultList GetResults()
        {
            return _results;
        }
    }
}
