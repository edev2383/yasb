using System;
using StockBox.Data.Indicators;
using StockBox.Data.SbFrames.Helpers;

namespace StockBox.Data.SbFrames.Providers
{

    /// <summary>
    /// Class <c>BaseDataSource</c> is the base DataPointListProvider
    /// that holds the data and points back to the parent. 
    /// </summary>
    public abstract class BaseDataProvider : IDataPointListProvider
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

        /// <summary>
        /// The last DateTime value in the adapter's data list
        /// </summary>
        public DateTime LastKey { get { return _data.Last().Date; } }

        public BaseDataProvider() { }

        public BaseDataProvider(DataPointList data)
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
        /// Create a new IDataFrameAdapter object, without preexisting data
        /// </summary>
        /// <returns></returns>
        public abstract IDataPointListProvider Create();

        /// <summary>
        /// Return an SbSeries for a specific column header
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public SbSeries GetSeries(string column)
        {
            // use GetData() as the primary data payload retreiver method
            // to allow the Provider to do what ever it needs to munge the
            // data to suit its needs.
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

