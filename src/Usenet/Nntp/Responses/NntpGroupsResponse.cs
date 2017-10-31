using System.Collections.Generic;
using Usenet.Nntp.Models;
using Usenet.Util;

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
        public IEnumerable<NntpGroup> Groups { get; }

        public NntpGroupsResponse(int code, string message, bool success, IEnumerable<NntpGroup> groups) 
            : base(code, message, success)
        {
            Groups = groups ?? EmptyList<NntpGroup>.Instance;
        }
    }
}
