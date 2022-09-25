using System;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Associations.Enums;
using StockBox.Associations;

namespace StockBox.Data.SbFrames
{

    public class MonthlyFrame : SbFrame
    {
        public MonthlyFrame(IDataFrameAdapter adapter, ISymbolProvider symbol) : base(adapter, EFrequency.eMonthly, symbol)
        {
        }
    }
}
