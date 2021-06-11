using Usenet.Nntp.Parsers;
using Usenet.Nntp.Responses;
using Usenet.Util;

namespace Usenet.Nntp
{
    /// <summary>
    /// Based on Kristian Hellang's NntpLib.Net project https://github.com/khellang/NntpLib.Net.
    /// </summary>
    public partial class NntpClient
    {
        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc4643#section-2.3">AUTHINFO USER and AUTHINFO PASS</a> 
        /// (<a href="https://tools.ietf.org/html/rfc2980#section-3.1.1">ad 1</a>)
        /// commands are used to present clear text credentials to the server.
        /// </summary>
        /// <param name="username">The username to use.</param>
        /// <param name="password">The password to use.</param>
        /// <returns>true if the user was authenticated successfully; otherwise false.</returns>
        public bool Authenticate(string username, string password = null)
        {
            Guard.ThrowIfNullOrWhiteSpace(username, nameof(username));
            NntpResponse userResponse = connection.Command($"AUTHINFO USER {username}", new ResponseParser(281));
            if (userResponse.Success)
            {
                return true;
            }
            if (userResponse.Code != 381 || string.IsNullOrWhiteSpace(password))
            {
                System.Console.WriteLine($"{userResponse.Code} - {userResponse.Message}");
                return false;
            }
            
            NntpResponse passResponse = connection.Command($"AUTHINFO PASS {password}", new ResponseParser(281));
            return passResponse.Success;
        }
    }
}
