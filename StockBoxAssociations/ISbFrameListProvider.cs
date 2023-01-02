using System;
using System.Collections.Generic;


namespace StockBox.Associations
{

    /// <summary>
    /// 
    /// </summary>
    public interface ISbFrameListProvider
    {
        List<ISbFrame> Create(IDomainCombinationsProvider combos, ISymbolProvider symbol);
        List<ISbFrame> CreateBacktestData(ISymbolProvider symbol);
        void AddIndicators(List<ISbFrame> framelist, IDomainCombinationsProvider domainCombinations);
        void HydrateFrameList(List<ISbFrame> frameList, IDomainCombinationsProvider domainCombinationsProvider);
    }
}
