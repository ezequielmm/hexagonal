using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GtMotive.Estimate.Microservice.Api.Filters
{
    [ExcludeFromCodeCoverage]
    public class InternalServerErrorObjectResult : ObjectResult
    {
        private const int DefaultStatusCode = StatusCodes.Status500InternalServerError;

        public InternalServerErrorObjectResult([ActionResultObjectValue] object error)
            : base(error)
        {
            StatusCode = DefaultStatusCode;
        }

        public InternalServerErrorObjectResult([ActionResultObjectValue] ModelStateDictionary modelState)
            : base(new SerializableError(modelState))
        {
            ArgumentNullException.ThrowIfNull(modelState);

            StatusCode = DefaultStatusCode;
        }
    }
}
