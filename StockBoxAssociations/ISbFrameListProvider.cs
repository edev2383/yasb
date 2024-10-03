using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace StockBox.Associations
{

    /// <summary>
    /// 
    /// </summary>
    public interface ISbFrameListProvider
    {
        Task<List<ISbFrame>> Create(IDomainCombinationsProvider combos, ISymbolProvider symbol);
        Task<List<ISbFrame>> CreateBacktestData(ISymbolProvider symbol);
        void AddIndicators(List<ISbFrame> framelist, IDomainCombinationsProvider domainCombinations);
        void HydrateFrameList(List<ISbFrame> frameList, IDomainCombinationsProvider domainCombinationsProvider);
    }
}
