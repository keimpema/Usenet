namespace Usenet.Nntp.Responses
{
    /// <summary>
    /// Represents a single-line response.
    /// </summary>
    public class NntpResponse
    {
        /// <summary>
        /// The response code received from the server.
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// The response message received from the server.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// A value indicating whether the command succeeded or failed.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NntpResponse"/> class.
        /// </summary>
        /// <param name="code">The response code received from the server.</param>
        /// <param name="message">The response message received from the server.</param>
        /// <param name="success">A value indicating whether the command succeeded or failed.</param>
        public NntpResponse(int code, string message, bool success)
        {
            Code = code;
            Message = message;
            Success = success;
        }
    }
}