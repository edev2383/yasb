using System;
using System.Collections.Generic;
using StockBox.Associations;
using sds = StockBox.Data.Scraper;

namespace StockBox.Data.Context.Providers
{
    public class CallContextProviderRegistry
    {
        public List<ICallContextProvider> Register { get { return _registry; } }
        public List<ICallContextProvider> _registry = new List<ICallContextProvider>();

        public CallContextProviderRegistry()
        {
            _registry.Add(new sds.SbScraper());
        }
    }
}
