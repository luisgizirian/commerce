
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models;

public class CatalogItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Name")]
    public string ProductName { get; set; } = null!;

    [BsonElement("Sku")]
    public string Sku { get; set; } = null!;

    public decimal Price { get; set; } // TODO: add multi-price-list support.

    public bool IsInStock { get; set; } = false!;

    public bool IsEnabled { get; set; } = false!;
}