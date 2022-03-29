namespace WebClient.Services;

public record WeatherForecast(DateTime Date, int TemperatureC, int TemperatureF, string? Summary);

public record CatalogItem(string Id, string ProductName, string Sku, decimal Price, bool IsInStock);