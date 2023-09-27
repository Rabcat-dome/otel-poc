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


            try
            {
                Thread.Sleep(2000);
                var result = InnerFunctionApi1();

                //no need to do it in real life!
                myActivity?.AddEvent(new("http 200 Event before 1s"));
                //Set Status After Finished
                myActivity?.SetStatus(ActivityStatusCode.Ok, "Normal Case");
                Thread.Sleep(1000);
                
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

        private IEnumerable<WeatherForecast> InnerFunctionApi1()
        {
            using var innerFunctionApi1Activity = Telemetry.MyActivitySource.StartActivity("InnerFunctionApi1Activity");



            Thread.Sleep(2000);
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