using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamplesCommon
{
    public  interface IWeatherService
    {
        public Task<string> GetForecastAsync(string location);
    }

    public interface IPostalCodeService
    {
        public bool IsValidAsync(string code);
    }
}
