using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StockBox.Data.SbFrames
{

    /// <summary>
    /// Class <c>DataColumn</c> encapsulates and describes a column and
    /// potential indices
    ///
    /// TODO: The usage of Column within this class has become diluted. Need to
    /// refactor this to make more sense without impacting the codebase too
    /// much. 
    /// </summary>
    public class DataColumn
    {

        public enum EColumns
        {
            // Core columns
            eDate = 0, // index
            eOpen,
            eHigh,
            eLow,
            eClose,
            eAdjClose,
            eVolume,

            // indicator columns
            eSma,
            eAvgVolume,
            eRsi,
            eEma,
            eSloSto,
            eFastSto,
            eBBandsLower,
            eBBandsHigher,
            eBBandsCenter,
            eAtr,
            ePriceChannel,
        }

        public string Column { get { return MapToColumnString(); } }

        /// <summary>
        /// ColumnToken is a string representation of the Column without any
        /// indices, i.e., SMA(50), Column = SMA(50), ColumnToken = SMA
        /// </summary>
        public string ColumnToken { get; set; }
        public List<int> Indices { get { return _indices; } }
        public EColumns EColumn { get { return _column; } }
        private EColumns _column;
        private List<int> _indices = new List<int>();


        public DataColumn(string column, params int[] indices)
        {
            _column = SanitizeRawStringColumn(column);
            ColumnToken = column;
            if (indices != null)
            {
                foreach (var item in indices)
                    _indices.Add(item);
            }
        }


        public DataColumn(EColumns column, params int[] indices)
        {
            _column = column;
            foreach (var item in indices)
                _indices.Add(item);
        }

        /// <summary>
        /// Return the enum for the targeted column
        /// </summary>
        /// <returns></returns>
        public EColumns GetColumnKey()
        {
            return _column;
        }

        public static DataColumn ParseColumnDescriptor(string descriptor)
        {
            string column = string.Empty;
            int[] indices = null;
            var re = @"^([a-zA-z0-9]+)(\(([0-9,\s]+)\))?$";
            var allMatches = Regex.Matches(descriptor, re);

            if (allMatches.Count == 0)
                throw new Exception($"Unknown Column descriptor ({descriptor}) provided");

            var match = allMatches.First();

            if (match.Groups.Count < 2)
                throw new Exception($"Unkown Column descriptor ({descriptor}) provided");

            column = match.Groups[1].Value.ToLower();

            if (match.Groups.Count < 4)
                throw new Exception($"Unkown Column descriptor ({descriptor}) provided");

            if (match.Groups[3].Value != string.Empty)
            {
                var splitMatch = match.Groups[3].Value.Split(',');
                var itemList = new List<int>();
                foreach (var item in splitMatch)
                {
                    if (int.TryParse(item.Trim(), out int resultInt))
                        itemList.Add(resultInt);
                }
                indices = itemList.ToArray();
            }

            var ret = new DataColumn(column, indices);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string MapToColumnString()
        {
            var ret = string.Empty;

            switch (_column)
            {
                case EColumns.eDate:
                    ret = "Date";
                    break;
                case EColumns.eOpen:
                    ret = "Open";
                    break;
                case EColumns.eHigh:
                    ret = "High";
                    break;
                case EColumns.eLow:
                    ret = "Low";
                    break;
                case EColumns.eClose:
                    ret = "Close";
                    break;
                case EColumns.eAdjClose:
                    ret = "AdjClose";
                    break;
                case EColumns.eSma:
                    ret = $"SMA({string.Join(",", _indices)})";
                    break;
                case EColumns.eSloSto:
                    ret = $"SlowSto({string.Join(",", _indices)})";
                    break;
                case EColumns.eAvgVolume:
                    ret = $"AvgVolume({string.Join(",", _indices)})";
                    break;
                case EColumns.eVolume:
                    ret = "Volume";
                    break;
                case EColumns.eRsi:
                    ret = $"RSI({string.Join(",", _indices)})";
                    break;
                case EColumns.eFastSto:
                    ret = $"FastSto({string.Join(",", _indices)})";
                    break;
                case EColumns.eAtr:
                    ret = $"ATR({string.Join(",", _indices)})";
                    break;
                case EColumns.ePriceChannel:
                    ret = $"PC({string.Join(",", _indices)})";
                    break;
                case EColumns.eEma:
                    break;
                case EColumns.eBBandsLower:
                    break;
                case EColumns.eBBandsHigher:
                    break;
                case EColumns.eBBandsCenter:
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown column value provided {_column}");
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private EColumns SanitizeRawStringColumn(string column)
        {
            switch (column.ToLower())
            {
                case "open":
                    return EColumns.eOpen;
                case "adjclose":
                    return EColumns.eAdjClose;
                case "low":
                    return EColumns.eLow;
                case "high":
                    return EColumns.eHigh;
                case "close":
                    return EColumns.eClose;
                case "sma":
                    return EColumns.eSma;
                case "volume":
                    return EColumns.eVolume;
                case "avgvolume":
                    return EColumns.eAvgVolume;
                case "rsi":
                    return EColumns.eRsi;
                case "slowsto":
                case "slosto":
                    return EColumns.eSloSto;
                case "faststo":
                    return EColumns.eFastSto;
                case "atr":
                    return EColumns.eAtr;
                case "pc":
                case "chan":
                    return EColumns.ePriceChannel;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
