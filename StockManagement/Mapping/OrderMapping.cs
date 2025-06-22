using System;
using StockManagement.Dtos;
using StockManagement.Entities;

namespace StockManagement.Mapping;

public static class OrderMapping
{
    public static OrdersSummaryDto ToOrdersSummaryDto(this StoreProduct product)
    {
        return new(

            product.Id,
            product.Title,
            product.Price,
            product.Category,
            product.Image
        );
    }
}
