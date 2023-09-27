using System.Diagnostics;

namespace otel.models
{
    public static class Telemetry
    {
        public static readonly ActivitySource MyActivitySource1 = new ("OpenTelemetryPoc.Backend.API1");
        public static readonly ActivitySource MyActivitySource2 = new("OpenTelemetryPoc.Backend.API2");
        public static readonly ActivitySource MyActivitySource3 = new("OpenTelemetryPoc.Backend.API3");
    }

}