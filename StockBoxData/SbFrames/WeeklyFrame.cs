using System;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Associations.Enums;


namespace StockBox.Data.SbFrames
{

    public class WeeklyFrame : SbFrame
    {
        public WeeklyFrame(IDataFrameAdapter adapter) : base(adapter, EFrequency.eWeekly)
        {
        }
    }
}
