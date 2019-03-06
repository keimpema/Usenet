using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usenet.Extensions;
using Usenet.Util;

namespace Usenet.Nzb
{
    /// <summary>
    /// Represents a <a href="https://sabnzbd.org/wiki/extra/nzb-spec">NZB</a> document.
    /// Based on Kristian Hellang's Nzb project https://github.com/khellang/Nzb.
    /// </summary>
    public class NzbDocument : IEquatable<NzbDocument>
    {
        /// <summary>
        /// A collection of metadata elements found in the NZB file.
        /// </summary>
        public ImmutableDictionary<string, ImmutableHashSet<string>> MetaData { get; }

        /// <summary>
        /// A collection of files found in the NZB file.
        /// </summary>
        public ImmutableList<NzbFile> Files { get; }

        /// <summary>
        /// The total size of the files found in the NZB file.
        /// </summary>
        public long Size { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NzbDocument"/> class.
        /// </summary>
        /// <param name="metaData">A collection of metadata elements found in the NZB file.</param>
        /// <param name="files">A collection of files found in the NZB file.</param>
        public NzbDocument(IDictionary<string, ICollection<string>> metaData, IEnumerable<NzbFile> files)
        {
            MetaData = (metaData ?? MultiValueDictionary<string, string>.Empty).ToImmutableDictionaryWithHashSets();
            Files = (files ?? new List<NzbFile>(0)).OrderBy(f => f.FileName).ToImmutableList();
            Size = Files.Sum(f => f.Size);
        }

        /// <summary>
        /// Loads a <see cref="NzbDocument"/> from the specified stream asynchronously using the default usenet encoding.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <returns>A task that represents the asynchronous load operation.
        /// The value of the task's result property contains the resulting <see cref="NzbDocument"/>.</returns>
        /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
        public static Task<NzbDocument> LoadAsync(Stream stream) => LoadAsync(stream, UsenetEncoding.Default);

        /// <summary>
        /// Loads a <see cref="NzbDocument"/> from the specified stream asynchronously using the specified character encoding.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <returns>A task that represents the asynchronous load operation.
        /// The value of the task's result property contains the resulting <see cref="NzbDocument"/>.</returns>
        /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
        public static async Task<NzbDocument> LoadAsync(Stream stream, Encoding encoding)
        {
            Guard.ThrowIfNull(stream, nameof(stream));
            Guard.ThrowIfNull(encoding, nameof(encoding));

            using (var reader = new StreamReader(stream, encoding))
            {
                return NzbParser.Parse(await reader.ReadToEndAsync());
            }
        }

        /// <summary>
        /// Loads a <see cref="NzbDocument"/> from the specified stream using the default usenet encoding.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <returns>The parsed <see cref="NzbDocument"/>.</returns>
        /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
        public static NzbDocument Load(Stream stream) => Load(stream, UsenetEncoding.Default);

        /// <summary>
        /// Loads a <see cref="NzbDocument"/> from the specified stream using the specified character encoding.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <returns>The parsed <see cref="NzbDocument"/>.</returns>
        /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
        public static NzbDocument Load(Stream stream, Encoding encoding)
        {
            Guard.ThrowIfNull(stream, nameof(stream));
            Guard.ThrowIfNull(encoding, nameof(encoding));

            using (var reader = new StreamReader(stream, encoding))
            {
                return NzbParser.Parse(reader.ReadToEnd());
            }
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() => HashCode.Start
            .Hash(Size)
            .Hash(Files);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="NzbDocument"/> value.
        /// </summary>
        /// <param name="other">A <see cref="NzbDocument"/> object to compare to this instance.</param>
        /// <returns>true if <paramref name="other" /> has the same value as this instance; otherwise, false.</returns>
        public bool Equals(NzbDocument other)
        {
            if ((object)other == null)
            {
                return false;
            }

            bool equals =
                Size.Equals(other.Size) &&
                Files.SequenceEqual(other.Files) &&
                MetaData.Count == other.MetaData.Count;

            if (!equals)
            {
                return false;
            }

            // compare metadata
            foreach (KeyValuePair<string, ImmutableHashSet<string>> pair in MetaData)
            {
                if (!other.MetaData.TryGetValue(pair.Key, out ImmutableHashSet<string> value) ||
                    !pair.Value.SetEquals(value))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <see cref="NzbDocument"/> value.
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare to this instance.</param>
        /// <returns>true if <paramref name="obj" /> has the same value as this instance; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as NzbDocument);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NzbDocument"/> value is equal to the second <see cref="NzbDocument"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NzbDocument"/>.</param>
        /// <param name="second">The second <see cref="NzbDocument"/>.</param>
        /// <returns>true if <paramref name="first"/> has the same value as <paramref name="second"/>; otherwise false.</returns>
        public static bool operator ==(NzbDocument first, NzbDocument second) => 
            (object) first == null ? (object) second == null : first.Equals(second);

        /// <summary>
        /// Returns a value indicating whether the frst <see cref="NzbDocument"/> value is unequal to the second <see cref="NzbDocument"/> value.
        /// </summary>
        /// <param name="first">The first <see cref="NzbDocument"/>.</param>
        /// <param name="second">The second <see cref="NzbDocument"/>.</param>
        /// <returns>true if <paramref name="first"/> has a different value than <paramref name="second"/>; otherwise false.</returns>
        public static bool operator !=(NzbDocument first, NzbDocument second) => !(first == second);
    }
}
