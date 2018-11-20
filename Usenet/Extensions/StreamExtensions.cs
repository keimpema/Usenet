using System.IO;
using Usenet.Util;

namespace Usenet.Extensions
{
    internal static class StreamExtensions
    {
        public static byte[] ReadAllBytes(this Stream stream)
        {
            Guard.ThrowIfNull(stream, nameof(stream));

            if (stream.CanSeek)
            {
                stream.Seek(0L, SeekOrigin.Begin);
            }
            if (stream is MemoryStream memoryStream)
            {
                return memoryStream.ToArray();
            }
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
