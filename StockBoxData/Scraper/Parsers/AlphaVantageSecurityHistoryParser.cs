using StockBox.Data.SbFrames;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StockBox.Data.Scraper.Parsers
{
    public class AlphaVantageSecurityHistoryParser : ScraperParserBase
    {

        public AlphaVantageSecurityHistoryParser() { }

        public class HistoryParser_OutType : OutType
        {
            public DataPointList Data { get; set; }
        }

        protected override OutType GetPayload(string json)
        {
            var responseObj = JsonSerializer.Deserialize<AlphaVantageResponse>(json);
            var dataPointList = new DataPointList();

            Dictionary<string, TimeSeries> timeSeries = responseObj.GetTimeSeries();

            if (timeSeries == null) throw new Exception("No time series found in response object");

            foreach (var ts in timeSeries)
            {

                if (DateTime.TryParse(ts.Key, out DateTime dt))
                {
                    var dp = new DataPoint()
                    {
                        Date = dt,
                        Open = ts.Value.Open != null ? double.Parse(ts.Value.Open) : 0,
                        High = ts.Value.High != null ? double.Parse(ts.Value.High) : 0,
                        Low = ts.Value.Low != null ? double.Parse(ts.Value.Low) : 0,
                        Close = ts.Value.Close != null ? double.Parse(ts.Value.Close) : 0,
                        AdjClose = ts.Value.Close != null ? double.Parse(ts.Value.Close) : 0,
                        Volume = ts.Value.Volume != null ? double.Parse(ts.Value.Volume) : 0,
                    };
                    dataPointList.Add(dp);
                }
            }

            return new HistoryParser_OutType()
            {
                Data = dataPointList,
            };
        }


        internal class AlphaVantageResponse
        {

            [JsonPropertyName("Meta Data")]
            public MetaData MetaData { get; set; }

            [JsonPropertyName("Time Series (Daily)")]
            public Dictionary<string, TimeSeries> DailyAdjustedTimeSeries { get; set; }

            [JsonPropertyName("Monthly Adjusted Time Series")]
            public Dictionary<string, TimeSeries> MonthlyAdjustedTimeSeries { get; set; }
            [JsonPropertyName("Weekly Adjusted Time Series")]
            public Dictionary<string, TimeSeries> WeeklyAdjustedTimeSeries { get; set; }

            public Dictionary<string, TimeSeries> GetTimeSeries()
            {
                if (DailyAdjustedTimeSeries != null)
                {
                    return DailyAdjustedTimeSeries;
                }
                else if (MonthlyAdjustedTimeSeries != null)
                {
                    return MonthlyAdjustedTimeSeries;
                }
                else if (WeeklyAdjustedTimeSeries != null)
                {
                    return WeeklyAdjustedTimeSeries;
                }
                else
                {
                    return null;
                }
            }
        }

        internal class MetaData
        {
            [JsonPropertyName("1. Information")]
            public string Information { get; set; }

            [JsonPropertyName("2. Symbol")]
            public string Symbol { get; set; }

            [JsonPropertyName("3. Last Refreshed")]
            public string LastRefreshed { get; set; }

            [JsonPropertyName("4. Output Size")]
            public string OutputSize { get; set; }

            [JsonPropertyName("5. Time Zone")]
            public string TimeZone { get; set; }
        }

        internal class TimeSeries
        {

            [JsonPropertyName("1. open")]
            public string Open { get; set; }

            [JsonPropertyName("2. high")]
            public string High { get; set; }

            [JsonPropertyName("3. low")]
            public string Low { get; set; }

            [JsonPropertyName("4. close")]
            public string Close { get; set; }

            //[JsonPropertyName("5. adjusted close")]
            //public string AdjustedClose { get; set; }

            [JsonPropertyName("5. volume")]
       
            public string Volume { get; set; }

            //[JsonPropertyName("7. dividend amount")]

            //public string DividendAmount { get; set; }

            //[JsonPropertyName("8. split coefficient")]
            //public string SplitCoefficient { get; set; }

        }

    }
}
