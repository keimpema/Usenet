using System.IO;

namespace Usenet.Util
{
    /// <summary>
    /// Represents a counting stream. It can be used to count the number of bytes read and written.
    /// </summary>
    public class CountingStream : AbstractBaseStream
    {
        private readonly Stream innerStream;

        /// <summary>
        /// The number of bytes read.
        /// </summary>
        public long BytesRead { get; private set; }

        /// <summary>
        /// The number of bytes written.
        /// </summary>
        public long BytesWritten { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="CountingStream"/> class.
        /// </summary>
        /// <param name="innerStream">The stream on which counting needs to be enabled.</param>
        public CountingStream(Stream innerStream)
        {
            this.innerStream = innerStream;
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            innerStream.Flush();
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = innerStream.Read(buffer, offset, count);
            unchecked
            {
                BytesRead += bytesRead;
                if (BytesRead < 0)
                {
                    ResetCounters();
                }
            }
            return bytesRead;
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            innerStream.Write(buffer, offset, count);
            unchecked
            {
                BytesWritten += count;
                if (BytesWritten < 0)
                {
                    ResetCounters();
                }
            }
        }

        /// <inheritdoc/>
        public override bool CanRead => true;
        
        /// <inheritdoc/>
        public override bool CanWrite => true;

        /// <summary>
        /// Resets the counters.
        /// </summary>
        public void ResetCounters()
        {
            BytesRead = 0;
            BytesWritten = 0;
        }
    }
}
