using WebApi.Models;
using Microsoft.Extensions.Options;
using System.Linq;
using MongoDB.Driver;

namespace WebApi.Services;

public interface ICatalogService
{
    Task CreateAsync(CatalogItem newcatalog);
    Task<List<CatalogItem>> GetAsync();
    Task<CatalogItem?> GetAsync(string id);
    Task RemoveAsync(string id);
    Task UpdateAsync(string id, CatalogItem updatedcatalog);
}

public class CatalogService : ICatalogService
{
    private readonly IMongoCollection<CatalogItem> _catalogCollection;
    private readonly ILogger<CatalogItem> _logger;

    public CatalogService(
        IOptions<CatalogStoreDatabaseSettings> catalogStoreDatabaseSettings,
        ILogger<CatalogItem> logger)
    {
        var mongoClient = new MongoClient(
            catalogStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            catalogStoreDatabaseSettings.Value.DatabaseName);

        _catalogCollection = mongoDatabase.GetCollection<CatalogItem>(
            catalogStoreDatabaseSettings.Value.CatalogCollectionName);

        _logger = logger;
    }

    public async Task<List<CatalogItem>> GetAsync() =>
        await _catalogCollection.Find(ci => ci.IsEnabled == true).ToListAsync();

    // public async Task<List<CatalogItem>> GetAsync() {
        
    //     var result = new List<CatalogItem>() {
    //         new CatalogItem { Id = System.Guid.NewGuid().ToString(), Sku = "001", ProductName = "Alfalfa", IsInStock = true, IsEnabled = true},
    //         new CatalogItem { Id = System.Guid.NewGuid().ToString(), Sku = "002", ProductName = "Albahaca", IsInStock = true, IsEnabled = true},
    //     };

    //     _logger.LogInformation($"Listing Catalog Products... {result.Count()}");

    //     return await Task.FromResult(result);
    // }

    public async Task<CatalogItem?> GetAsync(string id) =>
        await _catalogCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(CatalogItem newcatalog) =>
        await _catalogCollection.InsertOneAsync(newcatalog);

    public async Task UpdateAsync(string id, CatalogItem updatedcatalog) =>
        await _catalogCollection.ReplaceOneAsync(x => x.Id == id, updatedcatalog);

    public async Task RemoveAsync(string id) =>
        await _catalogCollection.DeleteOneAsync(x => x.Id == id);
}