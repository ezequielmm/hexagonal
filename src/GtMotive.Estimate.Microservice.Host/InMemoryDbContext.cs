using GtMotive.Estimate.Microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GtMotive.Estimate.Microservice.Host
{
    /// <summary>
    /// DbContext en memoria para pruebas.
    /// </summary>
#pragma warning disable CA1515 // Considere la posibilidad de hacer que los tipos públicos sean internos
    public class InMemoryDbContext : DbContext
#pragma warning restore CA1515 // Considere la posibilidad de hacer que los tipos públicos sean internos
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryDbContext"/> class.
        /// Constructor que recibe las opciones de DbContext.
        /// </summary>
        /// <param name="options">Opciones de configuración del DbContext.</param>
#pragma warning disable IDE0290 // Usar constructor principal
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options)
#pragma warning restore IDE0290 // Usar constructor principal
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets dbSet para la entidad Vehicle.
        /// </summary>
        public DbSet<Vehicle> Vehicles { get; set; }

        /// <summary>
        /// Configuración del modelo de la base de datos.
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder != null)
            {
                base.OnModelCreating(modelBuilder);

                // Configuración de la entidad Vehicle
                modelBuilder.Entity<Vehicle>(entity =>
                {
                    entity.HasKey(v => v.Id); // Definir la clave primaria
                    entity.Property(v => v.Make).IsRequired(); // Marcar Make como requerido
                    entity.Property(v => v.Model).IsRequired(); // Marcar Model como requerido
                    entity.Property(v => v.Year).IsRequired(); // Marcar Year como requerido
                    entity.Property(v => v.IsAvailable).IsRequired(); // Marcar IsAvailable como requerido
                });
            }
        }
    }
}
