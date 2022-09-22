using System;
using System.IO;

namespace StockBox.Associations
{
    public interface IStreamProvider
    {
        MemoryStream Stream { get; }
    }
}
