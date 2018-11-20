using System.Collections.Generic;
using Usenet.Util;

namespace Usenet.Yenc
{
    /// <summary>
    /// Represents a decoded yEnc-encoded article as a stream.
    /// </summary>
    public class YencStream : EnumerableStream
    {
        /// <summary>
        /// Contains the information obtained from the =ybegin header line and =ypart part-header
        /// line if present.
        /// </summary>
        public YencHeader Header { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="YencStream"/> class.
        /// </summary>
        /// <param name="header">The header of the yEnc-encoded article.</param>
        /// <param name="input">An enumeration of byte chunks from the decoded yEnc-encoded article.</param>
        public YencStream(YencHeader header, IEnumerable<byte[]> input) : base(input)
        {
            Guard.ThrowIfNull(header, nameof(header));
            Guard.ThrowIfNull(input, nameof(input));
            Header = header;
        }
    }
}
