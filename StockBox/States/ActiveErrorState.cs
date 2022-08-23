using System;
using StockBox.Models;


namespace StockBox.States
{
    public class ActiveErrorState : StateBase
    {

        public ActiveErrorState(ActiveErrorState source) : base(source) { }

        public ActiveErrorState() : base(
            new StateDataModel
            {
                Name = "ActiveError",
                StateDataModelId = (int)Helpers.EStateType.eActiveError,
                Type = Helpers.EStateType.eActiveError
            })
        { }

        public override StateBase Clone()
        {
            return new ActiveErrorState(this);
        }
    }
}
