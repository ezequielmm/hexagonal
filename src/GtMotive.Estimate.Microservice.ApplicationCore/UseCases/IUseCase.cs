using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Interface for the handler of an use case.
    /// </summary>
    /// <typeparam name="TUseCaseInput">Tyoe of the input message.</typeparam>
    public interface IUseCase<in TUseCaseInput>
        where TUseCaseInput : IUseCaseInput
    {
        /// <summary>
        /// Executes the Use Case.
        /// </summary>
        /// <param name="input">Input Message.</param>
        /// <returns>Task.</returns>
        Task Execute(TUseCaseInput input);
    }
}
