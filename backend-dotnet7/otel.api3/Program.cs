using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using otel.models.EF;

namespace otel.api3
{
    public class Program
    {
        private const string ServiceName = "OpenTelemetryPoc.Backend.API3";
        private const string ServiceVersion = "1.0.0";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDb>(options =>
                options.UseNpgsql(builder.Configuration["ConnectionStrings:AppDb"]));
            builder.Services.AddScoped<DbContext, AppDb>();

            builder.Services.AddControllers();
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
                        })
                        .AddHttpClientInstrumentation()
                        .AddSqlClientInstrumentation(options => {
                            options.EnableConnectionLevelAttributes = true;
                            options.SetDbStatementForStoredProcedure = true;
                            options.SetDbStatementForText = true;
                            options.RecordException = true;
                            options.Enrich = (activity, x, y) => activity.SetTag("db.type", "sql");
                        }).AddNpgsql();
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