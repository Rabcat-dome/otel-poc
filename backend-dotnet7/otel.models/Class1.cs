using System.Diagnostics;

namespace otel.models
{
    public static class Telemetry
    {
        public static readonly ActivitySource MyActivitySource = new ("OpenTelemetryPoc.Backend.API1");
    }

    public class Class1
    {

    }
}