using StockBox.Data.SbFrames;
using Deedle;
using System;
using StockBox.Data.Indicators;
using System.IO;

namespace StockBox.Data.Adapters.DataFrame
{
    /// <summary>
    /// DataFrame interface. Primary data handling by Deedle, however, should
    /// that change, any additional adapters can be substituted using this
    /// interface.
    /// </summary>
    public interface IDataFrameAdapter
    {
        public int? Length { get; }
        SbFrame Parent { get; set; }

        /// <summary>
        /// Add data from a MemoryStream after the object has been created.
        /// Assumes CSV format. We can add adapters for other formats if needed,
        /// i.e., JSON
        /// </summary>
        /// <param name="data"></param>
        void AddData(MemoryStream data);

        /// <summary>
        /// Add data to the adapter from a source DataPointList
        /// </summary>
        /// <param name="data"></param>
        void AddData(DataPointList data);

        /// <summary>
        /// Return a DataPoint object by indices from the first value
        /// </summary>
        /// <param name="indexFromZero"></param>
        /// <returns></returns>
        DataPoint GetDataPoint(int indexFromZero);

        /// <summary>
        /// Return an entire series of data from one Column
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        ColumnSeries GetColumnData(DataColumn column);

        /// <summary>
        /// Map an Indicator's payload into the DataPointList
        /// </summary>
        /// <param name="indicator"></param>
        void MapIndicator(IIndicator indicator);

        /// <summary>
        /// Create a new IDataFrameAdapter object, without preexisting data
        /// </summary>
        /// <returns></returns>
        IDataFrameAdapter Create();

        /// <summary>
        /// Return an SbSeries for a specific column header
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        SbSeries GetSeries(string column);

        /// <summary>
        /// Primary method for getting the current DataPointList. Use this when
        /// doing general rule parsing, as this takes into account the current
        /// 'window' for backtesting.
        /// </summary>
        /// <returns></returns>
        DataPointList GetData();

        /// <summary>
        /// Return the DataPointList source, ignoring the current 'window'. Use
        /// this when attempting to do calculations for indicators. 
        /// </summary>
        /// <returns></returns>
        DataPointList GetFullDataSource();

        /// <summary>
        /// Return true if an indicator is present in the SbFrame parent. 
        /// </summary>
        /// <param name="indicator"></param>
        /// <returns></returns>
        bool IndicatorExists(IIndicator indicator);
    }
}
