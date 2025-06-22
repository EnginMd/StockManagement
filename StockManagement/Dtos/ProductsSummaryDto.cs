namespace StockManagement.Dtos;

public record class ProductsSummaryDto(

    int Id,
    string Name,
    string ProductCode,
    int InitialStockCount,
    int CriticalStockCount
);
