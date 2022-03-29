namespace WebApi.Models;

public class CatalogStoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string CatalogCollectionName { get; set; } = null!;
}