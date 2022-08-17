﻿using System;
using static StockBox.Data.SbFrames.DataColumn;

namespace StockBox.Data.SbFrames
{
    public class DataPoint
    {
        public DateTime Date { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double AdjClose { get; set; }
        public int Volume { get; set; }

        public IndicatorDataPointList Indicators { get; set; } = new IndicatorDataPointList();

        public DataPoint()
        {
        }

        public DataPoint(DateTime date, double high, double low, double open, double close, double adjClose, int volume)
        {
            Date = date;
            High = high;
            Low = low;
            Open = open;
            Close = close;
            AdjClose = adjClose;
            Volume = volume;
        }

        public DataPoint(DataPoint source)
        {
            Date = source.Date;
            High = source.High;
            Low = source.Low;
            Open = source.Open;
            Close = source.Close;
            AdjClose = source.AdjClose;
            Volume = source.Volume;
            Indicators = source.Indicators.Clone();
        }

        public DataPoint Clone()
        {
            return new DataPoint(this);
        }

        public object GetByColumn(DataColumn tryColumn)
        {
            object ret = null;

            switch (tryColumn.GetColumnKey())
            {
                case EColumns.eHigh:
                    ret = High;
                    break;
                case EColumns.eLow:
                    ret = Low;
                    break;
                case EColumns.eOpen:
                    ret = Open;
                    break;
                case EColumns.eClose:
                    ret = Close;
                    break;
                case EColumns.eAdjClose:
                    ret = AdjClose;
                    break;
                case EColumns.eVolume:
                    ret = Volume;
                    break;
                default:
                    var foundIndicator = Indicators.FindByKey(tryColumn.Column);
                    if (foundIndicator != null)
                        ret = foundIndicator.Value;
                    break;
            }

            if (ret == null)
                throw new ArgumentOutOfRangeException($"GetByColumn: provided column not found: {tryColumn.Column}");

            return ret;
        }

        public void AddIndicatorValue(string column, object value)
        {
            Indicators.Add(new IndicatorDataPoint(column, value));
        }
    }
}