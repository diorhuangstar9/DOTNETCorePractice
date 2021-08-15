using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CancellationPractice.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("CancelTest")]
        public async Task<IActionResult> CancelTestAsync(CancellationToken tokenInParam)
        {
            //var cancelToken = HttpContext.RequestAborted;
            var tokenSource = new CancellationTokenSource();
            var waitTask = Task.Delay(3000, tokenSource.Token);
            tokenSource.CancelAfter(2500);
            await waitTask;

            Console.WriteLine("CancelTest Response OK");
            return Ok("OK");
        }

        [HttpGet("CancelTestUserCode")]
        public IActionResult CancelTestUserCode(CancellationToken cancelToken)
        {
            //var cancelToken = HttpContext.RequestAborted;
            for (var i = 0; i < 40; i++)
            {
                //cancelToken.ThrowIfCancellationRequested();
                if (cancelToken.IsCancellationRequested)
                {
                    Console.WriteLine("Operation Canceled");
                    break;
                }
                Thread.Sleep(100);
            }

            Console.WriteLine("CancelTestUserCode Response OK");
            return Ok("OK");
        }

        [HttpGet("CancelTestCooperation")]
        public async Task<IActionResult> CancelTestCooperationAsync(CancellationToken cancelToken)
        {
            cancelToken.Register(() => Console.WriteLine("Callback Canceled."));
            var task = Task.Run(() =>
            {
                for (var i = 0; i < 40; i++)
                {
                    cancelToken.ThrowIfCancellationRequested();
                    //if (cancelToken.IsCancellationRequested)
                    //{
                    //    Console.WriteLine("Operation Canceled");
                    //    throw new OperationCanceledException(cancelToken);
                    //}
                    Thread.Sleep(100);
                }
            }, cancelToken);

            try
            {
                await task;
            }
            catch (OperationCanceledException opEx)
            {
                Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {opEx.Message}");
            }
            finally
            {
                Console.WriteLine($"Task Status:{task.Status}");
            }


            Console.WriteLine("CancelTestUserCode Response OK");
            return Ok("OK");
        }

        // TODO wait handle

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
