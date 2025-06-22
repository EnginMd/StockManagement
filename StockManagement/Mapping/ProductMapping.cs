using System;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using StockManagement.Dtos;
using StockManagement.Entities;

namespace StockManagement.Mapping;

public static class ProductMapping
{
    public static Product ToEntity(this CreateProductDto product, int id)
    {
        return new()
        {
            Id = id,
            ProductCode = product.ProductCode,
            Name = product.Name,
            InitialStockCount = product.InitialStockCount,
            CriticalStockCount = product.CriticalStockCount
        };
    }

    public static ProductsSummaryDto ToProductsSummaryDto(this Product product)
    {
        return new(
        
            product.Id,
            product.Name,
            product.ProductCode,
            product.InitialStockCount,
            product.CriticalStockCount
        );
    } 
}
