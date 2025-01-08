using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.Infrastructure.Telemetry
{
    [ExcludeFromCodeCoverage]
    public class NoOpTelemetry : ITelemetry
    {
        public NoOpTelemetry()
        {
        }

        public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            // Use for testing
        }

        public void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
        {
            // Use for testing
        }
    }
}
