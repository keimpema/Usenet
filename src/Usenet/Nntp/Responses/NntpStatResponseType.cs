namespace Usenet.Nntp.Responses
{
    /// <summary>
    /// Represents all possible response codes for the
    /// <a href="https://tools.ietf.org/html/rfc3977#section-6.2.4">STAT</a> command.
    /// </summary>
    public enum NntpStatResponseType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown,
        /// <summary>
        /// Article exists (response code 223)
        /// </summary>
        ArticleExists = 223,
        /// <summary>
        /// No newsgroup selected (response code 412)
        /// </summary>
        NoGroupSelected = 412,
        /// <summary>
        /// Current article number is invalid (response code 420)
        /// </summary>
        CurrentArticleInvalid = 420,
        /// <summary>
        /// No article with that number (response code 423)
        /// </summary>
        NoArticleWithThatNumber = 423,
        /// <summary>
        /// No article with that message-id (response code 430)
        /// </summary>
        NoArticleWithThatMessageId = 430
    }
}
