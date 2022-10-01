using System.Collections.Generic;
using StockBox.Associations;
using StockBox.Associations.Enums;
using StockBox.Associations.Tokens;

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

        public SbFrameList FindAllBySymbolProvider(ISymbolProvider symbol)
        {
            var ret = new SbFrameList();
            foreach (SbFrame item in this)
                if (item.Symbol.Equals(symbol))
                    ret.Add(item);
            return ret;
        }

        public void Normalize()
        {

        }
    }
}
