using System.Collections.Generic;

namespace Usenet.Nntp.Responses
{
    /// <summary>
    /// Represents a generic multi-line response.
    /// </summary>
    public class NntpMultiLineResponse : NntpResponse
    {
        /// <summary>
        /// The lines received from the server.
        /// </summary>
        public IEnumerable<string> Lines { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NntpMultiLineResponse"/> class.
        /// </summary>
        /// <param name="code">The response code received from the server.</param>
        /// <param name="message">The response message received from the server.</param>
        /// <param name="success">A value indicating whether the command succeeded or failed.</param>
        /// <param name="lines">The lines received from the server.</param>
        public NntpMultiLineResponse(
            int code,
            string message,
            bool success,
            IEnumerable<string> lines) : base(code, message, success)
        {
            Lines = lines ?? new string[0];
        }
    }
}
