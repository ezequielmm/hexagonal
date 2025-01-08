using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure
{
    [CollectionDefinition(TestCollections.TestServer)]
    public class TestServerCollectionFixture : ICollectionFixture<GenericInfrastructureTestServerFixture>
    {
    }
}
