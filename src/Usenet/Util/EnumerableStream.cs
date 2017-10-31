using System;
using System.Collections.Generic;

namespace Usenet.Util
{
    public class EnumerableStream : AbstractBaseStream
    {
        private readonly IEnumerator<byte[]> enumerator;
        private byte[] currentChunk;
        private int currentOffset;

        public EnumerableStream(IEnumerable<byte[]> input)
        {
            Guard.ThrowIfNull(input, nameof(input));
            enumerator = input.GetEnumerator();
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    enumerator?.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Guard.ThrowIfNull(buffer, nameof(buffer));
            if (offset < 0 || offset >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }
            if (count < 0 || offset + count > buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            var total = 0;
            while (count > 0)
            {
                if (currentChunk == null || currentOffset >= currentChunk.Length)
                {
                    // need a new chunk
                    if (!enumerator.MoveNext())
                    {
                        // no more chunks available
                        return total;
                    }
                    currentChunk = enumerator.Current;
                    currentOffset = 0;
                }
                int copyCount = Math.Min(count, currentChunk.Length - currentOffset);
                Buffer.BlockCopy(currentChunk, currentOffset, buffer, offset, copyCount);
                currentOffset += copyCount;
                offset += copyCount;
                total += copyCount;
                count -= copyCount;
            }
            return total;
        }

        public override bool CanRead => true;
    }
}
