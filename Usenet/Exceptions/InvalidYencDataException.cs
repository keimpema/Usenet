using System;

namespace Usenet.Exceptions
{
    /// <inheritdoc />
    /// <summary>The exception that is thrown when a data stream or text is not in a valid yEnc format.</summary>
    public class InvalidYencDataException : Exception
    {
        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="InvalidYencDataException" /> class.</summary>
        public InvalidYencDataException()
        {
        }

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="InvalidYencDataException" /> 
        /// class with a specified error message.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InvalidYencDataException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="InvalidYencDataException" /> 
        /// class with a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. 
        /// If the <paramref name="innerException" /> parameter is not null, the current exception is 
        /// raised in a catch block that handles the inner exception.</param>
        public InvalidYencDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
