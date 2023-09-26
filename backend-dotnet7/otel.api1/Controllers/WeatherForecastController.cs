using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using otel.models;

namespace otel.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeleController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<TeleController> _logger;

        public TeleController(ILogger<TeleController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAPI1")]
        public IEnumerable<WeatherForecast> Get()
        {
            using var myActivity = Telemetry.MyActivitySource.StartActivity("GetAPI1");


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}