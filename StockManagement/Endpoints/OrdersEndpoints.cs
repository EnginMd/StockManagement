using StockManagement.Entities;
using StockManagement.Mapping;
using StockManagement.Services;

namespace StockManagement.Endpoints;

public static class OrdersEndpoints
{
    public static RouteGroupBuilder MapOrdersEndpoints(this WebApplication app, string fakeStoreUrl)
    {
        var group = app.MapGroup("orders")
                       .RequireRateLimiting("fixed");

        //POST /orders/check-and-place
        group.MapPost("/check-and-place", async () =>
        {
            List<StoreProduct> orders = [];

            IStoreService storeService = new FakeStoreService();
            List<StoreProduct>? fakeStoreProducts = await storeService.GetStoreProducts(fakeStoreUrl);
            string productCode;

            var lowStocks = ProductsEndpoints.Products.FindAll(x => x.InitialStockCount < x.CriticalStockCount);

            foreach (var product in lowStocks)
            {
                productCode = product.ProductCode;
                var matchingProducts = fakeStoreProducts!
                            .Where(p => p.Title.Contains(productCode, StringComparison.OrdinalIgnoreCase))
                            .ToList();

                if (!matchingProducts.Any())
                    throw new Exception($"Urun kodu '{productCode}' ile eslesen urun bulunamadi.");

                orders.Add(matchingProducts.OrderBy(p => p.Price).First());
            }
            //return Results.Created();
            return Results.Created("", orders.Select(order => order.ToOrdersSummaryDto()));
        })
            .WithSummary("Check stock and place order.")
            .WithDescription("This endpoint detects products below the stock threshold and automatically places orders for them."); 

        return group;
    }

}
