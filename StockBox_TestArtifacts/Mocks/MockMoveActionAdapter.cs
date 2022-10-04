using System;
using StockBox.Actions;
using StockBox.Actions.Adapters;
using StockBox.Actions.Responses;
using StockBox.Data.SbFrames;

namespace StockBox_TestArtifacts.Mocks
{

    /// <summary>
    /// Class <c>MockMoveActionAdapter</c> returns a message saying the symbol
    /// was transitioned to the next state. The real MoveAdapter will call for
    /// the state to be changed in the data layer
    /// </summary>
    public class MockMoveActionAdapter : SbActionAdapterBase
    {

        public MockMoveActionAdapter() : base()
        {
        }


        public override ActionResponse PerformAction(DataPoint dataPoint)
        {
            var ret = new ActionResponse(true);
            ParentAction.Symbol.TransitionState(ParentAction.TransitionState);
            ret.Message = $"Symbol {ParentAction.Symbol.Symbol.Name} was moved to {ParentAction.TransitionState.Name}";
            return ret;
        }
    }
}
