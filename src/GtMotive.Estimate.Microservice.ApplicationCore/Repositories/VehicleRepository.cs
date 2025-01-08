using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Services
{
    /// <summary>
    /// Repositorio para gestionar los vehículos en MongoDB.
    /// </summary>
    public class VehicleRepository : IVehicleRepository
    {
        private readonly IMongoCollection<Vehicle> _vehicles;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleRepository"/> class.
        /// Inicializa una nueva instancia de la clase <see cref="VehicleRepository"/>.
        /// </summary>
        /// <param name="database">La base de datos de MongoDB.</param>
        public VehicleRepository(IMongoDatabase database)
        {
            if (database != null)
            {
                _vehicles = database.GetCollection<Vehicle>("Vehicles");
            }
        }

        /// <summary>
        /// Obtiene todos los vehículos disponibles.
        /// </summary>
        /// <returns>Una lista de vehículos disponibles.</returns>
        public async Task<List<Vehicle>> GetAvailableVehiclesAsync()
        {
            return await _vehicles.Find(v => v.IsAvailable).ToListAsync();
        }

        /// <summary>
        /// Añade un nuevo vehículo a la flota.
        /// </summary>
        /// <param name="vehicle">El vehículo a añadir.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            await _vehicles.InsertOneAsync(vehicle);
        }

        /// <summary>
        /// Alquila un vehículo.
        /// </summary>
        /// <param name="vehicleId">El ID del vehículo a alquilar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task RentVehicleAsync(Guid vehicleId)
        {
            var filter = Builders<Vehicle>.Filter.Eq(v => v.Id, vehicleId);
            var update = Builders<Vehicle>.Update.Set(v => v.IsAvailable, false);
            await _vehicles.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Devuelve un vehículo alquilado.
        /// </summary>
        /// <param name="vehicleId">El ID del vehículo a devolver.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task ReturnVehicleAsync(Guid vehicleId)
        {
            var filter = Builders<Vehicle>.Filter.Eq(v => v.Id, vehicleId);
            var update = Builders<Vehicle>.Update.Set(v => v.IsAvailable, true);
            await _vehicles.UpdateOneAsync(filter, update);
        }
    }
}
