using System;
using StockBox.Associations.Enums;

namespace StockBox.Associations
{
    public interface ISbFrame
    {
        EFrequency Frequency { get; }
    }
}

