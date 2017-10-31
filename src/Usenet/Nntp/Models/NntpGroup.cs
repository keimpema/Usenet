using System;
using System.Collections.Generic;
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
        public int ArticleCount { get; }

        /// <summary>
        /// Reported low water mark.
        /// </summary>
        public int LowWaterMark { get; }

        /// <summary>
        /// Reported high water mark.
        /// </summary>
        public int HighWaterMark { get; }

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
        public IEnumerable<int> ArticleNumbers { get; }

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
            int articleCount, 
            int lowWaterMark,
            int highWaterMark,
            NntpPostingStatus postingStatus,
            string otherGroup,
            IEnumerable<int> articleNumbers)
        {
            Name = name ?? string.Empty;
            ArticleCount = articleCount;
            LowWaterMark = lowWaterMark;
            HighWaterMark = highWaterMark;
            PostingStatus = Enum.IsDefined(typeof(NntpPostingStatus), postingStatus) ? postingStatus : NntpPostingStatus.Unknown;
            OtherGroup = otherGroup ?? string.Empty;
            ArticleNumbers = articleNumbers ?? EmptyList<int>.Instance;
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
                hash *= 23 + Name.GetHashCode();
                hash *= 23 + ArticleCount.GetHashCode();
                hash *= 23 + LowWaterMark.GetHashCode();
                hash *= 23 + HighWaterMark.GetHashCode();
                hash *= 23 + PostingStatus.GetHashCode();
                hash *= 23 + OtherGroup.GetHashCode();
                hash *= 23 + ArticleNumbers.GetHashCode();
                return hash;
            }
        }

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
            return 
                Name.Equals(other.Name) && 
                ArticleCount.Equals(other.ArticleCount) &&
                LowWaterMark.Equals(other.LowWaterMark) && 
                HighWaterMark.Equals(other.HighWaterMark) &&
                PostingStatus.Equals(other.PostingStatus) && 
                OtherGroup.Equals(other.OtherGroup) &&
                MultiSetComparer<int>.Instance.Equals(ArticleNumbers, other.ArticleNumbers);
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
        public static bool operator ==(NntpGroup first, NntpGroup second)
        {
            if ((object)first == null)
            {
                return (object)second == null;
            }
            return first.Equals(second);
        }

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NntpGroup"/> value is unequal to the second <see cref="NntpGroup"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NntpGroup"/>.</param>
        /// <param name="second">The second <see cref="NntpGroup"/>.</param>
        /// <returns>true if <paramref name="first"/> has a different value than <paramref name="second"/>; otherwise false.</returns>
        public static bool operator !=(NntpGroup first, NntpGroup second) => !(first == second);
    }
}
