using StockBox.Data.SbFrames.Helpers;


namespace StockBox.Data.SbFrames.Providers
{

    /// <summary>
    /// 
    /// </summary>
    public class BackwardTestingDataProvider : BaseDataProvider
    {
        public BackwardTestingDataProvider() { }

        public BackwardTestingDataProvider(DataPointList source) : base(source)
        {
        }

        public override IDataPointListProvider Create()
        {
            return new BackwardTestingDataProvider();
        }

        private int? _windowIndex;

        /// <summary>
        /// Decrease the _windowIndex integer value by 1, increasing the actual
        /// window of data by 1.
        /// </summary>
        public void IterateWindow()
        {
            if (_data == null)
                throw new System.Exception("No data found");
            if (_windowIndex == null)
                _windowIndex = Length - 1;
            if (!IsAtEnd())
                _windowIndex--;
        }

        /// <summary>
        /// Return true if the _windowIndex value has reached 0
        /// </summary>
        /// <returns></returns>
        public bool IsAtEnd()
        {
            return _windowIndex <= 0;
        }

        /// <summary>
        /// Create a subset of the data by cloning the original list and getting
        /// the range of the current _windowIndex until the end of the list.
        /// This returns a slice of the origin data list
        /// </summary>
        /// <returns></returns>
        public override DataPointList GetData()
        {
            var clone = _data.Clone();
            var ret = clone.GetRange((int)_windowIndex, (int)Length - (int)_windowIndex);
            return new DataPointList(ret);
        }

        public int CurrentIteration()
        {
            return _windowIndex == null ? 0 : (int)_windowIndex;
        }
    }
}

