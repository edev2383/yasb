using System;
using StockBox.Models;
using StockBox.Setups;


namespace StockBox.Controllers
{

    public interface ISbController
    {
        void ScanSetup(Setup setup, SymbolProfileList profiles);
        void ScanSetup(SetupList setups, SymbolProfileList profiles);
    }
}
