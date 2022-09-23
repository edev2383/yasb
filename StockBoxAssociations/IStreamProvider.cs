using System;
using System.IO;


namespace StockBox.Associations
{

    /// <summary>
    /// Interface IStreamProvider provides a public property `Stream`
    /// </summary>
    public interface IStreamProvider
    {
        MemoryStream Stream { get; }
    }
}
