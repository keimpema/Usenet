using System;
using System.IO;

namespace Usenet.Util
{
    /// <summary>
    /// Represents an abstract base stream that overrides all abstract methods of the <see cref="Stream"/> class.
    /// Derived classes only need to override the methods they need.
    /// </summary>
    public abstract class AbstractBaseStream : Stream
    {
        /// <inheritdoc/>
        public override void Flush() { }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        /// <inheritdoc/>
        public override void SetLength(long value) => throw new NotSupportedException();

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        /// <inheritdoc/>
        public override bool CanRead => false;
        
        /// <inheritdoc/>
        public override bool CanSeek => false;
        
        /// <inheritdoc/>
        public override bool CanWrite => false;
        
        /// <inheritdoc/>
        public override long Length => throw new NotSupportedException();

        /// <inheritdoc/>
        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }
    }
}
