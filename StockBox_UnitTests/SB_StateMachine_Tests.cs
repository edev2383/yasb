using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Actions;
using StockBox.Interpreter;
using StockBox.Interpreter.Scanner;
using StockBox.Models;
using StockBox.Rules;
using StockBox.Services;
using StockBox.Setups;
using StockBox.States;
using StockBox_UnitTests.Accessors;


namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_StateMachine_Tests
    {

        [TestMethod]
        public void SB_StateMachine_01_TransitionCanBeCreated()
        {
            var transition = new Transition(new UserDefinedState("start"), new UserDefinedState("end"));
            Assert.IsNotNull(transition);
        }

        [TestMethod]
        public void SB_StateMachine_02_MachineCanBeCreated()
        {
            var sm = new StateMachine(new StateList());
            Assert.IsNotNull(sm);
        }

        [TestMethod]
        public void SB_StateMachine_03_StateBaseOperatorOverrideWorksAsExpected()
        {
            var stateOne = new UserDefinedState("testOne");
            var stateTwo = new UserDefinedState("testTwo");
            Assert.IsFalse(stateOne.Equals(stateTwo));
            Assert.IsTrue(stateOne != stateTwo);
        }

        [TestMethod]
        public void SB_StateMachine_04_TransitionOperatorOverrideWorksAsExpected()
        {
            var transitionOne = new Transition(new UserDefinedState("testOne"), new UserDefinedState("testTwo"));
            var transitionTwo = new Transition(new UserDefinedState("testOne"), new UserDefinedState("testTwo"));
            Assert.IsTrue(transitionOne.Equals(transitionTwo));
        }

        [TestMethod]
        public void SB_StateMachine_05_TransitionOperatorOverrideWorksAsExpected()
        {
            var transitionOne = new Transition(new UserDefinedState("testOne"), new UserDefinedState("testTwo"));
            var transitionTwo = new Transition(new UserDefinedState("testOne"), new UserDefinedState("diffTestTwo"));
            Assert.IsFalse(transitionOne.Equals(transitionTwo));
        }

        [TestMethod]
        public void SB_StateMachine_06_StateBaseOperatorOverrideWorksAsExpected()
        {
            var stateOne = new ActiveState();
            var stateTwo = new ActiveState();
            Assert.IsTrue(stateOne == stateTwo);
        }

        [TestMethod]
        public void SB_StateMachine_07_VerifyTransitionsCannotBeDuplicated()
        {
            var validStates = new StateList {
                new UserDefinedState("Testing"), new ActivePendingState(), new ActiveState()
            };
            var sm = new StateMachine_Accessor(validStates);
            var transitionOne = new Transition(new UserDefinedState("Testing"), new ActivePendingState());
            var transitionTwo = new Transition(new ActivePendingState(), new ActiveState());
            sm.AddTransition(transitionOne);
            sm.AddTransition(transitionTwo);
            sm.AddTransition(transitionOne);
            var vr = sm.TransitionCanBeAdded(transitionOne);

            Assert.AreEqual(sm.GetTransitions().Count, 2);
            Assert.IsFalse(vr.Success);
        }

        [TestMethod]
        public void SB_StateMachine_08_StateMachineCanTransitionToNewState()
        {
            var validStates = new StateList {
                new UserDefinedState("Start"),
                new InactiveState(),
                new ActiveState(),
            };
            var sm = new StateMachine_Accessor(validStates, new UserDefinedState("Start"));

            sm.AddTransition(new Transition(new UserDefinedState("Start"), new InactiveState()));
            sm.AddTransition(new Transition(new InactiveState(), new ActiveState()));
            Assert.IsTrue(sm.CurrentState.Equals(new UserDefinedState("Start")));

            sm.TryNextState(new InactiveState());
            Assert.IsTrue(sm.CurrentState.Equals(new InactiveState()));
        }

        [TestMethod]
        public void SB_StateMachine_09_StateMachinePreventsInvalidTransition()
        {
            var validStates = new StateList {
                new UserDefinedState("Start"),
                new InactiveState(),
                new ActiveState(),
            };
            var sm = new StateMachine_Accessor(validStates, new UserDefinedState("Start"));

            sm.AddTransition(new Transition(new UserDefinedState("Start"), new InactiveState()));
            sm.AddTransition(new Transition(new InactiveState(), new ActiveState()));
            Assert.IsTrue(sm.CurrentState.Equals(new UserDefinedState("Start")));

            sm.TryNextState(new ActiveState());
            Assert.IsFalse(sm.CurrentState.Equals(new ActiveState()));
        }

        [TestMethod, Description("Confirm that our Setup, Rules, Actions, and StateMachine all integrate properly")]
        public void SB_StateMachine_10_StateMachineSetupIntegration_TransitionSuccessful()
        {
            // user-defined states are defined by store string values, for now
            const string config_targetWatchList = "TargetWatchlist";
            var config_targetState = new StateDataModel(7, config_targetWatchList, StockBox.States.Helpers.EStateType.eUserDefined);

            // create a setup with a simple rulelist and action
            var ruleList = new RuleList { new Rule("1 == 1"), new Rule("1 == 1"), };
            var setup = new Setup(ruleList);
            setup.AddAction(new Move(null, config_targetWatchList));

            // define the valid states for the StateMachine
            var validStates = new StateList {
                new UserDefinedState("start"),
                new UserDefinedState(config_targetState)
            };

            // create the statemachine and define a valid transition
            var sm = new StateMachine(validStates, new UserDefinedState("start"));
            sm.AddTransition(new Transition(new UserDefinedState("start"), new UserDefinedState(config_targetState)));

            // create the service to perform the interpeting
            var scanner = new Scanner();
            var parser = new Parser();
            var intepreter = new SbInterpreter();
            var service = new ActiveService(scanner, parser);

            setup.Process(service);
            // run the interpeter process against the setup
            // note: if the setup contains rules that have domain expressions,
            // i.e., Close, SMA, etc, between these two steps will require us
            // to analyze the expression list AND load data to an SBFrame
            // this is a simple rule example, so that step is not needed here
            var result = setup.Evaluate(intepreter);

            // Confirm the expected Success result
            Assert.IsTrue(result.Success);

            // if the interpreter response is successful, try the next state
            // of the statemachine, based on the action returned by the setup
            if (result.Success)
                sm.TryNextState(setup.Action.TransitionState);

            // Assert that we are now in a state identical to the one expected
            Assert.AreEqual(sm.CurrentState, new UserDefinedState(config_targetState));
            Assert.AreNotEqual(sm.CurrentState, new UserDefinedState("start"));
        }

        [TestMethod, Description("Confirm that our Setup, Rules, Actions, and StateMachine all integrate properly")]
        public void SB_StateMachine_11_StateMachineSetupIntegration_TransitionFailure()
        {
            // config StateDataModels
            const string config_startWatchList = "Start";
            var config_startState = new StateDataModel(9, config_startWatchList, StockBox.States.Helpers.EStateType.eUserDefined);
            const string config_givenWatchList = "ProvidedWatchlist";
            var config_givenState = new StateDataModel(7, config_givenWatchList, StockBox.States.Helpers.EStateType.eUserDefined);
            const string config_targetWatchList = "TargetWatchlist";
            var config_targetState = new StateDataModel(8, config_targetWatchList, StockBox.States.Helpers.EStateType.eUserDefined);

            // create a setup with a simple rulelist and action
            var ruleList = new RuleList { new Rule("1 == 1"), new Rule("2 >= 1"), };
            var setup = new Setup(ruleList);
            setup.AddAction(new Move(null, config_targetWatchList));

            // define the valid states for the StateMachine
            var validStates = new StateList {
                new UserDefinedState(config_startState),
                new UserDefinedState(config_givenState),
                new UserDefinedState(config_targetState),
            };

            // create the statemachine and define a valid transition
            var sm = new StateMachine(validStates, new UserDefinedState(config_startState));
            sm.AddTransition(new Transition(new UserDefinedState(config_startState), new UserDefinedState(config_givenState)));

            // create the service to perform the interpeting
            var scanner = new Scanner();
            var parser = new Parser();
            var intepreter = new SbInterpreter();
            var service = new ActiveService(scanner, parser);

            // use the service to turn the rule statements into domain exprs
            setup.Process(service);

            // run the interpeter process against the setup
            // note: if the setup contains rules that have domain expressions,
            // i.e., Close, SMA, etc, between these two steps will require us
            // to analyze the expression list AND load data to an SBFrame
            // this is a simple rule example, so that step is not needed here
            var result = setup.Evaluate(intepreter);

            // Confirm the expected Success result
            Assert.IsTrue(result.Success);

            // if the interpreter response is successful, try the next state
            // of the statemachine, based on the action returned by the setup
            if (result.Success)
                // attempt to transition the the setup next state
                sm.TryNextState(setup.Action.TransitionState);

            // Assert that we did not transition to the action state
            Assert.AreNotEqual(sm.CurrentState, setup.Action.TransitionState);
            Assert.AreEqual(sm.CurrentState, new UserDefinedState(config_startState));
        }
    }
}
