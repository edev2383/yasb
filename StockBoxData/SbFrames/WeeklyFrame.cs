using System;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Associations.Enums;
using StockBox.Associations;

namespace StockBox.Data.SbFrames
{

    public class WeeklyFrame : SbFrame
    {
        public WeeklyFrame(IDataFrameAdapter adapter, ISymbolProvider symbol) : base(adapter, EFrequency.eWeekly, symbol)
        {
        }
    }
}
