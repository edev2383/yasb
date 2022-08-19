using System.IO;
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
    }
}
