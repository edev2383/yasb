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

            // 
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
            Add("yesterday's", TokenType.eYesterday);

            Add("and", TokenType.eAnd);

            #region Ignore Tokens

            Add("ago", TokenType.eAgo);

            #endregion

            Add("true", TokenType.eTrue);
            Add("false", TokenType.eFalse);
        }
    }
}
