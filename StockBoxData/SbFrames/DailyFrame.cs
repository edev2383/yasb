using System;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Associations.Enums;
using StockBox.Associations;

namespace StockBox.Data.SbFrames
{

    public class DailyFrame : SbFrame
    {
        public DailyFrame(IDataFrameAdapter adapter, ISymbolProvider symbol) : base(adapter, EFrequency.eDaily, symbol)
        {
        }
    }
}
