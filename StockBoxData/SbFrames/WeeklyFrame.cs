using StockBox.Associations.Enums;
using StockBox.Associations;
using StockBox.Data.SbFrames.Helpers;


namespace StockBox.Data.SbFrames
{

    /// <summary>
    /// 
    /// </summary>
    public class WeeklyFrame : SbFrame
    {
        public WeeklyFrame(IDataPointListProvider provider, ISymbolProvider symbol) : base(provider, EFrequency.eWeekly, symbol)
        {
        }
    }
}
