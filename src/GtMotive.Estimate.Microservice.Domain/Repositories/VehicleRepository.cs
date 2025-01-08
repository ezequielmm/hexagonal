using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Repositories.Interfaces;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Domain.Repositories
{
    /// <summary>
    /// Repository for managing vehicles in MongoDB.
    /// </summary>
    public class VehicleRepository : IVehicleRepository
    {
        private readonly IMongoCollection<Vehicle> _vehicles;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleRepository"/> class.
        /// </summary>
        /// <param name="database">The MongoDB database.</param>
        public VehicleRepository(IMongoDatabase database)
        {
            if (database != null)
            {
                _vehicles = database.GetCollection<Vehicle>("Vehicles");
            }
        }

        /// <summary>
        /// Gets all available vehicles.
        /// </summary>
        /// <returns>A list of available vehicles.</returns>
        public async Task<List<Vehicle>> GetAvailableVehiclesAsync()
        {
            return await _vehicles.Find(v => v.IsAvailable).ToListAsync();
        }

        /// <summary>
        /// Adds a new vehicle to the fleet.
        /// </summary>
        /// <param name="vehicle">The vehicle to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            await _vehicles.InsertOneAsync(vehicle);
        }

        /// <summary>
        /// Rents a vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to rent.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RentVehicleAsync(Guid vehicleId)
        {
            var filter = Builders<Vehicle>.Filter.Eq(v => v.Id, vehicleId);
            var update = Builders<Vehicle>.Update.Set(v => v.IsAvailable, false);
            await _vehicles.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Returns a rented vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to return.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ReturnVehicleAsync(Guid vehicleId)
        {
            var filter = Builders<Vehicle>.Filter.Eq(v => v.Id, vehicleId);
            var update = Builders<Vehicle>.Update.Set(v => v.IsAvailable, true);
            await _vehicles.UpdateOneAsync(filter, update);
        }
    }
}
