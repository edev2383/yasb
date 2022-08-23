using System;
using System.IO;

namespace StockBox.Associations
{
    public interface ICallContextProvider
    {
        MemoryStream GetDaily();
        MemoryStream GetWeekly();
        MemoryStream GetMontly();
    }
}
