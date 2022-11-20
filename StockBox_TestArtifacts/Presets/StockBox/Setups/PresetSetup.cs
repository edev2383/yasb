using System;
using StockBox.Rules;
using StockBox.Setups;

namespace StockBox_TestArtifacts.Presets.StockBox.Setups
{
    public class PresetSetup
    {
        public PresetSetup()
        {
        }

        public static Setup SMA2Crossover()
        {
            var pattern = new Pattern();
            pattern.Add(new Rule("SMA(2) X Close"));
            var ret = new Setup();
            ret.Rules = pattern;
            return ret;
        }
    }
}
