using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace GtMotive.Estimate.Microservice.Api.Authorization
{
    [ExcludeFromCodeCoverage]
    public static class AuthorizationOptionsExtensions
    {
        public static void Configure(AuthorizationOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);
        }
    }
}
