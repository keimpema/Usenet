using Usenet.Nntp.Models;

namespace Usenet.Nntp.Responses
{
    /// <summary>
    /// Represents a response to the
    /// <a href="https://tools.ietf.org/html/rfc3977#section-6.1.3">LAST</a> command.
    /// </summary>
    public class NntpLastResponse : NntpResponse
    {
        /// <summary>
        /// The type of the response received from the server.
        /// </summary>
        public NntpLastResponseType ResponseType { get; }

        /// <summary>
        /// The highest existing <see cref="NntpArticle"/> number less than the 
        /// current <see cref="NntpArticle"/> in the currently selected <see cref="NntpGroup"/>.
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// The <see cref="NntpMessageId"/> of the <see cref="NntpArticle"/> with the highest existing number less than the 
        /// current <see cref="NntpArticle"/> in the currently selected <see cref="NntpGroup"/>.
        /// </summary>
        public NntpMessageId MessageId { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NntpLastResponse"/> class.
        /// </summary>
        /// <param name="code">The response code received from the server.</param>
        /// <param name="message">The response message received from the server.</param>
        /// <param name="success">A value indicating whether the command succeeded or failed.</param>
        /// <param name="responseType">The type of the response received from the server.</param>
        /// <param name="number">The <see cref="NntpArticle"/> number received from the server.</param>
        /// <param name="messageId">The <see cref="NntpMessageId"/> received from the server.</param>
        public NntpLastResponse(int code, string message, bool success, NntpLastResponseType responseType, int number, NntpMessageId messageId)
            : base (code, message, success)
        {
            ResponseType = responseType;
            Number = number;
            MessageId = messageId ?? NntpMessageId.Empty;
        }
    }
}
