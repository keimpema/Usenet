using System;

namespace Usenet.Nntp.Responses
{
    /// <summary>
    /// Represents a response to the 
    /// <a href="https://tools.ietf.org/html/rfc3977#section-7.1">DATE</a> command.
    /// </summary>
    public class NntpDateResponse : NntpResponse
    {
        /// <summary>
        /// The date and time received from the server.
        /// </summary>
        public DateTimeOffset DateTime { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NntpDateResponse"/> class.
        /// </summary>
        /// <param name="code">The response code received from the server.</param>
        /// <param name="message">The response message received from the server.</param>
        /// <param name="success">A value indicating whether the command succeeded or failed.</param>
        /// <param name="dateTime">The date and time received from the server.</param>
        public NntpDateResponse(int code, string message, bool success, DateTimeOffset dateTime) 
            : base(code, message, success)
        {
            DateTime = dateTime;
        }
    }
}
