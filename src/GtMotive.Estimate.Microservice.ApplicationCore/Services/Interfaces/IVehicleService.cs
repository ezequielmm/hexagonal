using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Services.Interfaces
{
    /// <summary>
    /// Provides methods to manage vehicles.
    /// </summary>
    public interface IVehicleService
    {
        /// <summary>
        /// Gets the list of available vehicles.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of available vehicles.</returns>
        Task<List<Vehicle>> GetAvailableVehiclesAsync();

        /// <summary>
        /// Adds a new vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddVehicleAsync(Vehicle vehicle);

        /// <summary>
        /// Rents a vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to rent.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RentVehicleAsync(Guid vehicleId);

        /// <summary>
        /// Returns a rented vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to return.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ReturnVehicleAsync(Guid vehicleId);
    }
}
