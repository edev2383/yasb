using System;
using StockBox.Models;


namespace StockBox.States
{
    public class InactivePendingState : StateBase
    {

        public InactivePendingState() : base(
            new StateDataModel
            {
                Name = "InactivePending",
                StateDataModelId = 4,
                Type = Helpers.EStateType.eInactivePending
            })
        { }

    }
}
