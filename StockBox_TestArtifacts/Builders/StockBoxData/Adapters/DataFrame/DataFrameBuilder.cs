using System;
using System.Collections.Generic;
using StockBox.Data.SbFrames;
using StockBox.Data.SbFrames.Helpers;
using StockBox_TestArtifacts.Mocks.StockBoxData.SbFrames;


namespace StockBox_TestArtifacts.Builders.StockBoxData.Adapters.DataFrame
{

    /// <summary>
    /// 
    /// </summary>
    public class DataProviderBuilder
    {

        private List<MockDataPoint> _dataPoints = new List<MockDataPoint>();

        public DataProviderBuilder()
        {
        }

        public DataProviderBuilder WithCloseAndIndicator(DateTime? date = null, int? close = null, IndicatorDataPoint indicator = null)
        {
            _dataPoints.Add(new MockDataPoint(date, null, null, null, close, null, null, new List<IndicatorDataPoint>() { indicator }));
            return this;
        }

        public DataProviderBuilder WithData(DateTime? date = null, double? high = null, double? low = null, double? open = null, double? close = null, double? adjClose = null, double? volume = null, List<IndicatorDataPoint> indicators = null)
        {
            _dataPoints.Add(new MockDataPoint(date, high, low, open, close, adjClose, volume, indicators));
            return this;
        }

        public DataProviderBuilder WithData(DateTime? date = null, double? high = null, double? low = null, double? open = null, double? close = null, double? adjClose = null, double? volume = null, string indicatorKey = null, params object[] values)
        {
            var indicator = new IndicatorDataPoint(indicatorKey, values);
            _dataPoints.Add(new MockDataPoint(date, high, low, open, close, adjClose, volume, new List<IndicatorDataPoint>() { indicator }));
            return this;
        }

        public DataProviderBuilder WithData(DateTime? date = null, IndicatorDataPoint indicator = null)
        {
            _dataPoints.Add(new MockDataPoint(date, 0, 0, 0, 0, 0, 0, new List<IndicatorDataPoint>() { indicator }));
            return this;
        }

        public DataProviderBuilder WithData(DateTime? date = null, string key = null, params object[] values)
        {
            return WithData(date, indicator: new IndicatorDataPoint(key, values));
        }

        public T Build<T>() where T : IDataPointListProvider, new()
        {
            var ret = new T();
            ret.AddData(CreateDataPoints());
            return ret;
        }

        public DataPointList CreateDataPoints()
        {
            var ret = new DataPointList();
            foreach (var dp in _dataPoints)
            {
                DateTime? date = dp.Date;
                if (dp.Date == null)
                    date = DateTime.Now.Date.AddDays((-ret.Count));

                var dataPoint = new DataPoint()
                {
                    Date = (DateTime)date,
                    High = dp.High,
                    Low = dp.Low,
                    Open = dp.Open,
                    Close = dp.Close,
                    AdjClose = dp.AdjClose,
                    Volume = dp.Volume,
                };

                dataPoint.AddIndicatorValues(dp.Indicators);

                ret.Add(dataPoint);
            }

            return ret;
        }
    }
}
