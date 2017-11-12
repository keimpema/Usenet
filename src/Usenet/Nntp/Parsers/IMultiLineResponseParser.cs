using System.Collections.Generic;

namespace Usenet.Nntp.Parsers
{
    /// <summary>
    /// Represents a multi-line response parser.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public interface IMultiLineResponseParser<out TResponse>
    {
        /// <summary>
        /// Determines if the received response code is valid.
        /// </summary>
        /// <param name="code">The response code received from the server.</param>
        /// <returns>true if <paramref name="code"/> is valid; otherwise false.</returns>
        bool IsSuccessResponse(int code);

        /// <summary>
        /// Parses the multi-line response of an NNTP command into a new instance of type <typeparamref name="TResponse"/>.
        /// </summary>
        /// <param name="code">The response code received from the server.</param>
        /// <param name="message">The response message received from the server.</param>
        /// <param name="dataBlock">The multi-line datablock as received from the server.</param>
        /// <returns>A new instance of type <typeparamref name="TResponse"/>.</returns>
        TResponse Parse(int code, string message, IEnumerable<string> dataBlock);
    }
}
