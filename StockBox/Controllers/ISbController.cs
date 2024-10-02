using System;
using System.Threading.Tasks;
using StockBox.Models;
using StockBox.Setups;


namespace StockBox.Controllers
{

    public interface ISbController
    {
        Task ScanSetup(Setup setup, SymbolProfileList profiles);
        Task ScanSetups(SetupList setups, SymbolProfileList profiles);
    }
}
