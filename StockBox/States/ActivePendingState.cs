using System;
using StockBox.Models;


namespace StockBox.States
{
    public class ActivePendingState : StateBase
    {

        public ActivePendingState() : base(
            new StateDataModel
            {
                Name = "ActivePending",
                StateDataModelId = 1,
                Type = Helpers.EStateType.eActivePending
            })
        { }

    }
}
