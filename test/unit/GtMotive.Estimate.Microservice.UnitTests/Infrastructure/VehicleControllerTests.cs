using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Controllers;
using GtMotive.Estimate.Microservice.ApplicationCore.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace InfrastructureTests
{
    /// <summary>
    /// Pruebas unitarias para la clase Controller.
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
            var mockRepository = new Mock<IVehicleRepository>(); // Mock de IVehicleRepository
            mockRepository.Setup(r => r.GetAvailableVehiclesAsync()).ReturnsAsync([]);

            var vehicleService = new VehicleService(mockRepository.Object); // Crear VehicleService con el mock de IVehicleRepository
            var controller = new VehicleController(vehicleService); // Pasar VehicleService al controlador

            // Act
            var result = await controller.GetAvailableVehicles();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
