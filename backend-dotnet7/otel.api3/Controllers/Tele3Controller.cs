using Microsoft.AspNetCore.Mvc;
using otel.api;
using otel.models;
using System.Diagnostics;

namespace otel.api3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Tele3Controller : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<Tele3Controller> _logger;

        public Tele3Controller(ILogger<Tele3Controller> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAPI3")]
        public IEnumerable<WeatherForecast> Get()
        {
            using var myActivity = Telemetry.MyActivitySource3.StartActivity("Get");


            try
            {
                Thread.Sleep(2000);
                var result = InnerFunctionApi3();

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

        private IEnumerable<WeatherForecast> InnerFunctionApi3()
        {
            using var innerFunctionApi1Activity = Telemetry.MyActivitySource3.StartActivity("InnerFunctionApi3");



            Thread.Sleep(200);
            return Enumerable.Range(1, 5).Select(index =>
            {



                //Set tags to an Activity
                innerFunctionApi1Activity?.SetTag("operation.value", index);
                return new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                };
            }).ToArray(); ;
        }
    }
}