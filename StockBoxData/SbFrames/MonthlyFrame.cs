using StockBox.Associations.Enums;
using StockBox.Associations;
using StockBox.Data.SbFrames.Helpers;


namespace StockBox.Data.SbFrames
{

    /// <summary>
    /// 
    /// </summary>
    public class MonthlyFrame : SbFrame
    {
        public MonthlyFrame(IDataPointListProvider provider, ISymbolProvider symbol) : base(provider, EFrequency.eMonthly, symbol)
        {
        }
    }
}
