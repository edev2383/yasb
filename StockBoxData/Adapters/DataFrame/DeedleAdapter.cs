using System.IO;
using StockBox.Data.SbFrames;

namespace StockBox.Data.Adapters.DataFrame
{

    /// <summary>
    /// Class <c>DeedleAdapter</c> reverses the data set so DateTime keys are in
    /// reverse chronological order
    /// </summary>
    public class DeedleAdapter : BaseDataFrameAdapter
    {

        public DeedleAdapter() { }
        public DeedleAdapter(MemoryStream data) : base(data)
        {
        }

        /// <summary>
        /// Create a new DeedleAdapter object
        /// </summary>
        /// <returns></returns>
        public override IDataFrameAdapter Create()
        {
            return new DeedleAdapter();
        }

        /// <summary>
        /// Return the whole DataPointList
        /// </summary>
        /// <returns></returns>
        public override DataPointList GetData()
        {
            return _data;
        }
    }
}
