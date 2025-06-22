using System;
using System.Text.Json;
using StockManagement.Entities;

namespace StockManagement.Services;

public class FakeStoreService : IStoreService
{
    public async Task<List<StoreProduct>?> GetStoreProducts(string storeUrl)
    {
        using var httpClient = new HttpClient();
        List<StoreProduct>? fakeStoreProducts = null;
            
            HttpResponseMessage response = await httpClient.GetAsync(storeUrl);
            response.EnsureSuccessStatusCode(); // throws exception for 4xx/5xx

            string json = await response.Content.ReadAsStringAsync();

            fakeStoreProducts = JsonSerializer.Deserialize<List<StoreProduct>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return fakeStoreProducts;
    }
}
