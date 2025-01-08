using System;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// This type eliminates the need to depend directly on the ASP.NET Core logging types.
    /// </summary>
    /// <typeparam name="T">The type who's name is used for the logger category name.</typeparam>
    public interface IAppLogger<T>
    {
        /// <summary>
        /// Formats and writes an informational log message.
        /// </summary>
        /// <param name="message">Format string of the log message in message template format. Example: "User {User}
        /// logged in from {Address}".</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void LogInformation(string message, params object[] args);

        /// <summary>
        /// Formats and writes a warning log message.
        /// </summary>
        /// <param name="message">Format string of the log message in message template format. Example: "User {User}
        /// logged in from {Address}".</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void LogWarning(string message, params object[] args);

        /// <summary>
        /// Formats and writes a error log message.
        /// </summary>
        /// <param name="exception">Exception captured.</param>
        /// <param name="message">Format string of the log message in message template format. Example: "User {User}
        /// logged in from {Address}".</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void LogError(Exception exception, string message, params object[] args);

        /// <summary>
        /// Formats and writes a debug log message.
        /// </summary>
        /// <param name="message">Format string of the log message in message template format. Example: "User {User}
        /// logged in from {Address}".</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        void LogDebug(string message, params object[] args);
    }
}
