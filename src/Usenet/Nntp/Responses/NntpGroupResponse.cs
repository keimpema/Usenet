using Usenet.Nntp.Models;

namespace Usenet.Nntp.Responses
{
    /// <summary>
    /// Represents a response to the 
    /// <a href="https://tools.ietf.org/html/rfc3977#section-6.1.1">GROUP</a> and 
    /// <a href="https://tools.ietf.org/html/rfc3977#section-6.1.2">LISTGROUP</a> commands.
    /// </summary>
    public class NntpGroupResponse : NntpResponse
    {
        /// <summary>
        /// The <see cref="NntpGroup"/> received from the server.
        /// </summary>
        public NntpGroup Group { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NntpGroupResponse"/> class.
        /// </summary>
        /// <param name="code">The response code received from the server.</param>
        /// <param name="message">The response message received from the server.</param>
        /// <param name="success">A value indicating whether the command succeeded or failed.</param>
        /// <param name="group">The <see cref="NntpGroup"/> received from the server.</param>
        public NntpGroupResponse(int code, string message, bool success, NntpGroup group) 
            : base(code, message, success)
        {
            Group = group;
        }
    }
}
