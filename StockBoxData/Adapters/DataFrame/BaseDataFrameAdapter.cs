using System;
using System.IO;
using Deedle;
using StockBox.Data.SbFrames;
using System.Linq;
using StockBox.Data.Indicators;
using StockBox.Validation;

namespace StockBox.Data.Adapters.DataFrame
{

    /// <summary>
    /// Class <c>BaseDataFrameAdapter</c> accepts a dataset, currently only a
    /// MemoryStream, but that may expand to other types of data, using Deedle
    /// to read the stream as a CSV and then map that data to a DataPointList
    /// </summary>
    public abstract class BaseDataFrameAdapter : IDataFrameAdapter
    {

        protected DataPointList _data;
        public SbFrame Parent { get; set; }

        public int? Length
        {
            get
            {
                if (_data == null) return 0;
                return _data.Count;
            }
        }

        /// <summary>
        /// The first DateTime value in the adapter's data list
        /// </summary>
        public DateTime FirstKey { get { return _data.First().Date; } }

        public BaseDataFrameAdapter() { }

        public BaseDataFrameAdapter(MemoryStream data)
        {
            AddData(data);
        }

        /// <summary>
        /// Return the DataPointList source, ignoring the current 'window'. Use
        /// this when attempting to do calculations for indicators. 
        /// </summary>
        /// <returns></returns>
        public DataPointList GetFullDataSource()
        {
            return _data;
        }

        /// <summary>
        /// Primary method for getting the current DataPointList. Use this when
        /// doing general rule parsing, as this takes into account the current
        /// 'window' for backtesting.
        /// </summary>
        /// <returns></returns>
        public abstract DataPointList GetData();

        /// <summary>
        /// Add data from a MemoryStream after the object has been created.
        /// Assumes CSV format. We can add adapters for other formats if needed,
        /// i.e., JSON
        /// </summary>
        /// <param name="data"></param>
        public virtual void AddData(MemoryStream data)
        {
            // read the incoming stream as a CSV
            var rawData = Frame.ReadCsv(data);

            // using the Deedle library functions, index and sort by date
            var tmpSorted = rawData.IndexRows<DateTime>("Date").SortRowsByKey();

            // map the deedle data structure to the domain DataPointList
            _data = Map(tmpSorted);

            // since our data is time-series, we always want the data in desc
            // order, with the most recent date key at 0-idx.
            if (!_data.IsDesc)
                _data = _data.Reversed;
        }

        /// <summary>
        /// Add data to the adapter from a source DataPointList
        /// </summary>
        /// <param name="data"></param>
        public virtual void AddData(DataPointList data)
        {
            _data = data;
        }

        /// <summary>
        /// Return a DataPoint object by indices from the first value
        /// </summary>
        /// <param name="indexFromZero"></param>
        /// <returns></returns>
        public DataPoint GetDataPoint(int indexFromZero)
        {
            return GetData().FindByIndex(indexFromZero);
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

        /// <summary>
        /// Map an Indicator's payload into the DataPointList
        /// </summary>
        /// <param name="indicator"></param>
        public void MapIndicator(IIndicator indicator)
        {
            if (_data != null)
                _data.MapIndicator(indicator);
        }

        /// <summary>
        /// Map a Deedle frame into a StockBox.DataPointList object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private DataPointList Map(Frame<DateTime, string> data)
        {
            // fill missing in with a placeholder value. We can do some
            // post-processing to fill in missing values w/ averages
            data = data.FillMissing(-1);
            DataPointList ret = new DataPointList();
            for (var idx = 0; idx < data.RowCount; idx++)
            {
                var key = data.RowKeys.ToArray()[idx];
                var curr = data.Rows[key];
                var vr = ValidateObjectSeries(curr);
                // ignore any series that are missing data, the vr contains the
                // series as a ValidationObject so we could further examine for
                // additional data
                if (vr.Success)
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
                Volume = obj.GetAs<double>("Volume")
            };
        }

        protected ValidationResultList ValidateObjectSeries(ObjectSeries<string> obj)
        {
            var ret = new ValidationResultList();
            ret.Add(new ValidationResult(obj.ValueCount > 1, "ObjectSeries has more than one value"));
            if (ret.HasFailures)
                return ret;
            ret.Add(new ValidationResult(obj.TryGet("High").Value.ToString() != "null", "ObjectSeries(`High`) is not null"));
            ret.Add(new ValidationResult(obj.TryGet("Low").Value.ToString() != "null", "ObjectSeries(`Low`) is not null"));
            ret.Add(new ValidationResult(obj.TryGet("Open").Value.ToString() != "null", "ObjectSeries(`Open`) is not null"));
            ret.Add(new ValidationResult(obj.TryGet("Close").Value.ToString() != "null", "ObjectSeries(`Close`) is not null"));
            ret.Add(new ValidationResult(obj.TryGet("Adj Close").Value.ToString() != "null", "ObjectSeries(`Adj Close`) is not null"));
            ret.Add(new ValidationResult(obj.TryGet("Volume").Value.ToString() != "null", "ObjectSeries(`Volume`) is not null"));
            if (ret.HasFailures)
                ret.Add(new ValidationResult(EResult.eFail, "ObjectSeries is missing data", obj));
            return ret;
        }

        /// <summary>
        /// Create a new IDataFrameAdapter object, without preexisting data
        /// </summary>
        /// <returns></returns>
        public abstract IDataFrameAdapter Create();

        /// <summary>
        /// Return an SbSeries for a specific column header
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public SbSeries GetSeries(string column)
        {
            return GetData().ToSeries(column);
        }

        /// <summary>
        /// Return true if an indicator is present in the SbFrame parent. 
        /// </summary>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool IndicatorExists(IIndicator indicator)
        {
            return Parent.HasIndicator(indicator);
        }
    }
}
