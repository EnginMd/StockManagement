using System.ComponentModel.DataAnnotations;

namespace StockManagement.Dtos;

public record class CreateProductDto(

    [Required][StringLength(80)] string Name,
    [Required][StringLength(250)] string ProductCode,
    int InitialStockCount,
    [Range(1, 100)] int CriticalStockCount
);
