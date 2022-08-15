using System;
using StockBox.Models;


namespace StockBox.States
{
    public class InactiveErrorState : StateBase
    {

        public InactiveErrorState() : base(
            new StateDataModel
            {
                Name = "InactiveError",
                StateDataModelId = 6,
                Type = Helpers.EStateType.eInactiveError
            })
        { }

    }
}
