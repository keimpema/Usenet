using Usenet.Nntp.Models;

namespace Usenet.Nntp.Responses
{
    /// <summary>
    /// Represents a response to the 
    /// <a href="https://tools.ietf.org/html/rfc3977#section-6.2.1">ARTICLE</a>,
    /// <a href="https://tools.ietf.org/html/rfc3977#section-6.2.2">HEAD</a> and
    /// <a href="https://tools.ietf.org/html/rfc3977#section-6.2.3">BODY</a> commands.
    /// </summary>
    public class NntpArticleResponse : NntpResponse
    {
        /// <summary>
        /// The <see cref="NntpArticle"/> received from the server.
        /// </summary>
        public NntpArticle Article { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NntpArticleResponse"/> class.
        /// </summary>
        /// <param name="code">The response code received from the server.</param>
        /// <param name="message">The response message received from the server.</param>
        /// <param name="success">A value indicating whether the command succeeded or failed.</param>
        /// <param name="article">The <see cref="NntpArticle"/> received from the server.</param>
        public NntpArticleResponse(int code, string message, bool success, NntpArticle article) 
            : base(code, message, success)
        {
            Article = article;
        }
    }
}
