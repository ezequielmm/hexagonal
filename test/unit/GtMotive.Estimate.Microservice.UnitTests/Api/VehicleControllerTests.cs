using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Controllers;
using GtMotive.Estimate.Microservice.ApplicationCore.Services;
using GtMotive.Estimate.Microservice.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Tests
{
    /// <summary>
    /// Pruebas unitarias para la clase Api controller.
    /// </summary>
    public class VehicleControllerTests
    {
        /// <summary>
        /// Prueba que GetAvailableVehicles devuelva un resultado Ok con los vehículos disponibles.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetAvailableVehiclesShouldReturnOkWithVehicles()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>(); // Mock del servicio
            var availableVehicles = new List<Vehicle>
            {
                new() { Id = Guid.NewGuid(), Year = 2020, IsAvailable = true },
                new() { Id = Guid.NewGuid(), Year = 2021, IsAvailable = true }
            };

            // Configurar el mock para devolver la lista de vehículos disponibles
            mockService.Setup(service => service.GetAvailableVehiclesAsync())
                       .ReturnsAsync(availableVehicles);

            var controller = new VehicleController(mockService.Object); // Inyectar el mock en el controlador

            // Act
            var result = await controller.GetAvailableVehicles();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Verificar que el resultado es OkObjectResult
            var vehicles = Assert.IsType<List<Vehicle>>(okResult.Value); // Verificar que el valor es una lista de vehículos
            Assert.Equal(2, vehicles.Count); // Verificar que hay 2 vehículos disponibles
        }
    }
}
