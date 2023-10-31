using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using otel.api;
using otel.models;

namespace otel.api1.Controllers
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
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            foreach (var o in Request.Headers)
            {
                Console.WriteLine(o.Key + ": "+ o.Value);
            }

            using var myActivity = Telemetry.MyActivitySource1.StartActivity("Get");

            try
            {
                Thread.Sleep(200);
                var result = await CallExternalFromApi1();

                //no need to do it in real life!
                myActivity?.AddEvent(new("http 200 Event before 1s"));
                //Set Status After Finished
                myActivity?.SetStatus(ActivityStatusCode.Ok, "Normal Case");
                Thread.Sleep(100);

                return result;
            }
            catch
            {
                //track exception event
                myActivity?.AddEvent(new("Exception Event"));
                //Set Status After Finished
                myActivity?.SetStatus(ActivityStatusCode.Error, "Something bad happened!");
                Thread.Sleep(1000);
                throw;
            }
        }

        private async Task<IEnumerable<WeatherForecast>> CallExternalFromApi1()
        {
            using var innerFunctionApi1Activity = Telemetry.MyActivitySource1.StartActivity("CallExternalFromApi1");
            using HttpClient client = new();

            Thread.Sleep(500);
            var response = await client.GetFromJsonAsync<IEnumerable<WeatherForecast>>("http://web-api2/Tele2");

            return response ?? new List<WeatherForecast>();
        }
    }
}