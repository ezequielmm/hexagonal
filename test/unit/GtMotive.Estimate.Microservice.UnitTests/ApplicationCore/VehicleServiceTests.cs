using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Services;
using GtMotive.Estimate.Microservice.Domain.Entities;
using Moq;
using Xunit;

namespace UnitTests
{
    /// <summary>
    /// Pruebas unitarias para la clase VehicleService.
    /// </summary>
    public class VehicleServiceTests
    {
        /// <summary>
        /// Verifica que se lance una excepción cuando se intenta agregar un vehículo con más de 5 años de antigüedad.
        /// </summary>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        [Fact]
        public async Task AddVehicleShouldThrowExceptionWhenVehicleIsOlderThan5Years()
        {
            // Arrange
            var mockRepo = new Mock<IVehicleRepository>(); // Mock de la interfaz
            var service = new VehicleService(mockRepo.Object);

            var vehicle = new Vehicle { Year = DateTime.Now.Year - 6 };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddVehicleAsync(vehicle));
        }
    }
}
