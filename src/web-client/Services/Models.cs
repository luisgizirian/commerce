namespace WebClient.Services;

public record CatalogItem(string Id, string Name, string Sku, string? Description, List<Price> Prices, List<Tag> Tags, List<Prop> Props, bool IsUnavailable, bool IsEnabled);

public record Price(string List, decimal Regular, decimal? Sale);

public record Prop(string Name, string Value);

public record Tag (TagKind Kind, string Name);

public enum TagKind
{
    Category,
    Brand,
}