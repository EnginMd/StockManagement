using Microsoft.AspNetCore.Antiforgery;
using StockManagement.Dtos;
using StockManagement.Entities;
using StockManagement.Mapping;
using StockManagement.Services;

namespace StockManagement.Endpoints;

public static class ProductsEndpoints
{
    private static readonly List<Product> products = [];
    const string GetProductEndpointName = "GetProduct";

    public static List<Product> Products { get { return products; } } 
    
    public static RouteGroupBuilder MapProductsEndpoints(this WebApplication app, string fakeStoreUrl)
    {
        var group = app.MapGroup("products")
                       .WithParameterValidation()
                       .RequireRateLimiting("fixed");

        //GET /products
        group.MapGet("/", () => Results.Ok(products.Select(x => x.ToProductsSummaryDto())))

             .WithSummary("Get all products.")
             .WithDescription("This endpoint returns the list of all products manually added to the system."); ;

        //GET /products/low-stock
        group.MapGet("/low-stock", () =>
        {
            var lowStocks = products.FindAll(x => x.InitialStockCount < x.CriticalStockCount)
                                    .Select(x => x.ToProductsSummaryDto());

            return Results.Ok(lowStocks);
        })
            .WithSummary("Get products with low stock.")
            .WithDescription("This endpoint returns a list of products whose stock quantity is below the CriticalStockCount threshold.");

        //GET /products/id
        group.MapGet("/{id}", (int id) =>
        {
            return products.Find(product => product.Id == id);

        })
            .WithName(GetProductEndpointName)
            .WithSummary("Get product.")
            .WithDescription("This endpoint returns the product with the specified ID.");

        //POST /products
        group.MapPost("/", async (CreateProductDto newProduct, HttpContext context, IAntiforgery antiforgery) =>
        {
            await antiforgery.ValidateRequestAsync(context);

            IStoreService storeService = new FakeStoreService();
            List<StoreProduct>? fakeStoreProducts = await storeService.GetStoreProducts(fakeStoreUrl);

            StoreProduct? fakeProduct = fakeStoreProducts!.Find(product =>
                                    product.Title == newProduct.ProductCode);

            if (fakeProduct is null)
            {
                return Results.NotFound("Product code not found in store.");
            }

            Product product = newProduct.ToEntity(products.Count + 1);
            products.Add(product);

            return Results.CreatedAtRoute(GetProductEndpointName, new { id = product.Id }, product);
        })
            .WithSummary("Add product.")
            .WithDescription("This endpoint creates a new product in the system based on the provided data."); 

        return group;
    }

}
