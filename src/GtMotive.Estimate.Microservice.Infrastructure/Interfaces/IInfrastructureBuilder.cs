using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Infrastructure.Interfaces
{
    public interface IInfrastructureBuilder
    {
        IServiceCollection Services { get; }
    }
}
