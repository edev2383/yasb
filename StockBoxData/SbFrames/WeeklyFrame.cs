using System;
using StockBox.Data.Adapters.DataFrame;

namespace StockBox.Data.SbFrames
{
    public class WeeklyFrame : SbFrame
    {
        public WeeklyFrame(IDataFrameAdapter adapter) : base(adapter, EFrequency.eWeekly)
        {
        }
    }
}
