using System;
using Usenet.Util;

namespace Usenet.Nntp.Models
{
    /// <summary>
    /// Represents origin information about a <see cref="NntpGroup"/> like who created the 
    /// <see cref="NntpGroup"/> and when. Can be retrieved from the server with the
    /// <a href="https://tools.ietf.org/html/rfc3977#section-7.6.4">LIST ACTIVE.TIMES</a> command.
    /// (<a href="https://tools.ietf.org/html/rfc2980#section-2.1.3">Some older information</a>).
    /// </summary>
    public class NntpGroupOrigin : IEquatable<NntpGroupOrigin>
    {
        /// <summary>
        /// The name of the <see cref="NntpGroup"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The date and time the <see cref="NntpGroup"/> was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; }

        /// <summary>
        /// A description of the entity that created the <see cref="NntpGroup"/>;
        /// it is often a mailbox as described in <a href="https://tools.ietf.org/html/rfc2822">RFC 2822</a>.
        /// </summary>
        public string CreatedBy { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NntpGroupOrigin"/> class.
        /// </summary>
        /// <param name="name">The name of the <see cref="NntpGroup"/>.</param>
        /// <param name="createdAt">The date and time the <see cref="NntpGroup"/> was created.</param>
        /// <param name="createdBy">A description of the entity that created the <see cref="NntpGroup"/>;
        /// it is often a mailbox as described in <a href="https://tools.ietf.org/html/rfc2822">RFC 2822</a>. </param>
        public NntpGroupOrigin(string name, DateTimeOffset createdAt, string createdBy)
        {
            Name = name ?? string.Empty;
            CreatedAt = createdAt;
            CreatedBy = createdBy ?? string.Empty;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() => HashCode.Start
            .Hash(Name)
            .Hash(CreatedAt)
            .Hash(CreatedBy);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="NntpGroupOrigin"/> value.
        /// </summary>
        /// <param name="other">A <see cref="NntpGroupOrigin"/> object to compare to this instance.</param>
        /// <returns>true if <paramref name="other" /> has the same value as this instance; otherwise, false.</returns>
        public bool Equals(NntpGroupOrigin other)
        {
            if ((object)other == null)
            {
                return false;
            }
            return 
                Name.Equals(other.Name) &&
                CreatedAt.Equals(other.CreatedAt) &&
                CreatedBy.Equals(other.CreatedBy);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="object"/> value.
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare to this instance.</param>
        /// <returns>true if <paramref name="obj" /> has the same value as this instance; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as NntpGroupOrigin);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NntpGroupOrigin"/> value is equal to the second <see cref="NntpGroupOrigin"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NntpGroupOrigin"/>.</param>
        /// <param name="second">The second <see cref="NntpGroupOrigin"/>.</param>
        /// <returns>true if <paramref name="first"/> has the same value as <paramref name="second"/>; otherwise false.</returns>
        public static bool operator ==(NntpGroupOrigin first, NntpGroupOrigin second) => 
            (object) first == null ? (object) second == null : first.Equals(second);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NntpGroupOrigin"/> value is unequal to the second <see cref="NntpGroupOrigin"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NntpGroupOrigin"/>.</param>
        /// <param name="second">The second <see cref="NntpGroupOrigin"/>.</param>
        /// <returns>true if <paramref name="first"/> has a different value than <paramref name="second"/>; otherwise false.</returns>
        public static bool operator !=(NntpGroupOrigin first, NntpGroupOrigin second) => !(first == second);

    }
}
