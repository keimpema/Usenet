using System;

namespace Usenet.Exceptions
{
    /// <inheritdoc />
    /// <summary>The exception that is thrown when communicating using the Network News Transfer Protocol.</summary>
    public class NntpException : Exception
    {
        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="NntpException" /> class.</summary>
        public NntpException()
        {
        }

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="NntpException" /> 
        /// class with a specified error message.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public NntpException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="NntpException" /> 
        /// class with a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. 
        /// If the <paramref name="innerException" /> parameter is not null, the current exception is 
        /// raised in a catch block that handles the inner exception.</param>
        public NntpException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
