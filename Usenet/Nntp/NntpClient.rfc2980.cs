using Usenet.Nntp.Models;
using Usenet.Nntp.Parsers;
using Usenet.Nntp.Responses;

namespace Usenet.Nntp
{
    public partial class NntpClient
    {
        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc2980#section-2.6">XHDR</a> 
        /// command is used to retrieve a specific header from a specific article.
        /// </summary>
        /// <param name="field">The header field to retrieve.</param>
        /// <param name="messageId">The message-id of the article to retrieve the header for.</param>
        /// <returns>A multi-line response object containing the header.</returns>
        public NntpMultiLineResponse Xhdr(string field, NntpMessageId messageId) =>
            connection.MultiLineCommand($"XHDR {field} {messageId}", new MultiLineResponseParser(221));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc2980#section-2.6">XHDR</a> 
        /// command is used to retrieve a specific header from specific articles.
        /// </summary>
        /// <param name="field">The header field to retrieve.</param>
        /// <param name="range">The range of articles to retrieve the header for.</param>
        /// <returns>A multi-line response object containing the headers.</returns>
        public NntpMultiLineResponse Xhdr(string field, NntpArticleRange range) =>
            connection.MultiLineCommand($"XHDR {field} {range}", new MultiLineResponseParser(221));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc2980#section-2.6">XHDR</a> 
        /// command is used to retrieve a specific header from the current article.
        /// </summary>
        /// <param name="field">The header field to retrieve.</param>
        /// <returns>A multi-line response object containing the headers.</returns>
        public NntpMultiLineResponse Xhdr(string field) =>
            connection.MultiLineCommand($"XHDR {field}", new MultiLineResponseParser(221));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc2980#section-2.8">XOVER</a> 
        /// command returns information from the overview database for the article(s) specified.
        /// </summary>
        /// <param name="range">The range of articles to retrieve the overview information for.</param>
        /// <returns>A multi-line response object containing the overview database information.</returns>
        public NntpMultiLineResponse Xover(NntpArticleRange range) =>
            connection.MultiLineCommand($"XOVER {range}", new MultiLineResponseParser(224));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc2980#section-2.8">XOVER</a> 
        /// command returns information from the overview database for the current article.
        /// </summary>
        /// <returns>A multi-line response object containing the overview database information.</returns>
        public NntpMultiLineResponse Xover() => connection.MultiLineCommand("XOVER", new MultiLineResponseParser(224));

    }
}
