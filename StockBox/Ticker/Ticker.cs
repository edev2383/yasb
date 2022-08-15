using System;
using StockBox.Data.SbFrames;

namespace StockBox.Ticker
{
    public class Ticker
    {
        public SbFrameList Data { get { return _data; } }
        private readonly SbFrameList _data = new SbFrameList();

        public Ticker(SbFrameList data)
        {
            _data = data;
        }

        public Ticker()
        {
        }
    }
}
