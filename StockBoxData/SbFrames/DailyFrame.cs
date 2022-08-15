using System;
using StockBox.Data.Adapters.DataFrame;

namespace StockBox.Data.SbFrames
{
    public class DailyFrame :SbFrame 
    {
        public DailyFrame(IDataFrameAdapter adapter): base(adapter, EFrequency.eDaily)
        {
        }
    }
}
