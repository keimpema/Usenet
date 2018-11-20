using System.IO;
using System.Threading.Tasks;

namespace Usenet.Nzb
{
    /// <summary>
    /// TextWriter extension methods.
    /// </summary>
    public static class TextWriterExtensions
    {
        /// <summary>
        /// Writes the specified <see cref="NzbDocument"/> asynchronously to the stream.
        /// </summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to use.</param>
        /// <param name="nzbDocument">The <see cref="NzbDocument"/> to write.</param>
        /// <returns>A <see cref="Task"/> that can be awaited.</returns>
        public static Task WriteNzbDocumentAsync(this TextWriter textWriter, NzbDocument nzbDocument)
        {
            return new NzbWriter(textWriter).WriteAsync(nzbDocument);
        }

        /// <summary>
        /// Writes the specified <see cref="NzbDocument"/> to the stream.
        /// </summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to use.</param>
        /// <param name="nzbDocument">The <see cref="NzbDocument"/> to write.</param>
        public static void WriteNzbDocument(this TextWriter textWriter, NzbDocument nzbDocument)
        {
            new NzbWriter(textWriter).Write(nzbDocument);
        }
    }
}