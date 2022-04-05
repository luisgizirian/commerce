using WebApi.Models;
using Microsoft.Extensions.Options;
using System.Linq;
using MongoDB.Driver;

namespace WebApi.Services;

public interface ICatalogService
{
    Task CreateAsync(CatalogItem newcatalog);
    Task<List<CatalogItem>> GetAsync(bool all = false);
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

    public async Task<List<CatalogItem>> GetAsync(bool includeDisabled = false) =>
        !includeDisabled
            ? await _catalogCollection.Find(ci => ci.IsEnabled == true).ToListAsync()
            : await _catalogCollection.Find(_ => true).ToListAsync();
    

    public async Task<CatalogItem?> GetAsync(string id) =>
        await _catalogCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(CatalogItem newcatalog) =>
        await _catalogCollection.InsertOneAsync(newcatalog);

    public async Task UpdateAsync(string id, CatalogItem updatedcatalog) =>
        await _catalogCollection.ReplaceOneAsync(x => x.Id == id, updatedcatalog);

    public async Task RemoveAsync(string id) {
        var softDelete = Builders<CatalogItem>.Update.Set(x => x.IsEnabled, false);
        await _catalogCollection.UpdateOneAsync(x => x.Id == id, softDelete);
    }
}