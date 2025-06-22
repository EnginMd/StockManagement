using System;

namespace StockManagement.Entities;

public class StoreProduct
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public double Price { get; set; }

    public required string Description { get; set; }

    public required string Category { get; set; }

    public required string Image { get; set; }

    public FakeStoreRating? Rating { get; set; }


}
