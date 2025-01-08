using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Interfaz para gestionar las operaciones de vehículos.
    /// </summary>
    public interface IVehicleController
    {
        /// <summary>
        /// Obtiene todos los vehículos disponibles.
        /// </summary>
        /// <returns>Una lista de vehículos disponibles.</returns>
        Task<IActionResult> GetAvailableVehicles();

        /// <summary>
        /// Añade un nuevo vehículo a la flota.
        /// </summary>
        /// <param name="vehicle">El vehículo a añadir.</param>
        /// <returns>Resultado de la operación.</returns>
        Task<IActionResult> AddVehicle([FromBody] Vehicle vehicle);

        /// <summary>
        /// Alquila un vehículo.
        /// </summary>
        /// <param name="id">El identificador del vehículo a alquilar.</param>
        /// <returns>Resultado de la operación.</returns>
        Task<IActionResult> RentVehicle(Guid id);

        /// <summary>
        /// Devuelve un vehículo alquilado.
        /// </summary>
        /// <param name="id">El identificador del vehículo a devolver.</param>
        /// <returns>Resultado de la operación.</returns>
        Task<IActionResult> ReturnVehicle(Guid id);
    }
}
