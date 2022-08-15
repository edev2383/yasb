using System;
using StockBox.Models;


namespace StockBox.States
{
    public class ActiveErrorState : StateBase
    {

        public ActiveErrorState() : base(
            new StateDataModel
            {
                Name = "ActiveError",
                StateDataModelId = 3,
                Type = Helpers.EStateType.eActiveError
            })
        { }

    }
}
