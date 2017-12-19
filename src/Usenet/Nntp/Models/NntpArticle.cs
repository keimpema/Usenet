using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Usenet.Extensions;
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
        public long Number { get; }

        /// <summary>
        /// The message-id of the <see cref="NntpArticle"/>.
        /// </summary>
        public NntpMessageId MessageId { get; }

        /// <summary>
        /// The NNTP newsgroups this <see cref="NntpArticle"/> is posted in.
        /// </summary>
        public NntpGroups Groups { get; }

        /// <summary>
        /// The header dictionary of the <see cref="NntpArticle"/>.
        /// </summary>
        public ImmutableDictionary<string, ImmutableHashSet<string>> Headers { get; }

        /// <summary>
        /// The body of the <see cref="NntpArticle"/>.
        /// </summary>
        public IEnumerable<string> Body { get; private set; }

        /// <summary>
        /// Creates a new <see cref="NntpArticle"/> object.
        /// </summary>
        /// <param name="number">The number of the <see cref="NntpArticle"/>.</param>
        /// <param name="messageId">The <see cref="NntpMessageId"/> of the <see cref="NntpArticle"/>.</param>
        /// <param name="groups">The NNTP newsgroups this <see cref="NntpArticle"/> is posted in.</param>
        /// <param name="headers">The headers of the <see cref="NntpArticle"/>.</param>
        /// <param name="body">The body of the <see cref="NntpArticle"/>.</param>
        public NntpArticle(
            long number, 
            NntpMessageId messageId, 
            NntpGroups groups, 
            IDictionary<string, ICollection<string>> headers, 
            IEnumerable<string> body)
        {
            Number = number;
            MessageId = messageId ?? NntpMessageId.Empty;
            Groups = groups ?? NntpGroups.Empty;
            Headers = (headers ?? MultiValueDictionary<string, string>.Empty).ToImmutableDictionaryWithHashSets();

            switch (body)
            {
                case null:
                    // create empty immutable list
                    Body = new List<string>(0).ToImmutableList();
                    break;

                case ICollection<string> collection:
                    // make immutable
                    Body = collection.ToImmutableList();
                    break;

                default:
                    // not a collection but a stream of lines, keep enumerator
                    // this is immutable already
                    Body = body;
                    break;
            }
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() => HashCode.Start
            .Hash(Number)
            .Hash(MessageId)
            .Hash(Groups);

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

            bool equals =
                Number.Equals(other.Number) &&
                MessageId.Equals(other.MessageId) &&
                Groups.Equals(other.Groups);

            if (!equals)
            {
                return false;
            }

            // compare headers
            foreach (KeyValuePair<string, ImmutableHashSet<string>> pair in Headers)
            {
                if (!other.Headers.TryGetValue(pair.Key, out ImmutableHashSet<string> value) ||
                    !pair.Value.SetEquals(value))
                {
                    return false;
                }
            }

            // need to memoize the enumerables for comparison
            // otherwise they can not be used anymore after this call to equals
            if (!(Body is ICollection<string>))
            {
                Body = Body.ToList();
            }
            if (!(other.Body is ICollection<string>))
            {
                other.Body = other.Body.ToList();
            }

            // compare body
            return Body.SequenceEqual(other.Body);
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
        public static bool operator ==(NntpArticle first, NntpArticle second) => 
            (object) first == null ? (object) second == null : first.Equals(second);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NntpArticle"/> value is unequal to the second <see cref="NntpArticle"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NntpArticle"/>.</param>
        /// <param name="second">The second <see cref="NntpArticle"/>.</param>
        /// <returns>true if <paramref name="first"/> has a different value than <paramref name="second"/>; otherwise false.</returns>
        public static bool operator !=(NntpArticle first, NntpArticle second) => !(first == second);
    }
}
