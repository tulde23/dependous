using System.Collections.Concurrent;
using Autofac.Features.Indexed;
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
    private readonly IEnumerable<IWeatherService> weatherServices;
    private readonly IIndex<DependencyNames, IWeatherService> serviceMap;
    private readonly IPostalCodeService postalCodeService;
    private readonly IMyService myService;
    private readonly MySingleton mySingleton;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IEnumerable<IWeatherService> weatherServices, 
        IIndex<DependencyNames, IWeatherService> serviceMap,
        IPostalCodeService postalCodeService)
    {
        _logger = logger;
        this.weatherServices = weatherServices;
        this.serviceMap = serviceMap;
        this.postalCodeService = postalCodeService;
        this.myService = myService;
        this.mySingleton = mySingleton;
    }

    /// <summary>
    /// In this example, we show how we can invoke all implementations of IWeatherService.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<string>> Get()
    {
        var isValid = this.postalCodeService.IsValidAsync("30030");

        var state = new ConcurrentQueue<string>();
        await Parallel.ForEachAsync(weatherServices, async (item, token) =>
        {
            var forecast = await item.GetForecastAsync("Atlanta");
            state.Enqueue(forecast);
        });
        return state.ToList();
    }

    /// <summary>
    /// In this example, we show how to target a specific "named" dependency by a key.  Keys can by an object type.  I prefer to use strings or enums.
    /// </summary>
    /// <param name="dependencyNames">The dependency names.</param>
    /// <returns></returns>
    [HttpGet("byProvider")]
    public async Task<string> Get([FromQuery] DependencyNames dependencyNames)
    {
        if (serviceMap.TryGetValue(dependencyNames, out var service))
        {
            return await service.GetForecastAsync("Atlanta");
        }
        return null;
    }
}