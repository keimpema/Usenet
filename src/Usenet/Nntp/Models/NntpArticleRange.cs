using System;

namespace Usenet.Nntp.Models
{
    /// <summary>
    /// Represents an article range. The article range can be passed as parameter to the
    /// <a href="https://tools.ietf.org/html/rfc3977#section-6.1.2">LISTGROUP</a>,
    /// <a href="https://tools.ietf.org/html/rfc2980#section-2.6">XHDR</a>,
    /// <a href="https://tools.ietf.org/html/rfc2980#section-2.8">XOVER</a>,
    /// <a href="https://tools.ietf.org/html/rfc2980#section-2.9">XPAT</a> and
    /// <a href="https://tools.ietf.org/html/rfc2980#section-2.11">XROVER</a> commands.
    /// </summary>
    public class NntpArticleRange : IEquatable<NntpArticleRange>
    {
        /// <summary>
        /// The article number the range starts from.
        /// </summary>
        public int From { get; }

        /// <summary>
        /// The optional article number the range ends with.
        /// </summary>
        public int? To { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NntpArticleRange"/> class.
        /// </summary>
        /// <param name="from">The article number to start the range from.</param>
        /// <param name="to">The optional article number to end the range with.</param>
        private NntpArticleRange(int from, int? to)
        {
            From = from;
            To = to;
        }

        /// <summary>
        /// Creates a range of one article.
        /// </summary>
        /// <param name="number">The article number.</param>
        /// <returns>A new range of one article.</returns>
        public static NntpArticleRange SingleArticle(int number)
        {
            return new NntpArticleRange(number, number);
        }

        /// <summary>
        /// Creates a range containing the given article and all following.
        /// </summary>
        /// <param name="from">The article number to start the range from.</param>
        /// <returns>A new range containing the given article and all following.</returns>
        public static NntpArticleRange AllFollowing(int from)
        {
            return new NntpArticleRange(from, null);
        }

        /// <summary>
        /// Creates a range containing all articles between and including <paramref name="from"/> and <paramref name="to"/>.
        /// </summary>
        /// <param name="from">The article number to start the range from.</param>
        /// <param name="to">The article number to end the range with.</param>
        /// <returns>A new range containg all articles between and including <paramref name="from"/> and <paramref name="to"/>.</returns>
        public static NntpArticleRange Range(int from, int to)
        {
            return new NntpArticleRange(from, to);
        }

        /// <summary>
        /// Returns the text representation of the value formatted according to the NNTP specifications.
        /// </summary>
        /// <returns>The text representation of the value formatted according to the NNTP specifications</returns>
        public override string ToString()
        {
            if (To == null)
            {
                return $"{From}-";
            }
            return To.Value == From ? From.ToString() : $"{From}-{To.Value}";
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
                hash *= 23 + From.GetHashCode();
                hash *= 23 + To.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="NntpArticleRange"/> value.
        /// </summary>
        /// <param name="other">A <see cref="NntpArticleRange"/> object to compare to this instance.</param>
        /// <returns>true if <paramref name="other" /> has the same value as this instance; otherwise, false.</returns>
        public bool Equals(NntpArticleRange other)
        {
            return (object)other != null && From == other.From && To == other.To;
        }

        public override bool Equals(object obj) => Equals(obj as NntpArticleRange);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NntpArticleRange"/> value is equal to the second <see cref="NntpArticleRange"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NntpArticleRange"/>.</param>
        /// <param name="second">The second <see cref="NntpArticleRange"/>.</param>
        /// <returns>true if <paramref name="first"/> has the same value as <paramref name="second"/>; otherwise false.</returns>
        public static bool operator ==(NntpArticleRange first, NntpArticleRange second)
        {
            return (object)first == null ? (object)second == null : first.Equals(second);
        }

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NntpArticleRange"/> value is unequal to the second <see cref="NntpArticleRange"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NntpArticleRange"/>.</param>
        /// <param name="second">The second <see cref="NntpArticleRange"/>.</param>
        /// <returns>true if <paramref name="first"/> has a different value than <paramref name="second"/>; otherwise false.</returns>
        public static bool operator !=(NntpArticleRange first, NntpArticleRange second) => !(first == second);

    }
}
