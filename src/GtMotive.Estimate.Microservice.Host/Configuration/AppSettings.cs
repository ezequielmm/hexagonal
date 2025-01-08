using System;

[assembly: CLSCompliant(false)]

namespace GtMotive.Estimate.Microservice.Host.Configuration
{
    internal sealed class AppSettings
    {
        public string JwtAuthority { get; set; }
    }
}
