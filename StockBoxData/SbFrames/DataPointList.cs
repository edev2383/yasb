using System;
using System.Collections.Generic;
using System.Linq;
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

        public DataPoint Last()
        {
            return FindByIndex(Count - 1);
        }

        public DataPoint First()
        {
            if (Count == 0) return null;
            return FindByIndex(0);
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

        public SbSeries ToSeries(string column)
        {
            var ret = new SbSeries(column);
            var dataColumn = DataColumn.ParseColumnDescriptor(column);
            foreach (var item in this)
            {
                var obj = item.GetByColumn(dataColumn);
                if (obj != null)
                    ret.Add(item.Date, (double)obj);
            }

            return ret;
        }

        /// <summary>
        /// Accept a general indicator and map it to the DataPoints by DateTime.
        /// Because the indicator's payload could be different, we'll map each
        /// with their own method for now. Once we have a better idea, we may
        /// be able to combine some executions for simplicity
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
                    MapIndicator((AverageVolume)indicator);
                    break;
                case EIndicatorType.eRSI:
                    MapIndicator((RelativeStrengthIndex)indicator);
                    break;
                case EIndicatorType.eSlowStochastics:
                    MapIndicator((SlowStochastic)indicator);
                    break;
                case EIndicatorType.eFastStochastics:
                    MapIndicator((FastStochastic)indicator);
                    break;
                default:
                    break;
            }
        }

        public SbSeries Window(int frame, Func<DataPointList, double> expression)
        {
            var ret = new SbSeries();
            var clone = Clone();
            for (var idx = 0; idx < Count - frame + 1; idx++)
            {
                var window = new DataPointList(clone.GetRange(idx, frame));
                ret.Add(window.Last().Date, expression(window));
            }
            return ret;
        }

        public double Max(string column)
        {
            return ToSeries(column).Max();
        }

        public double Min(string column)
        {
            return ToSeries(column).Min();
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

        private void MapIndicator(AverageVolume sma)
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

        private void MapIndicator(SlowStochastic slowsto)
        {
            var payload = (Dictionary<DateTime, (double k, double d)>)slowsto.Payload;
            foreach (KeyValuePair<DateTime, (double k, double d)> item in payload)
            {
                var foundDataPoint = FindByDate(item.Key);
                if (foundDataPoint != null)
                {
                    var indicatorDataPoint = new IndicatorDataPoint(slowsto.Name, item.Value.k, item.Value.d);
                    foundDataPoint.AddIndicatorValue(indicatorDataPoint);
                }
            }
        }

        private void MapIndicator(FastStochastic faststo)
        {
            var payload = (Dictionary<DateTime, (double k, double d)>)faststo.Payload;
            foreach (KeyValuePair<DateTime, (double k, double d)> item in payload)
            {
                var foundDataPoint = FindByDate(item.Key);
                if (foundDataPoint != null)
                {
                    var indicatorDataPoint = new IndicatorDataPoint(faststo.Name, item.Value.k, item.Value.d);
                    foundDataPoint.AddIndicatorValue(indicatorDataPoint);
                }
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
