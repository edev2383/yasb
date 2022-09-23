using StockBox.Data.Adapters.DataFrame;


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

        bool IdentifiesAs(BaseIndicator item);
        void Calculate(IDataFrameAdapter frame);
    }
}
