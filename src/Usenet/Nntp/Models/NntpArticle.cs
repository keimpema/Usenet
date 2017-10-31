using System;
using System.Collections.Generic;
using System.Linq;
using Usenet.Util;

namespace Usenet.Nntp.Models
{
    /// <summary>
    /// Represents an NNTP article.
    /// </summary>
    public class NntpArticle : IEquatable<NntpArticle>
    {
        /// <summary>
        /// The number of the <see cref="NntpArticle"/> in the currently selected newsgroup.
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// The message-id of the <see cref="NntpArticle"/>.
        /// </summary>
        public NntpMessageId MessageId { get; }

        /// <summary>
        /// The header dictionary of the <see cref="NntpArticle"/>.
        /// </summary>
        public MultiValueDictionary<string, string> Headers { get; }

        /// <summary>
        /// The body of the <see cref="NntpArticle"/>.
        /// </summary>
        public IEnumerable<string> Body { get; }

        /// <summary>
        /// Creates a new <see cref="NntpArticle"/> object.
        /// </summary>
        /// <param name="number">The number of the <see cref="NntpArticle"/>.</param>
        /// <param name="messageId">The <see cref="NntpMessageId"/> of the <see cref="NntpArticle"/>.</param>
        /// <param name="headers">The headers of the <see cref="NntpArticle"/>.</param>
        /// <param name="body">The body of the <see cref="NntpArticle"/>.</param>
        public NntpArticle(int number, NntpMessageId messageId, MultiValueDictionary<string, string> headers, IEnumerable<string> body)
        {
            Number = number;
            MessageId = messageId ?? NntpMessageId.Empty;
            Headers = headers ?? MultiValueDictionary<string, string>.Empty;
            Body = body ?? EmptyList<string>.Instance;            
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash *= 23 + Number.GetHashCode();
                hash *= 23 + MessageId.GetHashCode();
                hash *= 23 + Headers.GetHashCode();
                hash *= 23 + Body.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="NntpArticle"/> value.
        /// </summary>
        /// <param name="other">A <see cref="NntpArticle"/> object to compare to this instance.</param>
        /// <returns>true if <paramref name="other" /> has the same value as this instance; otherwise, false.</returns>
        public bool Equals(NntpArticle other)
        {
            if ((object) other == null)
            {
                return false;
            }
            return 
                Number.Equals(other.Number) &&
                MessageId.Equals(other.MessageId) &&
                Headers.Equals(other.Headers) &&
                Body.SequenceEqual(other.Body);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="NntpArticle"/> value.
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare to this instance.</param>
        /// <returns>true if <paramref name="obj" /> has the same value as this instance; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as NntpArticle);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NntpArticle"/> value is equal to the second <see cref="NntpArticle"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NntpArticle"/>.</param>
        /// <param name="second">The second <see cref="NntpArticle"/>.</param>
        /// <returns>true if <paramref name="first"/> has the same value as <paramref name="second"/>; otherwise false.</returns>
        public static bool operator ==(NntpArticle first, NntpArticle second)
        {
            if ((object)first == null)
            {
                return (object)second == null;
            }
            return first.Equals(second);
        }

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NntpArticle"/> value is unequal to the second <see cref="NntpArticle"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NntpArticle"/>.</param>
        /// <param name="second">The second <see cref="NntpArticle"/>.</param>
        /// <returns>true if <paramref name="first"/> has a different value than <paramref name="second"/>; otherwise false.</returns>
        public static bool operator !=(NntpArticle first, NntpArticle second) => !(first == second);
    }
}
