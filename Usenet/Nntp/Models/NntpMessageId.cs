using System;
using System.Collections.Generic;
using Usenet.Extensions;

namespace Usenet.Nntp.Models
{
    /// <summary>
    /// Represents an NNTP <a href="https://tools.ietf.org/html/rfc3977#appendix-A.2">Message-ID</a>.
    /// (<a href="https://tools.ietf.org/html/rfc3977#section-3.6">More information</a>).
    /// </summary>
    public class NntpMessageId : IEquatable<NntpMessageId>
    {
        /// <summary>
        /// The value of the <see cref="NntpMessageId"/> object.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NntpMessageId"/> class.
        /// </summary>
        /// <param name="value">A string representing a <see cref="NntpMessageId"/>. 
        /// Wrapping characters "&lt;" and "&gt;" will be stripped.</param>
        public NntpMessageId(string value)
        {
            Value = value == null ? string.Empty : value.TrimStart('<').TrimEnd('>').Pack();
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="NntpMessageId" /> object has a valid value.
        /// </summary>
        /// <returns>
        /// true if the current <see cref="NntpMessageId" /> object has a value; 
        /// false if the current <see cref="NntpMessageId" /> object has no value.
        /// </returns>
        public bool HasValue => !string.IsNullOrWhiteSpace(Value);

        /// <summary>
        /// Wraps the <see cref="NntpMessageId"/> in "&lt;" and "&gt;" according to the NNTP specifications.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => string.IsNullOrEmpty(Value) ? string.Empty : string.Concat("<", Value, ">");

        /// <summary>
        /// Converts a <see cref="NntpMessageId"/> implicitly to a string.
        /// </summary>
        /// <param name="messageId">The <see cref="NntpMessageId"/> to convert.</param>
        public static implicit operator string(NntpMessageId messageId) => messageId?.ToString();

        /// <summary>
        /// Converts a string implicitly to a <see cref="NntpMessageId"/>.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public static implicit operator NntpMessageId(string value) => new NntpMessageId(value);

        /// <summary>
        /// Represents the empty <see cref="NntpMessageId"/>. The field is read-only.
        /// </summary>
        public static NntpMessageId Empty => new NntpMessageId(null);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() => EqualityComparer<string>.Default.GetHashCode(Value);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="NntpMessageId"/> value.
        /// </summary>
        /// <param name="other">A <see cref="NntpMessageId"/> object to compare to this instance.</param>
        /// <returns>true if <paramref name="other" /> has the same value as this instance; otherwise, false.</returns>
        public bool Equals(NntpMessageId other) => (object) other != null && Value == other.Value;

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="object"/> value.
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare to this instance.</param>
        /// <returns>true if <paramref name="obj" /> has the same value as this instance; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as NntpMessageId);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NntpMessageId"/> value is equal to the second <see cref="NntpMessageId"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NntpMessageId"/>.</param>
        /// <param name="second">The second <see cref="NntpMessageId"/>.</param>
        /// <returns>true if <paramref name="first"/> has the same value as <paramref name="second"/>; otherwise false.</returns>
        public static bool operator ==(NntpMessageId first, NntpMessageId second) => 
            (object) first == null ? (object) second == null : first.Equals(second);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NntpMessageId"/> value is unequal to the second <see cref="NntpMessageId"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NntpMessageId"/>.</param>
        /// <param name="second">The second <see cref="NntpMessageId"/>.</param>
        /// <returns>true if <paramref name="first"/> has a different value than <paramref name="second"/>; otherwise false.</returns>
        public static bool operator !=(NntpMessageId first, NntpMessageId second) => !(first == second);
    }
}
