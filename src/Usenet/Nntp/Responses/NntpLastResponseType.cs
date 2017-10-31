namespace Usenet.Nntp.Responses
{
    /// <summary>
    /// Represents all possible response codes for the
    /// <a href="https://tools.ietf.org/html/rfc3977#section-6.1.3">LAST</a> command.
    /// </summary>
    public enum NntpLastResponseType
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
        /// No previous article in group (response code 422)
        /// </summary>
        NoPreviousArticleInGroup = 422
    }
}
