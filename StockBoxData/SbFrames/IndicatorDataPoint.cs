using System;
using System.Collections.Generic;
using System.Linq;

namespace StockBox.Data.SbFrames
{

    /// <summary>
    /// 
    /// </summary>
    public class IndicatorDataPoint
    {

        public string Key { get; set; }
        public object Value { get { return _values.First(); } }
        public List<object> Values { get { return _values; } }
        public List<object> _values = new List<object>();

        public IndicatorDataPoint(string key, object value)
        {
            Key = key;
            _values.Add(value);
        }

        public IndicatorDataPoint(string key, params object[] values)
        {
            Key = key;
            foreach (var item in values)
                _values.Add(item);
        }

        public IndicatorDataPoint(string key, List<object> values)
        {
            Key = key;
            _values = values;
        }


        public IndicatorDataPoint Clone()
        {
            return new IndicatorDataPoint(Key, Values);
        }
    }
}
