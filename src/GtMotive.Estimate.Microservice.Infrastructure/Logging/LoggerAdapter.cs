using System;
using System.Diagnostics.CodeAnalysis;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace GtMotive.Estimate.Microservice.Infrastructure.Logging
{
    /// <summary>
    /// Class to implement the IAppLogger interface.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    public class LoggerAdapter<T> : IAppLogger<T>
    {
        /// <summary>
        /// Logger's interface.
        /// </summary>
        private readonly ILogger<T> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerAdapter{T}"/> class.
        /// </summary>
        /// <param name="loggerFactory">Type used to configure the logging system and create instances of ILogger.</param>
        public LoggerAdapter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<T>();
        }

        /// <summary>
        /// Formats and writes a warning log message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">Array that contains zero or more objects to format.</param>
        [ExcludeFromCodeCoverage]
        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        /// <summary>
        /// Formats and writes an informational log message.
        /// </summary>
        /// <param name="message">A message.</param>
        /// <param name="args">Array that contains zero or more objects to format.</param>
        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        /// <summary>
        /// Formats and writes an error log message.
        /// </summary>
        /// <param name="exception">A exception.</param>
        /// <param name="message">A message.</param>
        /// <param name="args">Array that contains zero or more objects to format.</param>
        [ExcludeFromCodeCoverage]
        public void LogError(Exception exception, string message, params object[] args)
        {
            _logger.LogError(exception, message, args);
        }

        /// <summary>
        /// Formats and writes a debug log message.
        /// </summary>
        /// <param name="message">A message.</param>
        /// <param name="args">Array that contains zero or more objects to format.</param>
        public void LogDebug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);
        }
    }
}
