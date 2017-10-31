using Usenet.Nntp.Models;

namespace Usenet.Nntp.Responses
{
    /// <summary>
    /// Represents a response to the
    /// <a href="https://tools.ietf.org/html/rfc3977#section-6.1.4">NEXT</a> command.
    /// </summary>
    public class NntpNextResponse : NntpResponse
    {
        /// <summary>
        /// The type of the response received from the server.
        /// </summary>
        public NntpNextResponseType ResponseType { get; }

        /// <summary>
        /// The lowest existing <see cref="NntpArticle"/> number greater than the 
        /// current <see cref="NntpArticle"/> in the currently selected <see cref="NntpGroup"/>.
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// The <see cref="NntpMessageId"/> of the <see cref="NntpArticle"/> with the lowest existing number greater than the 
        /// current <see cref="NntpArticle"/> in the currently selected <see cref="NntpGroup"/>.
        /// </summary>
        public NntpMessageId MessageId { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NntpNextResponse"/> class.
        /// </summary>
        /// <param name="code">The response code received from the server.</param>
        /// <param name="message">The response message received from the server.</param>
        /// <param name="success">A value indicating whether the command succeeded or failed.</param>
        /// <param name="responseType">The type of the response received from the server.</param>
        /// <param name="number">The <see cref="NntpArticle"/> number received from the server.</param>
        /// <param name="messageId">The <see cref="NntpMessageId"/> received from the server.</param>
        public NntpNextResponse(int code, string message, bool success, NntpNextResponseType responseType, int number, NntpMessageId messageId)
            : base(code, message, success)
        {
            ResponseType = responseType;
            Number = number;
            MessageId = messageId ?? NntpMessageId.Empty;
        }
    }
}
