using System;
using System.Collections.Generic;

namespace StockBox.Data.SbFrames
{
    public class IndicatorDataPointList : List<IndicatorDataPoint>
    {
        public IndicatorDataPointList()
        {
        }

        public IndicatorDataPoint FindByKey(string key)
        {
            IndicatorDataPoint ret = null;

            foreach (var item in this)
                if (item.Key.ToLower() == key.ToLower())
                    ret = item;

            return ret;
        }

        public IndicatorDataPointList Clone()
        {
            var ret = new IndicatorDataPointList();
            foreach (var item in this)
                ret.Add(item.Clone());
            return ret;
        }
    }
}
