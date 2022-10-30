using System.Collections.Generic;
using StockBox.Associations;
using StockBox.Associations.Enums;

namespace StockBox.Data.SbFrames
{

    /// <summary>
    /// 
    /// </summary>
    public class SbFrameList : List<ISbFrame>
    {

        public DataPoint Current
        {
            get
            {
                var daily = FindByFrequency(EFrequency.eWeekly);
                if (daily == null) return new DataPoint();
                return daily.FirstDataPoint();
            }
        }

        public double AllTimeHigh
        {
            get
            {
                return 0;
            }
        }

        public double AllTimeLow
        {
            get
            {
                return 0;
            }
        }

        public double FiftyTwoWeekHigh
        {
            get
            {
                return 0;
            }
        }

        public double FiftyTwoWeekLow
        {
            get
            {
                return 0;
            }
        }


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
                    ret = (SbFrame)frame;
            }
            return ret;
        }

        public SbFrameList FindAllBySymbolProvider(ISymbolProvider symbol)
        {
            var ret = new SbFrameList();
            foreach (SbFrame item in this)
                if (item.Symbol.Equals(symbol))
                    ret.Add(item);
            return ret;
        }

        /// <summary>
        /// Redraw the Weekly and Monthly windows based on the iterations of the
        /// Daily. 
        /// </summary>
        public void Normalize()
        {

        }
    }
}
