using System;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Associations.Enums;


namespace StockBox.Data.SbFrames
{

    public class MonthlyFrame : SbFrame
    {
        public MonthlyFrame(IDataFrameAdapter adapter) : base(adapter, EFrequency.eMonthly)
        {
        }
    }
}
