using System;
using System.Collections.Generic;
namespace StockBox.Data.Indicators
{
    public class IndicatorList : List<IIndicator>
    {
        public IndicatorList()
        {
        }

        public bool ContainsItem(BaseIndicator item)
        {
            foreach (var ind in this)
                if (ind.IdentifiesAs(item))
                    return true;
            return false;
        }
    }
}
