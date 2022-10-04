using System;
using StockBox.Actions.Adapters;
using StockBox.Actions.Helpers;
using StockBox.Actions.Responses;
using StockBox.Data.SbFrames;
using StockBox.States;


namespace StockBox.Actions
{

    /// <summary>
    /// Class <c>Alert</c> sends a message to a User. The method of which is
    /// determined by the User context.
    /// </summary>
    public class Alert : SbActionBase
    {

        public Alert(SbActionBase source) : base(source)
        {
        }

        public Alert(ISbActionAdapter adapter, StateBase transitionState) : base(adapter, transitionState, EActionType.eAlert)
        {
        }

        public override ISbAction Clone()
        {
            return new Alert(this);
        }

        public override ActionResponse PerformAction(DataPoint dataPoint)
        {
            return null;
        }
    }
}
