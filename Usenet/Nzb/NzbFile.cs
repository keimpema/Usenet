using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Usenet.Nntp.Models;
using Usenet.Util;

namespace Usenet.Nzb
{
    /// <summary>
    /// Represents a file in a <a href="https://sabnzbd.org/wiki/extra/nzb-spec">NZB</a> document.
    /// Based on Kristian Hellang's Nzb project https://github.com/khellang/Nzb.
    /// </summary>
    public class NzbFile : IEquatable<NzbFile>
    {
        /// <summary>
        /// The name of the poster. This is a copy of the article's From header field.
        /// </summary>
        public string Poster { get; }

        /// <summary>
        /// A slightly munged copy of the article's subject. The segment counter (xx/yy) 
        /// usually found at the end, is replaced with (1/yy). You can use the yy to 
        /// confirm all segments are present.
        /// </summary>
        public string Subject { get; }

        /// <summary>
        /// The file name extracted from the subject.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// The date the server saw this article.
        /// </summary>
        public DateTimeOffset Date { get; }

        /// <summary>
        /// The list of groups that reference this file.
        /// </summary>
        public NntpGroups Groups { get; }

        /// <summary>
        /// The list of segments that make up this file.
        /// </summary>
        public ImmutableList<NzbSegment> Segments { get; }

        /// <summary>
        /// The size of the file is calculated as the sum of the segment sizes.
        /// </summary>
        public long Size { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NzbFile"/> class.
        /// </summary>
        /// <param name="poster">The name of the poster. This is a copy of the article's From header field.</param>
        /// <param name="subject">A slightly munged copy of the article's subject. The segment counter (xx/yy) 
        ///     usually found at the end, is replaced with (1/yy). You can use the yy to 
        ///     confirm all segments are present.</param>
        /// <param name="fileName">The file name extracted from the subject.</param>
        /// <param name="date">The date the server saw this article.</param>
        /// <param name="groups">The list of groups that reference this file.</param>
        /// <param name="segments">The list of segments that make up this file.</param>
        public NzbFile(
            string poster, 
            string subject, 
            string fileName,
            DateTimeOffset date, 
            NntpGroups groups,
            IEnumerable<NzbSegment> segments)
        {
            Poster = poster;
            Subject = subject;
            FileName = fileName;
            Date = date;
            Groups = groups ?? NntpGroups.Empty;
            Segments = (segments ?? new List<NzbSegment>(0)).OrderBy(s => s.Number).ToImmutableList();
            Size = Segments.Sum(s => s.Size);   
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() => HashCode.Start
            .Hash(Poster)
            .Hash(Subject)
            .Hash(FileName)
            .Hash(Date)
            .Hash(Groups)
            .Hash(Size);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="NzbFile"/> value.
        /// </summary>
        /// <param name="other">A <see cref="NzbFile"/> object to compare to this instance.</param>
        /// <returns>true if <paramref name="other" /> has the same value as this instance; otherwise, false.</returns>
        public bool Equals(NzbFile other)
        {
            if ((object)other == null)
            {
                return false;
            }
            return
                Poster.Equals(other.Poster) &&
                Subject.Equals(other.Subject) &&
                FileName.Equals(other.FileName) &&
                Date.Equals(other.Date) &&
                Groups.Equals(other.Groups) &&
                Size.Equals(other.Size) &&
                Segments.SequenceEqual(other.Segments);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="NzbFile"/> value.
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare to this instance.</param>
        /// <returns>true if <paramref name="obj" /> has the same value as this instance; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as NzbFile);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NzbFile"/> value is equal to the second <see cref="NzbFile"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NzbFile"/>.</param>
        /// <param name="second">The second <see cref="NzbFile"/>.</param>
        /// <returns>true if <paramref name="first"/> has the same value as <paramref name="second"/>; otherwise false.</returns>
        public static bool operator ==(NzbFile first, NzbFile second) => 
            (object) first == null ? (object) second == null : first.Equals(second);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NzbFile"/> value is unequal to the second <see cref="NzbFile"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NzbFile"/>.</param>
        /// <param name="second">The second <see cref="NzbFile"/>.</param>
        /// <returns>true if <paramref name="first"/> has a different value than <paramref name="second"/>; otherwise false.</returns>
        public static bool operator !=(NzbFile first, NzbFile second) => !(first == second);

    }
}
