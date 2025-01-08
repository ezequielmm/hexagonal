using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GtMotive.Estimate.Microservice.Host.Infrastructure.Swagger
{
    internal sealed class IdentityServerApiSecurityOperationFilter : IOperationFilter
    {
        internal static readonly string[] OpenApiSecuritySchemesValues = ["estimate-api"];

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            ArgumentNullException.ThrowIfNull(operation);

            ArgumentNullException.ThrowIfNull(context);

            var controllerAttributes = context.MethodInfo.DeclaringType is null
                ? []
                : context.MethodInfo.DeclaringType
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .ToArray();

            var methodAttributes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .ToArray();

            var attributes = controllerAttributes.Union(methodAttributes).ToArray();

            if (attributes.Length != 0)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security =
                [
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "oauth2"
                                }
                            },
                            OpenApiSecuritySchemesValues
                        }
                    }

                ];
            }
        }
    }
}
