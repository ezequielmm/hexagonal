using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure
{
    [CollectionDefinition(TestCollections.Functional)]
    public class CompositionRootCollectionFixture : ICollectionFixture<CompositionRootTestFixture>
    {
    }
}
