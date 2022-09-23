using System.Collections.Generic;
using StockBox.Associations.Enums;


namespace StockBox.Data.SbFrames
{

    /// <summary>
    /// 
    /// </summary>
    public class SbFrameList : List<SbFrame>
    {

        public SbFrameList()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frequency"></param>
        /// <returns></returns>
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
