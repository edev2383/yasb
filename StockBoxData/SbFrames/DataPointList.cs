using System;
using System.Collections.Generic;
using StockBox.Data.Indicators;


namespace StockBox.Data.SbFrames
{

    /// <summary>
    /// 
    /// </summary>
    public class DataPointList : List<DataPoint>
    {
        public DataPointList Reversed { get { return GetReversed(); } }

        public DataPointList() { }

        public DataPointList(IEnumerable<DataPoint> source)
        {
            foreach (var item in source)
                Add(item.Clone());
        }

        public bool IsDesc
        {
            get
            {
                if (Count <= 1) return true;
                return FirstValue.Date > LastValue.Date;
            }
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

        /// <summary>
        /// Return a targeted DataPoint object by index from zero
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataPoint FindByIndex(int index)
        {
            return FindByDate(GetKeys()[index]);
        }

        /// <summary>
        /// Return a targeted DataPoint object by DateTime key
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataPoint FindByDate(DateTime date)
        {
            DataPoint ret = null;
            foreach (DataPoint item in this)
                if (item.Date == date)
                    ret = item;
            return ret;
        }

        /// <summary>
        /// Create a clone of this object and all contained items
        /// </summary>
        /// <returns></returns>
        public DataPointList Clone()
        {
            var ret = new DataPointList();
            foreach (var item in this)
                ret.Add(item.Clone());
            return ret;
        }

        /// <summary>
        /// Clone and return the whole dataset and return the new object
        ///
        /// Note: Not an in-place reversal
        /// </summary>
        /// <returns></returns>
        public DataPointList GetReversed()
        {
            var retSrc = Clone();
            retSrc.Reverse();
            return new DataPointList(retSrc);
        }

        /// <summary>
        /// Accept a general indicator and map it to the DataPoints by DateTime
        /// key
        /// </summary>
        /// <param name="indicator"></param>
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
                case EIndicatorType.eRSI:
                    MapIndicator((RelativeStrengthIndex)indicator);
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

        private void MapIndicator(RelativeStrengthIndex rsi)
        {
            var payload = (Dictionary<DateTime, double>)rsi.Payload;
            foreach (KeyValuePair<DateTime, double> item in payload)
            {
                var foundDataPoint = FindByDate(item.Key);
                if (foundDataPoint != null)
                    foundDataPoint.AddIndicatorValue(rsi.Name, item.Value);
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
