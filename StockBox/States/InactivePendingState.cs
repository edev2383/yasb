using System;
using StockBox.Models;


namespace StockBox.States
{
    public class InactivePendingState : StateBase
    {

        public InactivePendingState(InactivePendingState source) : base(source) { }

        public InactivePendingState() : base(
            new StateDataModel
            {
                Name = "InactivePending",
                StateDataModelId = (int)Helpers.EStateType.eInactivePending,
                Type = Helpers.EStateType.eInactivePending
            })
        { }

        public override StateBase Clone()
        {
            return new InactivePendingState(this);
        }
    }
}
