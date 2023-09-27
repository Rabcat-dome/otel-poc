
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace otel.api2
{
    public class Program
    {
        private const string ServiceName = "OpenTelemetryPoc.Backend.API2";
        private const string ServiceVersion = "1.0.0";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddOpenTelemetry()
                .WithTracing(b =>
                {
                    b.AddSource(ServiceName)
                        .ConfigureResource(resource =>
                            resource.AddService(
                                serviceName: ServiceName,
                                serviceVersion: ServiceVersion))
                        .AddAspNetCoreInstrumentation(o =>
                        {
                            o.EnrichWithHttpRequest = (activity, httpRequest) =>
                            {
                                activity.SetTag("requestProtocol", httpRequest.Protocol);
                            };
                            o.EnrichWithHttpResponse = (activity, httpResponse) =>
                            {
                                activity.SetTag("responseLength", httpResponse.ContentLength);
                            };
                            o.EnrichWithException = (activity, exception) =>
                            {
                                activity.SetTag("exceptionType", exception.GetType().ToString());
                            };
                        }).AddOtlpExporter(options =>
                        {
                            options.Endpoint = new Uri("http://host.docker.internal:4317"
                                                       ?? throw new InvalidOperationException());
                        }).AddHttpClientInstrumentation();
                })
                .WithMetrics(m =>
                {
                    m.AddAspNetCoreInstrumentation()
                        .AddOtlpExporter(options =>
                        {
                            options.Endpoint = new Uri("http://host.docker.internal:4317"
                                                       ?? throw new InvalidOperationException());
                        });
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}