
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models;

public class CatalogItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; } = null!;

    [BsonElement("Sku")]
    public string Sku { get; set; } = null!;

    public string? Description { set; get; }

    public List<Price> Prices { get; set; } = new List<Price>()!;

    public List<Tag> Tags { get; set; } = new List<Tag>();
    public List<Prop> Props { get; set; } = new List<Prop>();

    public bool IsUnavailable { get; set; } = false!;

    public bool IsEnabled { get; set; } = false!;
}

public class Tag
{
    public TagKind Kind { get; set; }
    public string Name { get; set; } = null!;
}

public enum TagKind
{
    Category,
    Brand,
}

public class Prop
{
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
}

public class Price
{
    public string List {get; set;} = null!;
    public decimal Regular { get; set; }
    public decimal? Sale { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
}