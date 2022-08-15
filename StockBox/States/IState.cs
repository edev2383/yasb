using System;
namespace StockBox.States
{
    public interface IState
    {

        object Action();
    }
}
