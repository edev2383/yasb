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
        Frame<DateTime, string> SourceData { get; }
        public int? Length { get; }
        SbFrame Parent { get; set; }
        /// <summary>  
        /// Return a single datapoint
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>

        void AddData(MemoryStream data);
        void AddData(DataPointList data);
        DataPoint GetDataPoint(int indexFromZero);
        ColumnSeries GetColumnData(DataColumn column);
        void MapIndicator(IIndicator indicator);
        IDataFrameAdapter Create();
        SbSeries GetSeries(string column);
        DataPointList GetData();
        DataPointList GetFullDataSource();
        bool IndicatorExists(IIndicator indicator);
    }
}
