﻿using System.IO;
using StockBox.Data.SbFrames;

namespace StockBox.Data.Adapters.DataFrame
{
    public class DeedleBacktestAdapter : BaseDataFrameAdapter
    {
        public DeedleBacktestAdapter(MemoryStream data):base(data) { }
    }
}