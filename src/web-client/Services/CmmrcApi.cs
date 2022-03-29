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
        var response = await Client.GetStringAsync("/h/catalog/list");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var result = JsonSerializer.Deserialize<IEnumerable<CatalogItem>>(response, options);

        _logger.LogInformation($"Listing Catalog Items... {result.Count()}");

        return result;
        
        // TEST: web - gw - api could be all Daprized. Check if there's any gain and measure.
        //       By now we'll leave it HTTP - HTTP - GRPC.
        //
        // return await _daprClient.InvokeMethodAsync<IEnumerable<CatalogItem>>(
        //     HttpMethod.Get,
        //     _configuration["API_APP_ID"],
        //     "/c/catalog/list");
    }
}