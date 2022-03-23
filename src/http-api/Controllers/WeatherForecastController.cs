using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly DaprClient _daprClient;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(
        DaprClient daprClient,
        ILogger<WeatherForecastController> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var result =  Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToList();

        var stored = await _daprClient.GetStateAsync<string>("cmmrc-statestore", "pa");

        if (stored == null) {
            stored = "_default_";
            await _daprClient.SaveStateAsync<string>("cmmrc-statestore", "pa", stored);
        }

        result.Add(new WeatherForecast { Date = DateTime.UtcNow, TemperatureC = 0, Summary = stored});

        return result.ToArray();
    }
}
