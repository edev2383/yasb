using StockBox.Associations.Enums;
using StockBox.Validation;
using System;


namespace StockBox.RiskProfiles
{

    /// <summary>
    /// RiskProfile will define all of the variables related to the user's
    /// action. Total requested shares/percentage, etc
    ///
    /// RiskProfile Rules:
    /// 
    /// Risk supercedes all other calculations/settings when LOWER, i.e.
    ///     If TotalRiskDollars is 200 but MaxPositionDollars is 150, Max will
    ///     be used. If TotalRiskDollars is lower than the requested position,
    ///     TotalRiskDollars will be used to calculate SharesTotal
    /// Explicit dollar values supercede like percentage values.
    /// All percents are doubles between 0.00 - 1.00
    /// </summary>
    public class RiskProfile
    {

        public Guid? RiskProfileGuid { get; set; }


        #region Primary Position sizing properties

        /// <summary>
        /// Maximum dollar amount to be spent.
        ///  - Supercedes MaxPositionShares
        ///  - Is superceded by TotalRiskDollars AND TotalRiskPercent
        ///  - The lessor of the compared values is what is taken
        /// </summary>
        public int? MaxPositionDollars { get; set; }

        /// <summary>
        /// Maximum number of shares to purchase
        ///  - Is superceded by MaxPositionDollars and both TotalRisk* props
        /// </summary>
        public int? MaxPositionShares { get; set; }

        /// <summary>
        /// Supercedes all other position sizing properties. Given this fact,
        /// if a user wants their positions sizes to increase as balance grows
        /// they should set a TotalRiskPercent, as the TotalRiskDollars will
        /// always remain the same and be used with priority
        /// </summary>
        public double? TotalRiskDollars { get; set; }

        /// <summary>
        /// The total percent of `TotalBalance` that a user is willing to risk
        /// on a given position.
        ///  - Is superceded by TotalRiskDollars
        ///  - Supercedes both MaxPositionDollars and MaxPositionShares
        ///  - Allows for larger positions as Balance grows
        ///  - If not set, the Normalization is bypassed because we want the
        ///    null to throw an exception when calculating
        /// </summary>
        public double? TotalRiskPercent
        {
            get
            {
                if (_totalRiskPercent != null)
                    return NormalizeUserInput_Percentage(_totalRiskPercent);
                return null;
            }
        }

        #endregion

        public double? TrailingStopDollars { get; set; }
        public double? TrailingStopPercent { get { return NormalizeUserInput_Percentage(_trailingStopPercent); } }
        public double? TargetDollars { get; set; }
        public double? TargetPercent { get { return NormalizeUserInput_Percentage(_targetPercent); } }
        public bool? SellHalf { get; set; }
        public double? SellHalfTargetDollars { get; set; }
        public double? SellHalfTargetPercent { get { return NormalizeUserInput_Percentage(_sellHalfTargetPercent); } }
        public double? StopLossDollars { get; set; }
        public double? StopLossPercent { get { return NormalizeUserInput_Percentage(_stopLossPercent); } }
        public int? ValidDuration { get; set; }
        public EFrequency DurationType { get; set; }
        public double? TotalBalance { get; set; }
        public double? ActiveBalance { get; set; }

        protected double? _totalRiskPercent;
        protected double? _trailingStopPercent;
        protected double? _targetPercent;
        protected double? _sellHalfTargetPercent;
        protected double? _stopLossPercent;

