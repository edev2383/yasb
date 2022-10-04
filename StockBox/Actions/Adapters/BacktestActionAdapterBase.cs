using System;


namespace StockBox.Actions.Adapters
{

    /// <summary>
    /// Class <c>BacktestActionAdapterBase</c> creates a shared base starting
    /// point for any actions to be performed during Backtesting.
    ///
    /// Backtesting action adapters have their own classes because Backtesting
    /// is an actual feature of the application, so they act kind of like in-app
    /// mocks.
    /// </summary>
    public abstract class BacktestActionAdapterBase : SbActionAdapterBase
    {
        public BacktestActionAdapterBase() : base()
        {
        }
    }
}
