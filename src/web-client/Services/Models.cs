namespace WebClient.Services;

public record CatalogItem(string Id, string ProductName, string Sku, decimal Price, bool IsInStock);