        public RiskProfile(Guid? riskProfileId, double? totalBalance, double? activeBalance, int? maxPositionDollars,
            int? maxPositionShares, double? totalRiskDollars, double? totalRiskPercent, double? trailingStopDollars,
            double? trailingStopPercent, double? targetDollars, double? targetPercent, bool? sellhalf,
            double? sellHalfTargetDollars, double? sellHalfTargetPercent, double? stopLossDollars, double? stopLossPercent,
            int? validDuration, EFrequency durationType)
        {
            RiskProfileGuid = riskProfileId;
            TotalBalance = totalBalance;
            ActiveBalance = activeBalance;
            MaxPositionDollars = maxPositionDollars;
            MaxPositionShares = maxPositionShares;
            TotalRiskDollars = totalRiskDollars;
            _totalRiskPercent = totalRiskPercent;
            TrailingStopDollars = trailingStopDollars;
            _trailingStopPercent = trailingStopPercent;
            TargetDollars = targetDollars;
            _targetPercent = targetPercent;
            SellHalf = sellhalf;
            SellHalfTargetDollars = sellHalfTargetDollars;
            _sellHalfTargetPercent = sellHalfTargetPercent;
            StopLossDollars = stopLossDollars;
            _stopLossPercent = stopLossPercent;
            ValidDuration = validDuration;
            DurationType = durationType;
        }

        public RiskProfile(RiskProfile source) : this(source.RiskProfileGuid, source.TotalBalance, source.ActiveBalance, source.MaxPositionDollars,
            source.MaxPositionShares, source.TotalRiskDollars, source.TotalRiskPercent, source.TrailingStopDollars,
            source.TrailingStopPercent, source.TargetDollars, source.TargetPercent, source.SellHalf,
            source.SellHalfTargetDollars, source.SellHalfTargetDollars, source.StopLossDollars, source.StopLossPercent,
            source.ValidDuration, source.DurationType)
        { }

        public RiskProfile() { }

        public RiskProfile Clone()
        {
            return new RiskProfile(this);
        }

        /// <summary>
        /// Calculate number of shares to request for purchase based on user
        /// provided values
        /// </summary>
        /// <param name="estimatedSharePrice"></param>
        /// <returns></returns>
        public (ValidationResultList Results, int? Shares) CalculateTotalShares(double estimatedSharePrice)
        {
            int ret = 0;
            var vr = new ValidationResultList();

            try
            {
                var riskOverride = CalculateRiskOverride();
                var requestedPositionSize = CalculateRequestedPosition(estimatedSharePrice);
                ret = NormalizeRiskCalculation(riskOverride, requestedPositionSize, estimatedSharePrice);
                vr.Add(new ValidationResult(EResult.eSuccess, $"Value calcualted: {ret}"));
                return (vr, ret);
            }
            catch (Exception e)
            {
                vr.Add(new ValidationResult(EResult.eFail, e.Message));
                return (vr, ret);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="estimatedSharePrice"></param>
        /// <returns></returns>
        private double CalculateRequestedPosition(double estimatedSharePrice)
        {
            if (MaxPositionDollars != null) return (double)MaxPositionDollars;
            if (MaxPositionShares != null) return (double)(MaxPositionShares * estimatedSharePrice);
            return 0;
        }

        private double CalculateRiskOverride()
        {
            if (TotalRiskDollars == null && TotalRiskPercent == null) throw new ArgumentException("AT LEAST ONE of the following `RiskProfile` properties MUST BE set to an integer value greater than 0: [TotalRiskDollars] or [TotalRiskPercent]");
            if (TotalRiskDollars != null) return (double)TotalRiskDollars;
            if (TotalRiskPercent != null) return (double)(TotalBalance * TotalRiskPercent);
            return 0;
        }

        private double NormalizeUserInput_Percentage(double? value)
        {
            if (value == null) return 0;
            if (value > 100 || value < 0) throw new ArgumentException("Please provide a percentage value between 0.00 - 1.00. A suggested RiskProfile rule of thumb is to never risk more than 4% (0.04) per trade.");
            if (value > 1) return (double)(value / 100);
            return (double)value;
        }

        private int NormalizeRiskCalculation(double calculatedRisk, double requestedPositionSize, double sharePrice)
        {
            if (calculatedRisk == 0) throw new Exception("`CalculateRiskOverride` returned an illegal calculation value (0)");
            if (requestedPositionSize == 0 || calculatedRisk > requestedPositionSize) return (int)(calculatedRisk / sharePrice);
            return (int)(requestedPositionSize / sharePrice);
        }
    }
}
