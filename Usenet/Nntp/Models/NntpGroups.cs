using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Usenet.Nntp.Builders;
using Usenet.Nntp.Parsers;
using Usenet.Util;

namespace Usenet.Nntp.Models
{
    /// <summary>
    /// Represent a list of NNTP newsgroups.
    /// </summary>
    public class NntpGroups : IEnumerable<string>, IEquatable<NntpGroups>
    {
        /// <summary>
        /// The seperator used in the NNTP Newsgroups header.
        /// </summary>
        public const string GroupSeperator = ";";

        private readonly ImmutableList<string> groups;

        /// <summary>
        /// Creates a new <see cref="NntpGroups"/> object.
        /// </summary>
        public NntpGroups(string groups) : this(GroupsParser.Parse(groups), false)
        {
        }

        /// <summary>
        /// Creates a new <see cref="NntpGroups"/> object.
        /// </summary>
        public NntpGroups(IEnumerable<string> groups) : this(groups, true)
        {
        }

        internal NntpGroups(IEnumerable<string> groups, bool doParse)
        {
            if (groups == null)
            {
                this.groups = ImmutableList<string>.Empty;
                return;
            }
            IEnumerable<string> parsedGroups = doParse ? GroupsParser.Parse(groups) : groups;
            this.groups = parsedGroups.OrderBy(g => g).ToImmutableList();
        }

        /// <summary>
        /// Gets a value that indicates whether this list is empty.
        /// </summary>
        public bool IsEmpty => groups.IsEmpty;

        /// <summary>
        /// Gets an empty <see cref="NntpGroups"/> object.
        /// </summary>
        public static NntpGroups Empty { get; } = new NntpGroups((string)null);

        /// <summary>Returns an enumerator that iterates through the <see cref="NntpGroups" /> values.</summary>
        /// <returns>An enumerator that iterates through the <see cref="NntpGroups" /> values.</returns>
        public IEnumerator<string> GetEnumerator() => groups.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Concatenates all NNTP newsgroup names in a single string using the ';' character as a seperator.
        /// This is the format used in the NNTP Newsgroups header.
        /// </summary>
        /// <returns>All NNTP newsgroup names in a single string seperated by the ';' character.</returns>
        public override string ToString() => string.Join(GroupSeperator, groups);

        /// <summary>
        /// Converts a string implicitly to a <see cref="NntpGroups"/>.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public static implicit operator NntpGroups(string value) => new NntpGroupsBuilder().Add(value).Build();

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() => HashCode.Start.Hash(groups);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="NntpGroups"/> value.
        /// </summary>
        /// <param name="other">A <see cref="NntpGroups"/> object to compare to this instance.</param>
        /// <returns>true if <paramref name="other" /> has the same value as this instance; otherwise, false.</returns>
        public bool Equals(NntpGroups other) => (object)other != null && groups.SequenceEqual(other.groups);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="NntpGroups"/> value.
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare to this instance.</param>
        /// <returns>true if <paramref name="obj" /> has the same value as this instance; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as NntpGroups);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NntpGroups"/> value is equal to the second <see cref="NntpGroups"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NntpGroups"/>.</param>
        /// <param name="second">The second <see cref="NntpGroups"/>.</param>
        /// <returns>true if <paramref name="first"/> has the same value as <paramref name="second"/>; otherwise false.</returns>
        public static bool operator ==(NntpGroups first, NntpGroups second) => 
            (object) first == null ? (object) second == null : first.Equals(second);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NntpGroups"/> value is unequal to the second <see cref="NntpGroups"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NntpGroups"/>.</param>
        /// <param name="second">The second <see cref="NntpGroups"/>.</param>
        /// <returns>true if <paramref name="first"/> has a different value than <paramref name="second"/>; otherwise false.</returns>
        public static bool operator !=(NntpGroups first, NntpGroups second) => !(first == second);
    }
}
