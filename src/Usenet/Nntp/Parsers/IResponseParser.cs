namespace Usenet.Nntp.Parsers
{
    /// <summary>
    /// Represents a single-line response parser.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public interface IResponseParser<out TResponse>
    {
        /// <summary>
        /// Determines if the received response code is valid.
        /// </summary>
        /// <param name="code">The response code received from the server.</param>
        /// <returns>true if <paramref name="code"/> is valid; otherwise false.</returns>
        bool IsSuccessResponse(int code);

        /// <summary>
        /// Parses the response of an NNTP command into a new instance of type <see cref="TResponse"/>.
        /// </summary>
        /// <param name="code">The response code received from the server.</param>
        /// <param name="message">The response message received from the server.</param>
        /// <returns>A new instance of type <see cref="TResponse"/>.</returns>

        TResponse Parse(int code, string message);
    }
}
