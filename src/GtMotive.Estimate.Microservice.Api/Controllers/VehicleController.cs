using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Services.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones de vehículos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController(IVehicleService vehicleService) : ControllerBase, IVehicleController
    {
        private readonly IVehicleService _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));

        /// <summary>
        /// Obtiene todos los vehículos disponibles.
        /// </summary>
        /// <returns>Una lista de vehículos disponibles.</returns>
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableVehicles()
        {
            var vehicles = await _vehicleService.GetAvailableVehiclesAsync();
            return Ok(vehicles);
        }

        /// <summary>
        /// Añade un nuevo vehículo a la flota.
        /// </summary>
        /// <param name="vehicle">El vehículo a añadir.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromBody] Vehicle vehicle)
        {
            await _vehicleService.AddVehicleAsync(vehicle);
            return Ok();
        }

        /// <summary>
        /// Alquila un vehículo.
        /// </summary>
        /// <param name="id">El identificador del vehículo a alquilar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost("{id}/rent")]
        public async Task<IActionResult> RentVehicle(Guid id)
        {
            await _vehicleService.RentVehicleAsync(id);
            return Ok();
        }

        /// <summary>
        /// Devuelve un vehículo alquilado.
        /// </summary>
        /// <param name="id">El identificador del vehículo a devolver.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost("{id}/return")]
        public async Task<IActionResult> ReturnVehicle(Guid id)
        {
            await _vehicleService.ReturnVehicleAsync(id);
            return Ok();
        }
    }
}
