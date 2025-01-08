using System;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// Represents a vehicle in the rental fleet.
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// Gets or sets the unique identifier of the vehicle.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the make of the vehicle.
        /// </summary>
        public string Make { get; set; }

        /// <summary>
        /// Gets or sets the model of the vehicle.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the manufacturing year of the vehicle.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle is available for rental.
        /// </summary>
        public bool IsAvailable { get; set; } = true;
    }
}
