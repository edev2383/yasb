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

        /// <summary>
        /// Return the last DataPoint in the list
        /// </summary>
        /// <returns></returns>
        public DataPoint Last()
        {
            return FindByIndex(Count - 1);
        }

        /// <summary>
        /// Return the first DataPoint in the list
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Return an SbSeries (Dict[DateTime, double]) of a single column
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
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
        /// with their own method for now. Once we have a better idea of the
        /// shape of indicator payload data, we may be able to combine some
        /// executions for the sake of simplicity
        /// </summary>
        /// <param name="indicator"></param>
        public void MapIndicator(IIndicator indicator)
        {
            switch (indicator.Type)
            {
                case EIndicatorType.sma:
                    MapIndicator((SimpleMovingAverage)indicator);
                    break;
                case EIndicatorType.volume:
                    MapIndicator((AverageVolume)indicator);
                    break;
                case EIndicatorType.rsi:
                    MapIndicator((RelativeStrengthIndex)indicator);
                    break;
                case EIndicatorType.slowStochastics:
                    MapIndicator((SlowStochastic)indicator);
                    break;
                case EIndicatorType.fastStochastics:
                    MapIndicator((FastStochastic)indicator);
                    break;
                case EIndicatorType.slope:
                    MapIndicator((Slope)indicator);
                    break;
                case EIndicatorType.averageTrueRange:
                    MapIndicator((AverageTrueRange)indicator);
                    break;
                case EIndicatorType.priceChannel:
                    MapIndicator((PriceChannel)indicator);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Apply an [expression] across a window [frame] of data, returning an
        /// SbSeries object. The frames aggregate toward the final record, so
        /// the key for the new SbSeries will be the DateTime of the last data
        /// record.
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Return the highest [value] in a given column
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public double Max(string column)
        {
            return ToSeries(column).Max();
        }

        /// <summary>
        /// Return the lower [value] in a given column
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public double Min(string column)
        {
            return ToSeries(column).Min();
        }

        private void MapIndicator(SimpleMovingAverage sma)
        {
            foreach (KeyValuePair<DateTime, double> item in sma.Payload)
            {
                var foundDataPoint = FindByDate(item.Key);
                if (foundDataPoint != null)
                    foundDataPoint.AddIndicatorValue(sma.Name, item.Value);
            }
        }

        private void MapIndicator(Slope slope)
        {
            foreach (KeyValuePair<DateTime, double> item in slope.Payload)
            {
                var foundDataPoint = FindByDate(item.Key);
                if (foundDataPoint != null)
                    foundDataPoint.AddIndicatorValue(slope.Name, item.Value);
            }
        }

        private void MapIndicator(AverageVolume averageVolume)
        {
            foreach (KeyValuePair<DateTime, double> item in averageVolume.Payload)
            {
                var foundDataPoint = FindByDate(item.Key);
                if (foundDataPoint != null)
                    foundDataPoint.AddIndicatorValue(averageVolume.Name, item.Value);
            }
        }

        private void MapIndicator(RelativeStrengthIndex rsi)
        {
            foreach (KeyValuePair<DateTime, double> item in rsi.Payload)
            {
                var foundDataPoint = FindByDate(item.Key);
                if (foundDataPoint != null)
                    foundDataPoint.AddIndicatorValue(rsi.Name, item.Value);
            }
        }

        private void MapIndicator(SlowStochastic slowsto)
        {
            foreach (KeyValuePair<DateTime, (double k, double d)> item in slowsto.Payload)
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
            foreach (KeyValuePair<DateTime, (double k, double d)> item in faststo.Payload)
            {
                var foundDataPoint = FindByDate(item.Key);
                if (foundDataPoint != null)
                {
                    var indicatorDataPoint = new IndicatorDataPoint(faststo.Name, item.Value.k, item.Value.d);
                    foundDataPoint.AddIndicatorValue(indicatorDataPoint);
                }
            }
        }

        private void MapIndicator(AverageTrueRange atr)
        {
            foreach (KeyValuePair<DateTime, double> item in atr.Payload)
            {
                var foundDataPoint = FindByDate(item.Key);
                if (foundDataPoint != null)
                    foundDataPoint.AddIndicatorValue(atr.Name, item.Value);
            }
        }

        private void MapIndicator(PriceChannel pc)
        {
            foreach (KeyValuePair<DateTime, (double high, double center, double low)> item in pc.Payload)
            {
                var foundDataPoint = FindByDate(item.Key);
                if (foundDataPoint != null)
                {
                    var indicatorDataPoint = new IndicatorDataPoint(pc.Name, item.Value.high, item.Value.center, item.Value.low);
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
