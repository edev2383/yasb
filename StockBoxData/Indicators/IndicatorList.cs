using System;
using System.Collections.Generic;


namespace StockBox.Data.Indicators
{

    /// <summary>
    /// Class <c>IndicatorList</c> is a class wrap around IIndicators. It's
    /// main purpose is as a cache within the SbFrame
    /// </summary>
    public class IndicatorList : List<IIndicator>
    {
        public IndicatorList()
        {
        }

        public bool ContainsItem(IIndicator item)
        {
            foreach (var ind in this)
                if (ind.IdentifiesAs(item))
                    return true;
            return false;
        }
    }
}
