using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Services.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Repositories.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Services
{
    /// <summary>
    /// Provides services for managing vehicles.
    /// </summary>
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleService"/> class.
        /// </summary>
        /// <param name="vehicleRepository">The vehicle repository.</param>
        public VehicleService(IVehicleRepository vehicleRepository)
        {
            if (vehicleRepository != null)
            {
                _vehicleRepository = vehicleRepository;
            }
        }

        /// <summary>
        /// Gets the list of available vehicles asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of available vehicles.</returns>
        public async Task<List<Vehicle>> GetAvailableVehiclesAsync()
        {
            return await _vehicleRepository.GetAvailableVehiclesAsync();
        }

        /// <summary>
        /// Adds a new vehicle asynchronously.
        /// </summary>
        /// <param name="vehicle">The vehicle to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the vehicle is more than 5 years old.</exception>
        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            if (vehicle != null)
            {
                if (DateTime.Now.Year - vehicle.Year > 5)
                {
                    throw new InvalidOperationException("The vehicle cannot be more than 5 years old.");
                }

                await _vehicleRepository.AddVehicleAsync(vehicle);
            }
        }

        /// <summary>
        /// Rents a vehicle asynchronously.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to rent.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task RentVehicleAsync(Guid vehicleId)
        {
            await _vehicleRepository.RentVehicleAsync(vehicleId);
        }

        /// <summary>
        /// Returns a rented vehicle asynchronously.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to return.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ReturnVehicleAsync(Guid vehicleId)
        {
            await _vehicleRepository.ReturnVehicleAsync(vehicleId);
        }
    }
}
