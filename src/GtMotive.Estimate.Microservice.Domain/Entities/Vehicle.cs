using System;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// Representa un vehículo en la flota de renting.
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// Gets or sets el identificador único del vehículo.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets la marca del vehículo.
        /// </summary>
        public string Make { get; set; }

        /// <summary>
        /// Gets or sets el modelo del vehículo.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets el año de fabricación del vehículo.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether el vehículo está disponible para alquiler.
        /// </summary>
        public bool IsAvailable { get; set; } = true;
    }
}
