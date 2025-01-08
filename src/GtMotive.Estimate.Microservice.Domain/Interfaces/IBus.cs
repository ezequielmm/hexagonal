using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// Bus API.
    /// </summary>
    public interface IBus
    {
        /// <summary>
        /// Sends a message on the queue/topic associated to this client.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <returns>A task to be resolved on when the operation has completed.</returns>
        Task Send(object message);
    }
}
