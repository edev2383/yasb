using System;
using StockBox.Models;


namespace StockBox.States
{
    public class UserDefinedState : StateBase
    {

        public UserDefinedState(string name) : base(
            new StateDataModel
            {
                Name = name,
                StateDataModelId = null,
                Type = Helpers.EStateType.eUserDefined
            })
        { }

        public UserDefinedState(StateDataModel state) : base(state)
        {
        }

    }
}
