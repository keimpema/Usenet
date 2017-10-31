using System.IO;
using System.Text;
using Usenet.Util;

namespace Usenet.Nntp
{
    /// <summary>
    /// This NNTP streamreader respects the <a href="https://tools.ietf.org/html/rfc3977#section-3.1.1">rules</a> for multi-line data blocks.
    /// It will undo dot-stuffing and will stop at the terminating line (".").
    /// </summary>
    public class NntpStreamReader : StreamReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NntpStreamReader"/> class for the specified stream, with the default usenet encoding.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        public NntpStreamReader(Stream stream) : base(stream, UsenetEncoding.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NntpStreamReader"/> class for the specified stream, with the specified character encoding.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        public NntpStreamReader(Stream stream, Encoding encoding) : base(stream, encoding)
        {
        }

        /// <summary>
        /// Reads a line of characters from the current stream and returns the data as a string.
        /// Dot-stuffing will be undone and the terminating line (".") will result in a null value
        /// indicating end of input.
        /// </summary>
        /// <returns>The next line from the input stream, or null if the end of the input stream is reached.</returns>
        public override string ReadLine()
        {
            string line = base.ReadLine();
            if (line == null)
            {
                return null;
            }
            if (line.Length == 0 || line[0] != '.')
            {
                return line;
            }
            if (line.Length == 1)
            {
                return null;
            }
            return line[1] == '.' ? line.Substring(1) : line;
        }
    }
}
