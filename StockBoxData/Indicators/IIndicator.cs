using System;
using System.Data;
using Deedle;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;

namespace StockBox.Data.Indicators
{
    public interface IIndicator
    {
        EIndicatorType Type { get; }
        string Name { get; }
        string ColumnKey { get; }
        int[] Indices { get; }
        object Payload { get; }

        void Calculate(IDataFrameAdapter frame);
    }
}
