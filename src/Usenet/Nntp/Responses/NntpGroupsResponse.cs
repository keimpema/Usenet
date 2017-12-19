using System.Collections.Generic;
using System.Collections.Immutable;
using Usenet.Nntp.Models;

namespace Usenet.Nntp.Responses
{
    /// <summary>
    /// Represents a response to the 
    /// <a href="https://tools.ietf.org/html/rfc6048#section-2.2">LIST COUNTS</a>, 
    /// <a href="https://tools.ietf.org/html/rfc6048#section-3">LIST ACTIVE</a> 
    /// (<a href="https://tools.ietf.org/html/rfc3977#section-7.6.3">ad 1</a>,
    /// <a href="https://tools.ietf.org/html/rfc2980#section-2.1.2">ad 2</a>) and 
    /// <a href="https://tools.ietf.org/html/rfc3977#section-7.3">NEWGROUPS</a> commands.
    /// </summary>
    public class NntpGroupsResponse : NntpResponse
    {
        /// <summary>
        /// The list of <see cref="NntpGroup"/> objects received from the server.
        /// </summary>
        public IEnumerable<NntpGroup> Groups { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NntpGroupResponse"/> class.
        /// </summary>
        /// <param name="code">The response code received from the server.</param>
        /// <param name="message">The response message received from the server.</param>
        /// <param name="success">A value indicating whether the command succeeded or failed.</param>
        /// <param name="groups">The list of <see cref="NntpGroup"/> objects received from the server.</param>
        public NntpGroupsResponse(int code, string message, bool success, IEnumerable<NntpGroup> groups) 
            : base(code, message, success)
        {
            switch (groups)
            {
                case null:
                    // create empty immutable list
                    Groups = new List<NntpGroup>(0).ToImmutableList();
                    break;

                case ICollection<NntpGroup> collection:
                    // make immutable
                    Groups = collection.ToImmutableList();
                    break;

                default:
                    // not a collection but a stream of lines, keep enumerator
                    // this is immutable already
                    Groups = groups;
                    break;
            }
        }
    }
}
