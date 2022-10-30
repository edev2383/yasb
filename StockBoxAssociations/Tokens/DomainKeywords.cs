namespace StockBox.Associations.Tokens
{
    public class DomainKeywords : KeywordList
    {
        public DomainKeywords() : base()
        {
        }

        protected override void Init()
        {
            // 
            Add("close", TokenType.eColumn);
            Add("high", TokenType.eColumn);
            Add("low", TokenType.eColumn);
            Add("open", TokenType.eColumn);
            Add("volume", TokenType.eColumn);
            Add("sma", TokenType.eIndicator, true);

            // Note: any "'s" that are typed into the rules are
            // ignored by the scanner *as expected behavior* We
            // don't need the possessive. We could also have
            // stripped out the character from the rules, but we
            // may have use for it in the future and changes
            // will be relatively easy
            Add("day", TokenType.eDaily);
            Add("days", TokenType.eDaily);
            Add("daily", TokenType.eDaily);
            Add("week", TokenType.eWeekly);
            Add("weeks", TokenType.eWeekly);
            Add("weekly", TokenType.eWeekly);
            Add("month", TokenType.eMonthly);
            Add("months", TokenType.eMonthly);
            Add("monthly", TokenType.eMonthly);

            Add("last", TokenType.eLast);
            Add("yesterday", TokenType.eYesterday);
            Add("yesterdays", TokenType.eYesterday);

            Add("x", TokenType.eCrossOver);

            Add("@entry", TokenType.eEntryPoint);
            Add("@52weekhigh", TokenType.e52WeekHigh);
            Add("@52weeklow", TokenType.e52WeekLow);
            Add("@ath", TokenType.eAllTimeHigh);
            Add("@atl", TokenType.eAllTimeLow);

            Add("and", TokenType.eAnd);

            #region Ignore Tokens

            Add("ago", TokenType.eAgo);

            #endregion

            Add("true", TokenType.eTrue);
            Add("false", TokenType.eFalse);

            #region Language Keywords
            Add("var", TokenType.eVar);
            #endregion
        }
    }
}
