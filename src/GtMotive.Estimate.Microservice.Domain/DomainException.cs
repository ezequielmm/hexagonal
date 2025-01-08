using System;
using System.Diagnostics.CodeAnalysis;

[assembly: CLSCompliant(false)]

namespace GtMotive.Estimate.Microservice.Domain
{
    /// <summary>
    /// Domain Exception.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DomainException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        public DomainException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        public DomainException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
