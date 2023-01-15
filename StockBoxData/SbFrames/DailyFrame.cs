using StockBox.Associations.Enums;
using StockBox.Associations;
using StockBox.Data.SbFrames.Helpers;


namespace StockBox.Data.SbFrames
{

    /// <summary>
    /// Class <c>DailyFrame</c>
    /// </summary>
    public class DailyFrame : SbFrame
    {
        public DailyFrame(IDataPointListProvider provider, ISymbolProvider symbol) : base(provider, EFrequency.eDaily, symbol)
        {
        }
    }
}
