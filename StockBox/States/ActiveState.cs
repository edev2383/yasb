using System;
using StockBox.Models;


namespace StockBox.States
{
    public class ActiveState : StateBase
    {

        public ActiveState() : base(
            new StateDataModel
            {
                Name = "Active",
                StateDataModelId = 2,
                Type = Helpers.EStateType.eActive
            })
        { }

    }
}
