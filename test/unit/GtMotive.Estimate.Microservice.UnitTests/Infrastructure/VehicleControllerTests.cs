using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Controllers;
using GtMotive.Estimate.Microservice.ApplicationCore.Services;
using GtMotive.Estimate.Microservice.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace InfrastructureTests
{
    /// <summary>
    /// Unit tests for the Controller class.
    /// </summary>
    public class VehicleControllerTests
    {
        /// <summary>
        /// Tests that get available cars.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [Fact]
        public async Task GetAvailableVehiclesShouldReturnOk()
        {
            // Arrange
            var mockRepository = new Mock<IVehicleRepository>(); // Mock of IVehicleRepository
            mockRepository.Setup(r => r.GetAvailableVehiclesAsync()).ReturnsAsync([]);

            var vehicleService = new VehicleService(mockRepository.Object); // Create VehicleService with the mock of IVehicleRepository
            var controller = new VehicleController(vehicleService); // Pass VehicleService to the controller

            // Act
            var result = await controller.GetAvailableVehicles();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
