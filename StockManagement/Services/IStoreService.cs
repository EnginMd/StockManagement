using System;
using StockManagement.Entities;

namespace StockManagement.Services;

public interface IStoreService
{
    Task<List<StoreProduct>?> GetStoreProducts(string storeUrl);

}
