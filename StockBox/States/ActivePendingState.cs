using System;
using StockBox.Models;


namespace StockBox.States
{
    public class ActivePendingState : StateBase
    {

        public ActivePendingState(ActivePendingState source) : base(source) { }

        public ActivePendingState() : base(
            new StateDataModel
            {
                Name = "ActivePending",
                StateDataModelId = (int)Helpers.EStateType.eActivePending,
                Type = Helpers.EStateType.eActivePending
            })
        { }

        public override StateBase Clone()
        {
            return new ActivePendingState(this);
        }
    }
}
