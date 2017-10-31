using Usenet.Extensions;

namespace Usenet.Nntp
{
    /// <summary>
    /// An NNTP client that is compliant with
    /// <a href="https://tools.ietf.org/html/rfc2980">RFC 2980</a>,
    /// <a href="https://tools.ietf.org/html/rfc3977">RFC 3977</a>,
    /// <a href="https://tools.ietf.org/html/rfc4643">RFC 4643</a> and
    /// <a href="https://tools.ietf.org/html/rfc6048">RFC 6048</a>.
    /// </summary>
    public partial class NntpClient
    {
        private readonly INntpConnection connection;

        /// <summary>
        /// Creates a new instance of the <see cref="NntpClient"/> class.
        /// </summary>
        /// <param name="connection">The connection to use.</param>
        public NntpClient(INntpConnection connection)
        {
            this.connection = connection.ThrowIfNull(nameof(connection));
        }
    }
}
