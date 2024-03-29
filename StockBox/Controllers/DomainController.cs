﻿using System;
using StockBox.Associations;
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
    /// Class <c>DomainController</c> runs all provided setups and symbols as a
    /// snapshot in time, with the zero index being the most recent datetime key
    /// </summary>
    public class DomainController : SbControllerBase
    {

        public DomainController(ISbService service, StateMachine stateMachine, ISbFrameListProvider frameListProvider) : base(service, stateMachine, frameListProvider)
        {

        }


        /// <summary>
        /// Accepts a List of Setups to test against the profile List. Setups
        /// and Profiles are matched by current/origin State
        /// </summary>
        /// <param name="setups"></param>
        /// <param name="profiles"></param>
        public override void ScanSetups(SetupList setups, SymbolProfileList profiles)
        {
            foreach (Setup s in setups)
                _results.AddRange(ProcessSetup(s, profiles.FindBySetup(s)));
        }

        /// <summary>
        /// Test the Setup against a list of matched profiles
        /// </summary>
        /// <param name="setup"></param>
        /// <param name="relatedProfiles"></param>
        /// <returns></returns>
        protected override ValidationResultList ProcessSetup(Setup setup, SymbolProfileList relatedProfiles)
        {
            var ret = new ValidationResultList();

            setup.Process(_service);
            // analyze the expression list to get details about the needed
            // dataset
            var expressionAnalyzer = new ExpressionAnalyzer(setup.Rules.Expressions);
            expressionAnalyzer.Scan();

            var masterFrameList = new SbFrameList();

            // Aggregate all SbFrames for all found SymbolProfiles
            foreach (SymbolProfile sp in relatedProfiles)
            {
                masterFrameList.AddRange(_frameListProvider.Create(expressionAnalyzer.Combos, sp.Symbol));
            }

            foreach (SymbolProfile sp in relatedProfiles)
            {
                var innerVr = new ValidationResultList();

                // create a local setup clone and add the current SymbolProfile
                var localSetup = setup.Clone();
                localSetup.AddSymbol(sp);

                // create a local statemachine to be used within this block,
                // for this local transaction only
                var localSm = _stateMachine.CreateWithStateAndTransitions();

                // apply the current state from the setup (this could also
                // come from the given SymbolProfile, dealer's choice..)
                localSm.SetCurrentState(localSetup.OriginState);

                // get the frames specific to the current Symbol
                var localFrameList = masterFrameList.FindAllBySymbolProvider(sp.Symbol);

                // pass the interpreter, injected w/ the created framelist to
                // the setup and run the evaluation
                var evalResult = localSetup.Evaluate(new SbInterpreter(localFrameList));

                // For evalResult.Success to be true, EVERY Rule in the setup
                // had to have been true.
                if (evalResult.Success)
                {
                    var locsym = sp;
                    innerVr.AddRange(localSm.TryNextState(localSetup.Action.TransitionState, ref locsym));
                    // if the the StateMachine allows the transition, we can
                    // perform the action contained within the Setup
                    if (innerVr.Success)
                    {
                        var dailyFrame = localFrameList.FindByFrequency(Associations.Enums.EFrequency.eDaily);
                        var vr = PerformSetupActions(localSetup, dailyFrame.FirstDataPoint());
                        innerVr.AddRange(vr);
                    }
                }

                // add our results to the aggregate. This return object can be
                // queried to return an action report or log
                ret.AddRange(innerVr);
                ret.AddRange(evalResult);
            }
            return ret;
        }



        protected override ValidationResultList ProcessSetups(SetupList setups, SymbolProfile symbol)
        {
            throw new NotImplementedException();
        }
    }
}
