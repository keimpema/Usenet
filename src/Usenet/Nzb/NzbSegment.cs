using Usenet.Nntp.Models;

namespace Usenet.Nzb
{
    /// <summary>
    /// Represents a segment of a file in a <a href="https://sabnzbd.org/wiki/extra/nzb-spec">NZB</a> document.
    /// </summary>
    public class NzbSegment
    {
        /// <summary>
        /// Segment number of the article, gleaned by parsing (yy/zz).
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// Size of the article, in bytes, as a number, with no comma separation.
        /// </summary>
        public long Size { get; }

        /// <summary>
        /// The Message-ID of this article.
        /// </summary>
        public NntpMessageId MessageId { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NzbSegment"/> class.
        /// </summary>
        /// <param name="number">Segment number of the article, gleaned by parsing (yy/zz).</param>
        /// <param name="size">Size of the article, in bytes, as a number, with no comma separation.</param>
        /// <param name="messageId">The Message-ID of this article.</param>
        public NzbSegment(
            int number,
            long size,
            NntpMessageId messageId)
        {
            Number = number;
            Size = size;
            MessageId = messageId ?? NntpMessageId.Empty;
        }
    }
}
