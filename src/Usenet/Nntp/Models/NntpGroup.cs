using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Usenet.Util;

namespace Usenet.Nntp.Models
{
    /// <summary>
    /// Represents an NNTP newsgroup.
    /// </summary>
    public class NntpGroup : IEquatable<NntpGroup>
    {
        /// <summary>
        /// The name of the <see cref="NntpGroup"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The estimated number of articles in the <see cref="NntpGroup"/>.
        /// </summary>
        public long ArticleCount { get; }

        /// <summary>
        /// Reported low water mark.
        /// </summary>
        public long LowWaterMark { get; }

        /// <summary>
        /// Reported high water mark.
        /// </summary>
        public long HighWaterMark { get; }

        /// <summary>
        /// The current <see cref="NntpPostingStatus"/> of the <see cref="NntpGroup"/> on the server.
        /// </summary>
        public NntpPostingStatus PostingStatus { get; }

        /// <summary>
        /// The name of the other <see cref="NntpGroup"/> under which the articles are filed when the
        /// <see cref="PostingStatus"/> is <see cref="NntpPostingStatus.OnlyArticlesFromPeersPermittedFiledLocally"/>.
        /// </summary>
        public string OtherGroup { get; }

        /// <summary>
        /// A list of <see cref="NntpArticle"/> numbers in the <see cref="NntpGroup"/>.
        /// </summary>
        public IEnumerable<long> ArticleNumbers { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="NntpGroup"/> class.
        /// </summary>
        /// <param name="name">The name of the <see cref="NntpGroup"/>.</param>
        /// <param name="articleCount">The estimated number of articles in the <see cref="NntpGroup"/>.</param>
        /// <param name="lowWaterMark">The reported low water mark of the <see cref="NntpGroup"/>.</param>
        /// <param name="highWaterMark">The reported high water mark of the <see cref="NntpGroup"/>.</param>
        /// <param name="postingStatus">The current <see cref="NntpPostingStatus"/> of the <see cref="NntpGroup"/>.</param>
        /// <param name="otherGroup">The name of the other <see cref="NntpGroup"/> under which the articles are filed when the
        /// <see cref="PostingStatus"/> is <see cref="NntpPostingStatus.OnlyArticlesFromPeersPermittedFiledLocally"/>.</param>
        /// <param name="articleNumbers">A list of <see cref="NntpArticle"/> numbers in the <see cref="NntpGroup"/>.</param>
        public NntpGroup(
            string name, 
            long articleCount, 
            long lowWaterMark,
            long highWaterMark,
            NntpPostingStatus postingStatus,
            string otherGroup,
            IEnumerable<long> articleNumbers)
        {
            Name = name ?? string.Empty;
            ArticleCount = articleCount;
            LowWaterMark = lowWaterMark;
            HighWaterMark = highWaterMark;
            PostingStatus = Enum.IsDefined(typeof(NntpPostingStatus), postingStatus) ? postingStatus : NntpPostingStatus.Unknown;
            OtherGroup = otherGroup ?? string.Empty;

            switch (articleNumbers)
            {
                case null:
                    // create empty immutable list
                    ArticleNumbers = new List<long>(0).ToImmutableList();
                    break;

                case ICollection<long> collection:
                    // make immutable
                    ArticleNumbers = collection.OrderBy(n => n).ToImmutableList();
                    break;

                default:
                    // not a collection but a stream of numbers, keep enumerator
                    // this is immutable already
                    ArticleNumbers = articleNumbers;
                    break;
            }
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() => HashCode.Start
            .Hash(Name)
            .Hash(ArticleCount)
            .Hash(LowWaterMark)
            .Hash(HighWaterMark)
            .Hash(PostingStatus)
            .Hash(OtherGroup);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="NntpGroup"/> value.
        /// </summary>
        /// <param name="other">A <see cref="NntpGroup"/> object to compare to this instance.</param>
        /// <returns>true if <paramref name="other" /> has the same value as this instance; otherwise, false.</returns>
        public bool Equals(NntpGroup other)
        {
            if ((object) other == null)
            {
                return false;
            }

            bool equals =
                Name.Equals(other.Name) &&
                ArticleCount.Equals(other.ArticleCount) &&
                LowWaterMark.Equals(other.LowWaterMark) &&
                HighWaterMark.Equals(other.HighWaterMark) &&
                PostingStatus.Equals(other.PostingStatus) &&
                OtherGroup.Equals(other.OtherGroup);

            if (!equals)
            {
                return false;
            }

            // need to memoize the enumerables for comparison
            // otherwise they can not be used anymore after this call to equals
            if (!(ArticleNumbers is ICollection<long>))
            {
                ArticleNumbers = ArticleNumbers.ToList();
            }
            if (!(other.ArticleNumbers is ICollection<long>))
            {
                other.ArticleNumbers = other.ArticleNumbers.ToList();
            }

            return ArticleNumbers.SequenceEqual(other.ArticleNumbers);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="object"/> value.
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare to this instance.</param>
        /// <returns>true if <paramref name="obj" /> has the same value as this instance; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as NntpGroup);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NntpGroup"/> value is equal to the second <see cref="NntpGroup"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NntpGroup"/>.</param>
        /// <param name="second">The second <see cref="NntpGroup"/>.</param>
        /// <returns>true if <paramref name="first"/> has the same value as <paramref name="second"/>; otherwise false.</returns>
        public static bool operator ==(NntpGroup first, NntpGroup second) => 
            (object) first == null ? (object) second == null : first.Equals(second);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NntpGroup"/> value is unequal to the second <see cref="NntpGroup"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NntpGroup"/>.</param>
        /// <param name="second">The second <see cref="NntpGroup"/>.</param>
        /// <returns>true if <paramref name="first"/> has a different value than <paramref name="second"/>; otherwise false.</returns>
        public static bool operator !=(NntpGroup first, NntpGroup second) => !(first == second);
    }
}
