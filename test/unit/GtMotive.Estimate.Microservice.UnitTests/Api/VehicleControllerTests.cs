using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Controllers;
using GtMotive.Estimate.Microservice.ApplicationCore.Services.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Tests
{
    /// <summary>
    /// Unit tests for the Api controller class.
    /// </summary>
    public class VehicleControllerTests
    {
        /// <summary>
        /// Tests that GetAvailableVehicles returns an Ok result with the available vehicles.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetAvailableVehiclesShouldReturnOkWithVehicles()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>(); // Mock the service
            var availableVehicles = new List<Vehicle>
            {
                new() { Id = Guid.NewGuid(), Year = 2020, IsAvailable = true },
                new() { Id = Guid.NewGuid(), Year = 2021, IsAvailable = true }
            };

            // Configure the mock to return the list of available vehicles
            mockService.Setup(service => service.GetAvailableVehiclesAsync())
                       .ReturnsAsync(availableVehicles);

            var controller = new VehicleController(mockService.Object); // Inject the mock into the controller

            // Act
            var result = await controller.GetAvailableVehicles();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Verify that the result is OkObjectResult
            var vehicles = Assert.IsType<List<Vehicle>>(okResult.Value); // Verify that the value is a list of vehicles
            Assert.Equal(2, vehicles.Count); // Verify that there are 2 available vehicles
        }
    }
}
