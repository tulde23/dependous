using System;
using System.Collections.Generic;
using Dependous.WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dependous.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IService1 _service1;

        public TestController(ILogger<TestController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        [Produces("application/json")]
        public dynamic Get()
        {
            try
            {
                var s1 = _serviceProvider.GetService<IService1>();
                var s2 = _serviceProvider.GetService<IOpenGenericService<Hello>>();
                var s3 = _serviceProvider.GetService<Model>();

                return new List<string>()
                {
                    s1.GetType().Name,
                    s2.GetType().Name,
                    this._logger.GetType().Name,
                    $"{s3.GetType().Name} = {s3.Test()}"
                };
            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }
            //let's run some tests.
        }
    }
}