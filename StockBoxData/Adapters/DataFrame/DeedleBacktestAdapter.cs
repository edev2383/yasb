using System.IO;
using StockBox.Data.SbFrames;


namespace StockBox.Data.Adapters.DataFrame
{

    /// <summary>
    /// Class <c>DeedleBacktestAdapter</c> creates a growing subset of data to
    /// test against, simulating the passing of time toward the present. This
    /// allows us to test Setups over time, versus the DeedleAdapter which only
    /// tests from the most recent date as the origin zero-index.
    /// </summary>
    public class DeedleBacktestAdapter : BaseDataFrameAdapter
    {
        public DeedleBacktestAdapter()
        {
        }
        public DeedleBacktestAdapter(MemoryStream data) : base(data)
        {
        }
        public DeedleBacktestAdapter(DataPointList data)
        {
            if (!data.IsDesc)
                data = data.Reversed;
            AddData(data);
        }

        private int? _windowIndex;
        private DataPointList _viewData { get { return GetData(); } }

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
        /// Create a new Backtest adapter
        /// </summary>
        /// <returns></returns>
        public override IDataFrameAdapter Create()
        {
            return new DeedleBacktestAdapter();
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
        protected override DataPointList GetData()
        {
            var clone = _data.Clone();
            var ret = clone.GetRange((int)_windowIndex, (int)Length - (int)_windowIndex);
            return new DataPointList(ret);
        }
    }
}
