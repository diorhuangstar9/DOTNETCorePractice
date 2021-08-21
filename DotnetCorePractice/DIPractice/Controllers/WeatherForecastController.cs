using DIPractice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPractice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IScopedService test1Service;
        private readonly IScopedService test2Service;
        private readonly IScopedService test3Service;
        private readonly TestExecService _testExecService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            ScopedServiceFactory scopedServiceResolver, TestExecService testExecService)
        {
            _logger = logger;
            test1Service = scopedServiceResolver("test1");
            test2Service = scopedServiceResolver("test2");
            test3Service = scopedServiceResolver("test3");
            _testExecService = testExecService;
        }

        [HttpGet]
        public IActionResult Get()
        {

            var test1 = test1Service.GetScopeId();
            var test2 = test2Service.GetScopeId();
            var test3 = test3Service.GetScopeId();
            var testResult = new List<string> { test1, test2, test3 };
            var execTestResult = _testExecService.GetScopeIds();

            return Ok(new { testResult, execTestResult });
        }
    }
}
