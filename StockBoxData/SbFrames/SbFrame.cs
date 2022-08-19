using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.Indicators;


namespace StockBox.Data.SbFrames
{
    /// <summary>
    /// The StockBox Frame class. This frame will act as a controller for the
    /// actual stock time series data. The adapter will act as a bridge between
    /// this Frame class and the actual Deedle library. This will allow us to
    /// swap data libraries as needed, provided we have the proper adapter
    /// </summary>
    public class SbFrame
    {
        public EFrequency Frequency { get { return _frequency; } }

        /// <summary>
        /// List of Indicators added to the frame for reference. We can
        /// easily access this list for ancillary actions, while the values
        /// are retrieved from the Adapter's._data directly by DateTime key
        /// </summary>
        public IndicatorList Inidcators { get { return _indicators; } }

        private IDataFrameAdapter _adapter;
        private readonly EFrequency _frequency;
        private readonly IndicatorList _indicators = new IndicatorList();

        public SbFrame() { }

        public SbFrame(EFrequency frequency)
        {
            _frequency = frequency;
        }

        public SbFrame(IDataFrameAdapter adapter, EFrequency frequency) : this(frequency)
        {
            _adapter = adapter;
        }

        public object GetTargetValue(DataColumn column, int indexFromZero)
        {
            var dataPoint = GetDataPoint(indexFromZero);
            return dataPoint.GetByColumn(column);
        }

        public DataPoint FirstDataPoint()
        {
            return GetDataPoint(0);
        }

        public DataPoint GetDataPoint(int index)
        {
            return _adapter.GetDataPoint(index);
        }

        public ColumnSeries GetColumnData(string column)
        {
            return _adapter.GetColumnData(new DataColumn(column));
        }

        public ColumnSeries GetColumnData(DataColumn column)
        {
            return _adapter.GetColumnData(column);
        }

        public void AddIndicator(IIndicator indicator)
        {
            indicator.Calculate(this._adapter);
            _adapter.MapIndicator(indicator);
            _indicators.Add(indicator);
        }
    }
}
