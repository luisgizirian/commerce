using WebClient.Services;
using Dapr.Client;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

public interface ICmmrcApi
{
    Task<IEnumerable<CatalogItem>> ListCatalogItems();
}

public class CmmrcApi : ICmmrcApi
{
    // TODO: enable only if State, or some other DAPR requirement at this stage.
    // private readonly DaprClient _daprClient;
    private readonly IConfiguration _configuration;

    private IHttpClientFactory _httpFactory;
    private readonly ILogger<CmmrcApi> _logger;

    public CmmrcApi(
        IConfiguration configuration,
        // DaprClient daprClient,
        IHttpClientFactory httpFactory,
        ILogger<CmmrcApi> logger)
    {
        _configuration = configuration;
        // _daprClient = daprClient;
        _httpFactory = httpFactory;
        _logger = logger;
    }

    private HttpClient Client => _httpFactory.CreateClient("c");

    public async Task<IEnumerable<CatalogItem>> ListCatalogItems()
    {
        var response = await Client.GetStringAsync("/h/catalog");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var result = JsonSerializer.Deserialize<IEnumerable<CatalogItem>>(response, options);

        return result;
    }
}