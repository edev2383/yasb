using System;
namespace StockBox.RiskProfiles
{
    /// <summary>
    /// RiskProfile will define all of the variables related to the user's
    /// action. Total requested shares/percentage, etc
    /// </summary>
    public class RiskProfile
    {
        public int? RiskProfileId { get; set; }

        public RiskProfile()
        {
        }
    }
}
