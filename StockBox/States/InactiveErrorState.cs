using System;
using StockBox.Models;


namespace StockBox.States
{
    public class InactiveErrorState : StateBase
    {

        public InactiveErrorState(InactiveErrorState source) : base(source) { }

        public InactiveErrorState() : base(
            new StateDataModel
            {
                Name = "InactiveError",
                StateDataModelId = (int)Helpers.EStateType.eInactiveError,
                Type = Helpers.EStateType.eInactiveError
            })
        { }

        public override StateBase Clone()
        {
            return new InactiveErrorState(this);
        }
    }
}
