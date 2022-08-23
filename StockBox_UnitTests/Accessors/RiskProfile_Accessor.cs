using System;
using StockBox.RiskProfiles;

namespace StockBox_UnitTests.Accessors
{
    public class RiskProfile_Accessor : RiskProfile
    {
        public RiskProfile_Accessor()
        {
        }

        public void SetRiskPercent(double p)
        {
            _totalRiskPercent = p;
        }

        public static RiskProfile_Accessor CreateSample_RiskDollars()
        {
            var rpa = new RiskProfile_Accessor();
            rpa.TotalBalance = 10000;
            rpa.ActiveBalance = 500;
            rpa.TotalRiskDollars = 200;
            return rpa;
        }

        public static RiskProfile_Accessor CreateSample_RiskPercent(double percent = 15)
        {
            var rpa = new RiskProfile_Accessor();
            rpa.TotalBalance = 10000;
            rpa.ActiveBalance = 500;
            rpa.SetRiskPercent(percent);
            return rpa;
        }

        public static RiskProfile_Accessor CreateSample_NoTotalRiskSet()
        {
            var rpa = new RiskProfile_Accessor();
            rpa.TotalBalance = 10000;
            rpa.ActiveBalance = 500;
            return rpa;
        }
    }
}
