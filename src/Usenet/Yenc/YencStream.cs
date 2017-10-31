using System.Collections.Generic;
using Usenet.Util;

namespace Usenet.Yenc
{
    public class YencStream : EnumerableStream
    {
        public YencHeader Header { get; }

        public YencStream(YencHeader header, IEnumerable<byte[]> input) : base(input)
        {
            Guard.ThrowIfNull(header, nameof(header));
            Guard.ThrowIfNull(input, nameof(input));
            Header = header;
        }
    }
}
