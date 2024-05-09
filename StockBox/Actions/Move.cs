using System;
using StockBox.States;
using StockBox.Actions.Adapters;
using StockBox.Actions.Helpers;
using StockBox.Actions.Responses;
using StockBox.Data.SbFrames;

namespace StockBox.Actions
{

    public class Move : SbActionBase
    {

        public Move(Move source) : base(source) { }

        /// <summary>
        /// Move will be the simplest of Actions, being that it is merely an
        /// update event to the database, moving the state of a given Symbol
        /// to a new state for the current user
        /// </summary>
        /// <param name="adapter"></param>
        /// <param name="watchlistName"></param>
        public Move(ISbActionAdapter adapter, string watchlistName)
            : base(adapter, new UserDefinedState(watchlistName), EActionType.eMoveGeneral)
        { }

        public Move(ISbActionAdapter adapter, StateBase state)
            : base(adapter, state, EActionType.eMoveGeneral)
        { }

        public override ISbAction Clone()
        {
            return new Move(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override ActionResponse Act(DataPoint dataPoint)
        {
            return Adapter.PerformAction(dataPoint);
        }
    }
}
