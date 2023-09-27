using Microsoft.AspNetCore.Mvc;
using otel.api;
using otel.models;
using System.Diagnostics;
using System.Net.Http;

namespace otel.api2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Tele2Controller : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<Tele2Controller> _logger;

        public Tele2Controller(ILogger<Tele2Controller> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAPI2")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            using var myActivity = Telemetry.MyActivitySource2.StartActivity("Get");


            try
            {
                Thread.Sleep(200);
                var result = await CallExternalFromApi2();

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

        private async Task<IEnumerable<WeatherForecast>> CallExternalFromApi2()
        {
            using var innerFunctionApi1Activity = Telemetry.MyActivitySource2.StartActivity("CallExternalFromApi2");
            using HttpClient client = new();

            Thread.Sleep(500);
            var response = await client.GetFromJsonAsync<IEnumerable<WeatherForecast>>("http://web-api3/Tele3");
            
            return response ?? new List<WeatherForecast>();
        }
    }
}