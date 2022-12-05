using System;
using StockBox.Rules;

namespace StockBox_TestArtifacts.Presets.StockBox.Rules
{
    public class PresetPattern
    {
        public PresetPattern()
        {
        }

        /// <summary>
        /// Simple 3 day pull back after a positive close, 1g-3r, with an SMA(5)
        /// > SMA(20)
        /// </summary>
        /// <returns></returns>
        public static Pattern ThreeDayPullBack()
        {
            return new Pattern() {
                new Rule("Close < Yesterday Close"),
                new Rule("Yesterday Close < 2 days ago Close"),
                new Rule("2 days ago Close < 3 day ago Close"),
                new Rule("3 day ago close > 4 day ago close"),
                new Rule("SMA(5) > SMA(20)"),
            };
        }
    }
}

