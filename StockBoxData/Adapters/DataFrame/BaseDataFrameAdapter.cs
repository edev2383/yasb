using System;
using System.IO;
using Deedle;
using StockBox.Data.SbFrames;
using System.Linq;
using StockBox.Data.Indicators;

namespace StockBox.Data.Adapters.DataFrame
{
    public abstract class BaseDataFrameAdapter : IDataFrameAdapter
    {

        protected DataPointList _data;

        /// <summary>
        /// The original dataset Frame created using Deedle Library. Keeping
        /// this in the Adapter allows us to use the built-in window functions
        /// to do our Indicator calculations. If we want to add window functions
        /// down the road, we can potentially remove this, but so far we're able
        /// to keep the Deedle reference to the StockBoxData library project
        /// </summary>
        public Frame<DateTime, string> SourceData { get { return _sourceData; } }
        protected Frame<DateTime, string> _sourceData;

        /// <summary>
        /// The first DateTime value in the adapter's data list
        /// </summary>
        public DateTime FirstKey { get { return _data.First().Date; } }

        public BaseDataFrameAdapter() { }

        public BaseDataFrameAdapter(MemoryStream data)
        {
            AddData(data);
        }

        public void AddData(MemoryStream data)
        {
            var rawData = Frame.ReadCsv(data);
            _sourceData = rawData.IndexRows<DateTime>("Date").SortRowsByKey();
            _data = Map(_sourceData);
        }

        /// <summary>
        /// Return a DataPoint object by indices from the first value
        /// </summary>
        /// <param name="indexFromZero"></param>
        /// <returns></returns>
        public DataPoint GetDataPoint(int indexFromZero)
        {
            return _data.FindByIndex(indexFromZero);
        }

        /// <summary>
        /// Return an entire series of data from one Column
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public ColumnSeries GetColumnData(DataColumn column)
        {
            ColumnSeries ret = new ColumnSeries(column.Column);
            foreach (DataPoint item in _data)
                ret.Add(item.Date, item.GetByColumn(column));
            return ret;
        }

        public void MapIndicator(IIndicator indicator)
        {
            _data.MapIndicator(indicator);
        }

        /// <summary>
        /// Map a Deedle frame into a StockBox.DataPointList object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private DataPointList Map(Frame<DateTime, string> data)
        {
            DataPointList ret = new DataPointList();
            for (var idx = 0; idx < data.RowCount; idx++)
            {
                var key = data.RowKeys.ToArray()[idx];
                var curr = data.Rows[key];
                ret.Add(MapDeedleObjectSeries(key, curr));
            }
            return ret;
        }

        /// <summary>
        /// Map a single row within a Deedle.Frame to a DataPoint object
        /// </summary>
        /// <param name="dateIndex"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private DataPoint MapDeedleObjectSeries(DateTime dateIndex, ObjectSeries<string> obj)
        {
            return new DataPoint
            {
                Date = dateIndex,
                High = obj.GetAs<double>("High"),
                Low = obj.GetAs<double>("Low"),
                Open = obj.GetAs<double>("Open"),
                Close = obj.GetAs<double>("Close"),
                AdjClose = obj.GetAs<double>("Adj Close"),
                Volume = obj.GetAs<int>("Volume")
            };
        }
    }
}
