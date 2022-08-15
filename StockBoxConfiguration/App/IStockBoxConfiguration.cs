using System;
namespace StockBox.Configuration.App
{
    public interface IStockBoxConfiguration
    {
        EEnvironment Environment { get; }
    }
}
