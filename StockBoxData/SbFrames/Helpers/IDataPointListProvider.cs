using StockBox.Data.Indicators;


namespace StockBox.Data.SbFrames.Helpers
{

    /// <summary>
    /// DataFrame interface. Primary data handling by Deedle, however, should
    /// that change, any additional adapters can be substituted using this
    /// interface.
    /// </summary>
    public interface IDataPointListProvider
    {
        public int? Length { get; }
        SbFrame Parent { get; set; }

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
        /// Map an Indicator's payload into the DataPointList
        /// </summary>
        /// <param name="indicator"></param>
        void MapIndicator(IIndicator indicator);

        /// <summary>
        /// Create a new IDataFrameAdapter object, without preexisting data
        /// </summary>
        /// <returns></returns>
        IDataPointListProvider Create();

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

