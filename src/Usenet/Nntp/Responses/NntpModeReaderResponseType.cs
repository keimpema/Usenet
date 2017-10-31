namespace Usenet.Nntp.Responses
{
    /// <summary>
    /// Represents all possible response codes for the
    /// <a href="https://tools.ietf.org/html/rfc3977#section-5.3">MODE READER</a>
    /// (<a href="https://tools.ietf.org/html/rfc2980#section-2.3">ad 1</a>) command.
    /// </summary>
    public enum NntpModeReaderResponseType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown,
        /// <summary>
        /// Posting is allowed (response code 200)
        /// </summary>
        PostingAllowed = 200,
        /// <summary>
        /// Posting is not allowed (response code 201)
        /// </summary>
        PostingProhibited = 201,
        /// <summary>
        /// Reading service permanently unavailale (response code 502)
        /// </summary>
        ReadingServiceUnavailable = 502
    }
}
