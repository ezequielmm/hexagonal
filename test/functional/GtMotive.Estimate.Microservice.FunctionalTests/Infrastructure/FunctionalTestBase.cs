using System.Threading.Tasks;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure
{
    [Collection(TestCollections.Functional)]
    internal abstract class FunctionalTestBase(CompositionRootTestFixture fixture) : IAsyncLifetime
    {
        public const int QueueWaitingTimeInMilliseconds = 1000;

        protected CompositionRootTestFixture Fixture { get; } = fixture;

        public async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }
    }
}
