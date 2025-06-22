using System;

namespace StockManagement.Entities;

public class Product
{
    public int Id { get; set; }
    public required string ProductCode { get; set; }

    public required string Name { get; set; }

    public int InitialStockCount { get; set; }

    public int CriticalStockCount { get; set; }


}
