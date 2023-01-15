using StockBox.Data.SbFrames.Helpers;


namespace StockBox.Data.Indicators
{

    /// <summary>
    /// 
    /// </summary>
    public interface IIndicator
    {
        EIndicatorType Type { get; }
        string Name { get; }
        string ColumnKey { get; }
        int[] Indices { get; }
        object Payload { get; }

        bool IdentifiesAs(IIndicator item);
        void Calculate(IDataPointListProvider frame);
    }
}
