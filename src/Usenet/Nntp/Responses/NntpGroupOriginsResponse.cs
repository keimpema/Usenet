using System.Collections.Generic;
using Usenet.Nntp.Models;

namespace Usenet.Nntp.Responses
{
    /// <summary>
    /// Represents a response to the 
    /// <a href="https://tools.ietf.org/html/rfc3977#section-7.6.4">LIST ACTIVE.TIMES</a> 
    /// (<a href="https://tools.ietf.org/html/rfc2980#section-2.1.3">ad 1</a>) command.
    /// </summary>
    public class NntpGroupOriginsResponse : NntpResponse
    {
        /// <summary>
        /// The list of <see cref="NntpGroupOrigin"/> objects received from the server.
        /// </summary>
        public IEnumerable<NntpGroupOrigin> GroupOrigins { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NntpGroupOriginsResponse"/> class.
        /// </summary>
        /// <param name="code">The response code received from the server.</param>
        /// <param name="message">The response message received from the server.</param>
        /// <param name="success">A value indicating whether the command succeeded or failed.</param>
        /// <param name="groupOrigins">The list of <see cref="NntpGroupOrigin"/> objects received from the server.</param>
        public NntpGroupOriginsResponse(int code, string message, bool success, IEnumerable<NntpGroupOrigin> groupOrigins) 
            : base(code, message, success)
        {
            GroupOrigins = groupOrigins ?? new NntpGroupOrigin[0];
        }
    }
}
