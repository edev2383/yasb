﻿using System;
using Deedle;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;

namespace StockBox.Data.Indicators
{
    public abstract class BaseIndicator : IIndicator
    {
        public BaseIndicator(string column, EIndicatorType type, params int[] indices)
        {
            ColumnKey = column;
            Indices = indices;
            _type = type;
        }

        public string Name
        {
            get
            {
                string ret = string.Empty;
                if (!string.IsNullOrEmpty(ColumnKey))
                    ret += ColumnKey;
                if (Indices != null && Indices.Length > 0)
                    ret += $"({string.Join(",", Indices)})";
                return ret;
            }
        }

        public EIndicatorType Type { get { return _type; } }

        public string ColumnKey { get; set; }

        public int[] Indices { get; set; }

        public object Payload { get { return _payload; } }

        private object _payload;
        private EIndicatorType _type;

        public void Calculate(IDataFrameAdapter adapter)
        {
            _payload = CalculateIndicator(adapter);
        }

        /// <summary>
        /// All child calculations should be bracketted within this method and
        /// return the final response object to be set to the _payload value.
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns></returns>
        protected abstract object CalculateIndicator(IDataFrameAdapter adapter);

    }
}