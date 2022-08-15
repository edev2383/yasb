using System;
namespace StockBox.Data.SbFrames
{
    public class IndicatorDataPoint
    {

        public string Key { get; set; }
        public object Value { get; set; }

        public IndicatorDataPoint(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public IndicatorDataPoint Clone()
        {
            return new IndicatorDataPoint(Key, Value);
        }
    }
}
