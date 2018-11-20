using System;
using Usenet.Nntp.Models;
using Usenet.Util;

namespace Usenet.Nzb
{
    /// <summary>
    /// Represents a segment of a file in a <a href="https://sabnzbd.org/wiki/extra/nzb-spec">NZB</a> document.
    /// </summary>
    public class NzbSegment : IEquatable<NzbSegment>
    {
        /// <summary>
        /// Segment number of the article, gleaned by parsing (yy/zz).
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// Offset of the segment in the file. This is a calculated value, not an attribute
        /// on the segment element in the NZB document.
        /// </summary>
        public long Offset { get; }

        /// <summary>
        /// Size of the article, in bytes, as a number, with no comma separation.
        /// </summary>
        public long Size { get; }

        /// <summary>
        /// The Message-ID of this article.
        /// </summary>
        public NntpMessageId MessageId { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NzbSegment"/> class.
        /// </summary>
        /// <param name="number">Segment number of the article, gleaned by parsing (yy/zz).</param>
        /// <param name="offset">Offset of the segment in the file.</param>
        /// <param name="size">Size of the article, in bytes, as a number, with no comma separation.</param>
        /// <param name="messageId">The Message-ID of this article.</param>
        public NzbSegment(
            int number,
            long offset,
            long size,
            NntpMessageId messageId)
        {
            Number = number;
            Offset = offset;
            Size = size;
            MessageId = messageId ?? NntpMessageId.Empty;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() => HashCode.Start
            .Hash(Number)
            .Hash(Offset)
            .Hash(Size)
            .Hash(MessageId);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="NzbSegment"/> value.
        /// </summary>
        /// <param name="other">A <see cref="NzbSegment"/> object to compare to this instance.</param>
        /// <returns>true if <paramref name="other" /> has the same value as this instance; otherwise, false.</returns>
        public bool Equals(NzbSegment other)
        {
            if ((object)other == null)
            {
                return false;
            }
            return
                Number.Equals(other.Number) &&
                Offset.Equals(other.Offset) &&
                Size.Equals(other.Size) &&
                MessageId.Equals(other.MessageId);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="NzbSegment"/> value.
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare to this instance.</param>
        /// <returns>true if <paramref name="obj" /> has the same value as this instance; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as NzbSegment);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NzbSegment"/> value is equal to the second <see cref="NzbSegment"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NzbSegment"/>.</param>
        /// <param name="second">The second <see cref="NzbSegment"/>.</param>
        /// <returns>true if <paramref name="first"/> has the same value as <paramref name="second"/>; otherwise false.</returns>
        public static bool operator ==(NzbSegment first, NzbSegment second) => 
            (object) first == null ? (object) second == null : first.Equals(second);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NzbSegment"/> value is unequal to the second <see cref="NzbSegment"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NzbSegment"/>.</param>
        /// <param name="second">The second <see cref="NzbSegment"/>.</param>
        /// <returns>true if <paramref name="first"/> has a different value than <paramref name="second"/>; otherwise false.</returns>
        public static bool operator !=(NzbSegment first, NzbSegment second) => !(first == second);
    }
}
