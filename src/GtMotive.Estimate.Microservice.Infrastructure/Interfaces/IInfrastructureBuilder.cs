using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    public interface IInfrastructureBuilder
    {
        IServiceCollection Services { get; }
    }
}
