﻿using Usenet.Extensions;

namespace Usenet.Nntp
{
    /// <summary>
    /// An NNTP client that is compliant with
    /// <a href="https://tools.ietf.org/html/rfc2980">RFC 2980</a>,
    /// <a href="https://tools.ietf.org/html/rfc3977">RFC 3977</a>,
    /// <a href="https://tools.ietf.org/html/rfc4643">RFC 4643</a> and
    /// <a href="https://tools.ietf.org/html/rfc6048">RFC 6048</a>.
    /// Based on Kristian Hellang's NntpLib.Net project https://github.com/khellang/NntpLib.Net.
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

            Common.NntpExtensions.IncrementClientCount();
        }

        /// <summary>
        /// The number of bytes read.
        /// </summary>
        public long BytesRead => connection.Stream?.BytesRead ?? 0;

        /// <summary>
        /// The number of bytes written.
        /// </summary>
        public long BytesWritten => connection.Stream?.BytesWritten ?? 0;

        /// <summary>
        /// Resets the counters.
        /// </summary>
        public void ResetCounters()
        {
            connection.Stream?.ResetCounters();
        }
    }
}
