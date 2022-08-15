using System;
using StockBox.Models;


namespace StockBox.States
{
    public class InactiveState : StateBase
    {

        public InactiveState() : base(
            new StateDataModel
            {
                Name = "Inactive",
                StateDataModelId = 5,
                Type = Helpers.EStateType.eInactive
            })
        { }

    }
}
