using System;
using System.IO;
using Deedle;
using StockBox.Data.SbFrames;

namespace StockBox.Data.Adapters.DataFrame
{
    public class DeedleAdapter : BaseDataFrameAdapter
    {

        public DeedleAdapter() { }
        public DeedleAdapter(MemoryStream data) : base(data)
        {
            // forward testing requires a descending ordered data set
            _data = _data.Reversed;
        }

        public override IDataFrameAdapter Create()
        {
            return new DeedleAdapter();
        }

        /// <summary>
        /// Override so we can reverse the data chronologically
        /// </summary>
        /// <param name="data"></param>
        public override void AddData(MemoryStream data)
        {
            base.AddData(data);
            _data = _data.Reversed;
        }
    }
}
