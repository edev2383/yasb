using System;
using StockBox.States.Helpers;

namespace StockBox.Models
{
    /// <summary>
    /// A data class that will model available states
    /// </summary>
    public class StateDataModel
    {
        public int? StateDataModelId { get; set; }
        public string Name { get; set; }
        public EStateType Type { get; set; }

        public StateDataModel(string name)
        {
            Name = name;
        }

        public StateDataModel(StateDataModel source)
        {
            StateDataModelId = source.StateDataModelId;
            Name = source.Name;
            Type = source.Type;
        }

        public StateDataModel(int id, string name, EStateType type)
        {
            StateDataModelId = id;
            Name = name;
            Type = type;
        }

        public StateDataModel(int id, string name)
        {
            StateDataModelId = id;
            Name = name;
        }

        public StateDataModel()
        {
        }

        public StateDataModel Clone()
        {
            return new StateDataModel(this);
        }
    }
}
