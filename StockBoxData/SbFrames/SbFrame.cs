using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.Indicators;
using StockBox.Associations.Enums;
using StockBox.Associations;
using StockBox.Data.SbFrames.Helpers;

namespace StockBox.Data.SbFrames
{

    /// <summary>
    /// The StockBox Frame class. This frame will act as a container around the
    /// actual stock time series data. The adapter will act as a bridge between
    /// this Frame class and the actual Deedle library. This will allow us to
    /// swap data libraries as needed, provided we have the proper adapter
    /// </summary>
    public class SbFrame : ISbFrame
    {

        public ISymbolProvider Symbol { get { return _symbol; } }
        public EFrequency Frequency { get { return _frequency; } }

        /// <summary>
        /// List of Indicators added to the frame for reference. We can
        /// easily access this list for ancillary actions, while the values
        /// are retrieved from the Adapter's._data directly by DateTime key
        /// </summary>
        public IndicatorList Inidcators { get { return _indicators; } }

        /// <summary>
        /// Length of active dataset
        /// </summary>
        public int? Length
        {
            get
            {
                if (_provider == null) return 0;
                return _provider.Length;
            }
        }

        private IDataPointListProvider _provider;
        private readonly EFrequency _frequency;
        private readonly ISymbolProvider _symbol;
        private readonly IndicatorList _indicators = new IndicatorList();

        public SbFrame() { }

        public SbFrame(EFrequency frequency, ISymbolProvider symbol)
        {
            _frequency = frequency;
            _symbol = symbol;
        }

        public SbFrame(IDataPointListProvider provider, EFrequency frequency, ISymbolProvider symbol) : this(frequency, symbol)
        {
            _provider = provider;
            _provider.Parent = this;
        }

        /// <summary>
        /// Return a value (in general should be an int/double) at an index from
        /// zero-origin, based on the provided column value
        /// </summary>
        /// <param name="column"></param>
        /// <param name="indexFromZero"></param>
        /// <returns></returns>
        public object GetTargetValue(DataColumn column, int indexFromZero)
        {
            var dataPoint = GetDataPoint(indexFromZero);
            return dataPoint.GetByColumn(column);
        }

        /// <summary>
        /// return the first row in the data list, i.e., the "current" datapoint
        /// </summary>
        /// <returns></returns>
        public DataPoint FirstDataPoint()
        {
            return GetDataPoint(0);
        }

        /// <summary>
        /// Return a single entry/row from the data, targetd by an index from
        /// the zero-origin.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataPoint GetDataPoint(int index)
        {
            return _provider.GetDataPoint(index);
        }

        /// <summary>
        /// Return an entire SbSeries using a column header key
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public SbSeries ToSeries(string column)
        {
            return _provider.GetFullDataSource().ToSeries(column);
        }

        public void AddIndicator(IIndicator indicator)
        {
            // simple guard in case the indicator already exists
            if (_indicators.ContainsItem(indicator)) return;

            // indicator calculates own values based on the data in the adapter
            // it receives
            indicator.Calculate(_provider);

            // the indicator payload is then mapped to the adapters data
            _provider.MapIndicator(indicator);

            // add the indicator to the frame's list of indicators
            _indicators.Add(indicator);
        }

        public void AddData(DataPointList data)
        {
            _provider.AddData(data);
        }


        public IDataPointListProvider GetProvider()
        {
            return _provider;
        }

        public bool HasIndicator(IIndicator indicator)
        {
            return _indicators.ContainsItem(indicator);
        }

    }
}
