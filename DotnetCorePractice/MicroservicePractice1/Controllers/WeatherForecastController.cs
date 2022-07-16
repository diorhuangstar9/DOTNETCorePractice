using MicroservicePractice1.ServiceClients;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;

namespace MicroservicePractice1.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ITestServiceClient _testServiceClient;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ITestServiceClient testServiceClient)
    {
        _logger = logger;
        _testServiceClient = testServiceClient;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [HttpGet("GetTestServiceItem")]
    public async Task<string> GetTestServiceItem()
    {
        string result;
        try
        {
            result = await _testServiceClient.GetTestServiceItem();
        }
        catch (BrokenCircuitException e)
        {
            result = HandleBrokenCircuitException();
        }
        return result;
    }
    
    private string HandleBrokenCircuitException()
    {
        return "Microservice is inoperative, please try later on. (Business message due to Circuit-Breaker)";
    }

}