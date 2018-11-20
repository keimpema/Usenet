using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Usenet.Util;
using Xunit;

namespace UsenetTests.Util
{
    public class CountingStreamTests
    {
        [Fact]
        public void CountingStreamShouldCountBytesRead()
        {
            var memStream = new MemoryStream(new byte[10]);
            var stream = new CountingStream(memStream);
            stream.ReadByte();
            stream.ReadByte();
            stream.ReadByte();
            stream.ReadByte();
            stream.ReadByte();

            Assert.Equal(5, stream.BytesRead);
        }

        [Fact]
        public void CountingStreamShouldCountBytesWritten()
        {
            var memStream = new MemoryStream(new byte[10]);
            var stream = new CountingStream(memStream);
            stream.WriteByte(1);
            stream.WriteByte(2);
            stream.WriteByte(3);
            stream.WriteByte(4);
            stream.WriteByte(5);

            Assert.Equal(5, stream.BytesWritten);
        }

        [Fact]
        public void ResetCountersShouldResetBytesReadAndBytesWritten()
        {
            var memStream = new MemoryStream(new byte[10]);
            var stream = new CountingStream(memStream);
            stream.ReadByte();
            stream.ReadByte();
            stream.WriteByte(1);
            stream.WriteByte(2);

            Assert.Equal(2, stream.BytesRead);
            Assert.Equal(2, stream.BytesWritten);

            stream.ResetCounters();

            Assert.Equal(0, stream.BytesRead);
            Assert.Equal(0, stream.BytesWritten);
        }
    }
}
