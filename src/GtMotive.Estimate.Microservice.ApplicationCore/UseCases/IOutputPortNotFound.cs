namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Interface to define the Not Found Output Port.
    /// </summary>
    public interface IOutputPortNotFound
    {
        /// <summary>
        /// Informs the resource was not found.
        /// </summary>
        /// <param name="message">Text description.</param>
        void NotFoundHandle(string message);
    }
}
