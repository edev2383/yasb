using System;
namespace StockBox.Associations.Tokens
{

    public class DomainCombination
    {
        public double IntervalIndex { get; set; }
        public Token IntervalFrequency { get; set; }
        public string DomainKeyword { get; set; }
        public int[] Indices { get; set; }

        public bool IsIndicator { get { return IsKeywordAnIndicator(); } }

        public DomainCombination(object intervalIndex, Token intervalFrequency, string domainKeyword, int[] indices)
        {
            double.TryParse(intervalIndex.ToString(), out double result);
            IntervalIndex = result;
            IntervalFrequency = intervalFrequency;
            DomainKeyword = domainKeyword;
            Indices = indices;
        }

        public DomainCombination(DomainCombination source)
        {
            IntervalIndex = source.IntervalIndex;
            IntervalFrequency = source.IntervalFrequency;
            DomainKeyword = source.DomainKeyword;
            Indices = source.Indices;
        }

        public DomainCombination Clone()
        {
            return new DomainCombination(this);
        }

        private bool IsKeywordAnIndicator()
        {
            bool ret = false;

            switch (DomainKeyword.ToLower())
            {
                case "sma": ret = true; break;
                default: break;
            }
            return ret;
        }
    }
}
