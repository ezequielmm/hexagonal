using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Services;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Repositories.Interfaces;
using Moq;
using Xunit;

namespace UnitTests
{
    /// <summary>
    /// Unit tests for the VehicleService class.
    /// </summary>
    public class VehicleServiceTests
    {
        /// <summary>
        /// Verifies that an exception is thrown when attempting to add a vehicle older than 5 years.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [Fact]
        public async Task AddVehicleShouldThrowExceptionWhenVehicleIsOlderThan5Years()
        {
            // Arrange
            var mockRepo = new Mock<IVehicleRepository>(); // Mock of the interface
            var service = new VehicleService(mockRepo.Object);

            var vehicle = new Vehicle { Year = DateTime.Now.Year - 6 };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddVehicleAsync(vehicle));
        }
    }
}
