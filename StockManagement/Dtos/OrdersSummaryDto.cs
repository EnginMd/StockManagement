namespace StockManagement.Dtos;

public record class OrdersSummaryDto(
    int Id,
    string Title,
    double Price,
    string Category,
    string Image
);
