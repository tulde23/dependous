using ExamplesCommon;
using Microsoft.AspNetCore.Mvc;

namespace MinimalWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMyService myService;
    private readonly MySingleton mySingleton;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMyService myService, MySingleton mySingleton)
    {
        _logger = logger;
        this.myService = myService;
        this.mySingleton = mySingleton;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var message = await this.myService.ExecuteAsync();
        var debug = this.mySingleton.ScanResults.ToString();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)],
            Message = $"{message} {debug}"
        })
        .ToArray();
    }
}
