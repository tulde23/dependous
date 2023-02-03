using Dependous;
using Dependous.Autofac;
using ExamplesCommon;

namespace ConsoleExample
{
    [Target(typeof(IWeatherService)), NamedDependency(DependencyNames.Accuweather)]
    public class AccuweatherService : IWeatherService, ISingletonDependency
    {
        public Task<string> GetForecastAsync(string location)
        {
            return Task.FromResult($"The forecase for: {location} is Sunny 58 degrees. Forcast provider by {DependencyNames.Accuweather}");
        }
    }

    [Target(typeof(IWeatherService)), NamedDependency(DependencyNames.WeatherChannel)]
    public class WeatherChannelService : IWeatherService, ISingletonDependency
    {
        public Task<string> GetForecastAsync(string location)
        {
            return Task.FromResult($"The forecast for: {location} is Raining 47 degrees.  Forcast provider by {DependencyNames.WeatherChannel}");
        }
    }


    public class PostalCodeService : IPostalCodeService, ISingletonDependency
    {
        public bool IsValidAsync(string code)
        {
            return true;
        }
    }

    public class MethodCallInterceptorDecorator : IPostalCodeService, IDecorator<IPostalCodeService>, ISingletonDependency
    {
        private readonly IPostalCodeService trueService;

        public MethodCallInterceptorDecorator(IPostalCodeService trueService)
        {
            this.trueService = trueService;
        }

        public bool IsValidAsync(string code)
        {
            Console.WriteLine("Before GetForecastAsync");
            var result = this.trueService.IsValidAsync(code);
            Console.WriteLine("After GetForecastAsync");
            return result;
        }
    }
}