using System;
using System.Collections.Generic;
using StockBox.Data.Indicators;

namespace StockBox.Data.SbFrames
{
    public class DataPointList : List<DataPoint>
    {
        public DataPointList Reversed { get { return GetReversed(); } }

        public DataPointList() { }

        public DataPointList(IEnumerable<DataPoint> source)
        {
            foreach (var item in source)
                Add(item.Clone());
        }

        public DataPoint LastValue
        {
            get
            {
                if (Count == 0) return new DataPoint();
                return this[^1];
            }
        }

        public DataPoint FirstValue
        {
            get
            {
                if (Count == 0) return new DataPoint();
                return this[0];
            }
        }

        public DataPoint FindByIndex(int index)
        {
            return FindByDate(GetKeys()[index]);
        }

        public DataPoint FindByDate(DateTime date)
        {
            DataPoint ret = null;
            foreach (DataPoint item in this)
                if (item.Date == date)
                    ret = item;
            return ret;
        }

        public DataPointList Clone()
        {
            var ret = new DataPointList();
            foreach (var item in this)
                ret.Add(item.Clone());
            return ret;
        }

        public DataPointList GetReversed()
        {
            var retSrc = Clone();
            retSrc.Reverse();
            return new DataPointList(retSrc);
        }

        public void MapIndicator(IIndicator indicator)
        {
            switch (indicator.Type)
            {
                case EIndicatorType.eSma:
                    MapIndicator((SimpleMovingAverage)indicator);
                    break;
                case EIndicatorType.eVolume:
                    MapIndicator((Volume)indicator);
                    break;
                default:
                    break;
            }
        }

        private void MapIndicator(SimpleMovingAverage sma)
        {
            var payload = (Dictionary<DateTime, double>)sma.Payload;
            foreach (KeyValuePair<DateTime, double> item in payload)
            {
                var foundDataPoint = FindByDate(item.Key);
                if (foundDataPoint != null)
                    foundDataPoint.AddIndicatorValue(sma.Name, item.Value);
            }
        }

        private void MapIndicator(Volume sma)
        {
            var payload = (Dictionary<DateTime, double>)sma.Payload;
            foreach (KeyValuePair<DateTime, double> item in payload)
            {
                var foundDataPoint = FindByDate(item.Key);
                if (foundDataPoint != null)
                    foundDataPoint.AddIndicatorValue(sma.Name, item.Value);
            }
        }

        private List<DateTime> GetKeys()
        {
            var ret = new List<DateTime>();
            foreach (var item in this)
                ret.Add(item.Date);
            return ret;
        }
    }
}
