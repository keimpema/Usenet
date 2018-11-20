using System;
using System.Collections.Generic;

namespace Usenet.Util
{
    /// <summary>
    /// Represents an enumerable stream. Can be used to stream an enumerable collection of
    /// byte buffers.
    /// </summary>
    public class EnumerableStream : AbstractBaseStream
    {
        private readonly IEnumerator<byte[]> enumerator;
        private byte[] currentChunk;
        private int currentOffset;

        /// <summary>
        /// Creates a new instance of the <see cref="EnumerableStream"/> class.
        /// </summary>
        /// <param name="input">An enumerable collection of byte buffers.</param>
        public EnumerableStream(IEnumerable<byte[]> input)
        {
            Guard.ThrowIfNull(input, nameof(input));
            enumerator = input.GetEnumerator();
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

                if (currentChunk == null)
                {
                    continue;
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

        /// <inheritdoc/>
        public override bool CanRead => true;
    }
}
