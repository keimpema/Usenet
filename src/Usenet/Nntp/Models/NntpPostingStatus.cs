namespace Usenet.Nntp.Models
{
    /// <summary>
    /// Represents the status a <see cref="NntpGroup"/> can have on the server.
    /// See https://tools.ietf.org/html/rfc3977#section-7.6.3 
    /// and https://tools.ietf.org/html/rfc6048#section-3.1 for more information.
    /// </summary>
    public enum NntpPostingStatus
    {
        /// <summary>
        /// Unknown status.
        /// </summary>
        Unknown,

        /// <summary>
        /// Posting is permitted (status "y").
        /// </summary>
        PostingPermitted,

        /// <summary>
        /// Posting is not permitted (status "n").
        /// </summary>
        PostingNotPermitted,

        /// <summary>
        /// Postings will be forwarded to the newsgroup moderator (status "m").
        /// </summary>
        PostingsWillBeReviewed,

        /// <summary>
        /// Postings and articles from peers are not permitted (status "x").
        /// </summary>
        ArticlesFromPeersNotPermitted,

        /// <summary>
        /// Only articles from peers are permitted; no articles are locally filed (status "j").
        /// </summary>
        OnlyArticlesFromPeersPermittedNotFiledLocally,

        /// <summary>
        /// Only articles from peers are permitted, and are filed under the newsgroup named "other.group" (status "=other.group").
        /// </summary>
        OnlyArticlesFromPeersPermittedFiledLocally
    }
}