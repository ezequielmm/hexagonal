using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Services;
using GtMotive.Estimate.Microservice.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace FunctionalTests
{
    /// <summary>
    /// Pruebas unitarias para la clase VehicleService.
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
            var mockCollection = new Mock<IMongoCollection<Vehicle>>(); // Mock de IMongoCollection<Vehicle>
            var mockDatabase = new Mock<IMongoDatabase>(); // Mock de IMongoDatabase

            // Configurar el mock de IMongoDatabase para devolver el mock de IMongoCollection<Vehicle>
            mockDatabase
                .Setup(db => db.GetCollection<Vehicle>(It.IsAny<string>(), null))
                .Returns(mockCollection.Object);

            // Configurar el mock de IMongoCollection<Vehicle> para simular FindAsync
            var mockCursor = new Mock<IAsyncCursor<Vehicle>>();
            var vehicles = new List<Vehicle>(); // Lista vacía de vehículos

            mockCursor.Setup(c => c.Current).Returns(vehicles); // Devuelve la lista vacía
            mockCursor.Setup(c => c.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false); // No hay más datos

            mockCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<Vehicle>>(),
                    It.IsAny<FindOptions<Vehicle, Vehicle>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            var host = new HostBuilder()
                .ConfigureServices(services =>
                {
                    // Registrar IMongoDatabase (mock)
                    services.AddSingleton(mockDatabase.Object);

                    // Registrar IVehicleRepository con la implementación VehicleRepository
                    services.AddScoped<IVehicleRepository, VehicleRepository>();

                    // Registrar VehicleService
                    services.AddScoped<VehicleService>();
                })
                .Build();

            var service = host.Services.GetRequiredService<VehicleService>();
            var repo = host.Services.GetRequiredService<IVehicleRepository>();

            var vehicle = new Vehicle { Id = Guid.NewGuid(), Year = DateTime.Now.Year, IsAvailable = true };

            // Configurar el mock de IMongoCollection<Vehicle> para simular AddVehicleAsync
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
