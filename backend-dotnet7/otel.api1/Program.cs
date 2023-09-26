
using System.Diagnostics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace otel.api
{
    public class Program
    {
        private const string ServiceName = "OpenTelemetryPoc.Backend.API1";
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
                    b
                     .AddConsoleExporter()
                     .AddSource(ServiceName)
                     .ConfigureResource(resource =>
                         resource.AddService(
                             serviceName: ServiceName,
                             serviceVersion: ServiceVersion));
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