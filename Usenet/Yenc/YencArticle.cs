using Usenet.Extensions;

namespace Usenet.Yenc
{
    /// <summary>
    /// Represents a decoded yEnc-encoded article.
    /// </summary>
    public class YencArticle
    {
        /// <summary>
        /// Contains the information obtained from the =ybegin header line and =ypart part-header
        /// line if present.
        /// </summary>
        public YencHeader Header { get; }

        /// <summary>
        /// Contains the information obtained from the =yend footer line.
        /// </summary>
        public YencFooter Footer { get; }

        /// <summary>
        /// The binary data obtained by decoding the yEnc-encoded article.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="YencArticle"/> class.
        /// </summary>
        /// <param name="header">The header of the yEnc-encoded article.</param>
        /// <param name="footer">The optional footer of the yEnc-encoded article.</param>
        /// <param name="data">The binary data obtained by decoding the yEnc-encoded article.</param>
        public YencArticle(YencHeader header, YencFooter footer, byte[] data)
        {
            Header = header.ThrowIfNull(nameof(header));
            Footer = footer;
            Data = data.ThrowIfNull(nameof(data));
        }
    }
}
