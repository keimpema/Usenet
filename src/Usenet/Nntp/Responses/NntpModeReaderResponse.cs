namespace Usenet.Nntp.Responses
{
    /// <summary>
    /// Represents the response to the
    /// <a href="https://tools.ietf.org/html/rfc3977#section-5.3">MODE READER</a>
    /// (<a href="https://tools.ietf.org/html/rfc2980#section-2.3">ad 1</a>) command.
    /// </summary>
    public class NntpModeReaderResponse : NntpResponse
    {
        /// <summary>
        /// The type of the response received from the server.
        /// </summary>
        public NntpModeReaderResponseType ResponseType { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NntpModeReaderResponse"/> class.
        /// </summary>
        /// <param name="code">The response code received from the server.</param>
        /// <param name="message">The response message received from the server.</param>
        /// <param name="success">A value indicating whether the command succeeded or failed.</param>
        /// <param name="responseType">The type of the response received from the server.</param>
        public NntpModeReaderResponse(int code, string message, bool success, NntpModeReaderResponseType responseType) 
            : base(code, message, success)
        {
            ResponseType = responseType;
        }
    }
}
