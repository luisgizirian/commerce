using WebClient.Services;
using Dapr.Client;
using Microsoft.Extensions.Configuration;

public interface ICmmrcApi
{
    Task<IEnumerable<WeatherForecast>> GetCustomers();
}

public class CmmrcApi : ICmmrcApi
{
    private readonly DaprClient _daprClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CmmrcApi> _logger;

    public CmmrcApi(
        IConfiguration configuration,
        DaprClient daprClient,
        ILogger<CmmrcApi> logger)
    {
        _configuration = configuration;
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task<IEnumerable<WeatherForecast>> GetCustomers()
    {
        _logger.LogWarning("Invoking API and Log Sink testing.");
        return await _daprClient.InvokeMethodAsync<IEnumerable<WeatherForecast>>(
            HttpMethod.Get,
            _configuration["API_APP_ID"],
            "weatherforecast");
    }
}