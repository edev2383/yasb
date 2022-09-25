using System.IO;


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
            // forward testing requires a descending ordered data set
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
