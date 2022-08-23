using System;
using System.Collections.Generic;
using StockBox.Associations.Enums;

namespace StockBox.Data.SbFrames
{
    public class SbFrameList : List<SbFrame>
    {
        public SbFrameList()
        {
        }

        public SbFrame FindByFrequency(EFrequency frequency)
        {
            SbFrame ret = null;
            foreach (var frame in this)
            {
                if (frame.Frequency == frequency)
                    ret = frame;
            }
            return ret;
        }
    }
}
