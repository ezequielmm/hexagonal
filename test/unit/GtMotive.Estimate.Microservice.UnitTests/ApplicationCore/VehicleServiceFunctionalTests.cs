using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Services;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Repositories;
using GtMotive.Estimate.Microservice.Domain.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace FunctionalTests
{
    /// <summary>
    /// Unit tests for the VehicleService class.
    /// </summary>
    public class VehicleServiceFunctionalTests
    {
        /// <summary>
        /// Tests that renting a vehicle makes it unavailable.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [Fact]
        public async Task RentVehicleShouldMakeVehicleUnavailable()
        {
            // Arrange
            var mockCollection = new Mock<IMongoCollection<Vehicle>>(); // Mock of IMongoCollection<Vehicle>
            var mockDatabase = new Mock<IMongoDatabase>(); // Mock of IMongoDatabase

            // Configure the IMongoDatabase mock to return the IMongoCollection<Vehicle> mock
            mockDatabase
                .Setup(db => db.GetCollection<Vehicle>(It.IsAny<string>(), null))
                .Returns(mockCollection.Object);

            // Configure the IMongoCollection<Vehicle> mock to simulate FindAsync
            var mockCursor = new Mock<IAsyncCursor<Vehicle>>();
            var vehicles = new List<Vehicle>(); // Empty list of vehicles

            mockCursor.Setup(c => c.Current).Returns(vehicles); // Returns the empty list
            mockCursor.Setup(c => c.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false); // No more data

            mockCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<Vehicle>>(),
                    It.IsAny<FindOptions<Vehicle, Vehicle>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            var host = new HostBuilder()
                .ConfigureServices(services =>
                {
                    // Register IMongoDatabase (mock)
                    services.AddSingleton(mockDatabase.Object);

                    // Register IVehicleRepository with the VehicleRepository implementation
                    services.AddScoped<IVehicleRepository, VehicleRepository>();

                    // Register VehicleService
                    services.AddScoped<VehicleService>();
                })
                .Build();

            var service = host.Services.GetRequiredService<VehicleService>();
            var repo = host.Services.GetRequiredService<IVehicleRepository>();

            var vehicle = new Vehicle { Id = Guid.NewGuid(), Year = DateTime.Now.Year, IsAvailable = true };

            // Configure the IMongoCollection<Vehicle> mock to simulate AddVehicleAsync
            mockCollection
                .Setup(c => c.InsertOneAsync(vehicle, null, default))
                .Returns(Task.CompletedTask);

            await repo.AddVehicleAsync(vehicle);

            // Act
            await service.RentVehicleAsync(vehicle.Id);

            // Assert
            var updatedVehicle = await repo.GetAvailableVehiclesAsync();
            Assert.DoesNotContain(updatedVehicle, v => v.Id == vehicle.Id);
        }
    }
}
