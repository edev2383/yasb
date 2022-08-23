using System;
using StockBox.Models;


namespace StockBox.States
{

    public class ActiveState : StateBase
    {
        public ActiveState(ActiveState source) : base(source) { }

        public ActiveState() : base(
            new StateDataModel
            {
                Name = "Active",
                StateDataModelId = (int)Helpers.EStateType.eActive,
                Type = Helpers.EStateType.eActive
            })
        { }

        public override StateBase Clone()
        {
            return new ActiveState(this);
        }
    }
}
