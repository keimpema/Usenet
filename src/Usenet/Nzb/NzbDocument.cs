using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usenet.Util;

namespace Usenet.Nzb
{
    /// <summary>
    /// Represents a <a href="https://sabnzbd.org/wiki/extra/nzb-spec">NZB</a> document.
    /// </summary>
    public class NzbDocument
    {
        /// <summary>
        /// A collection of metadata elements found in the NZB file.
        /// </summary>
        public ILookup<string, string> MetaData { get; }

        /// <summary>
        /// A collection of files found in the NZB file.
        /// </summary>
        public IList<NzbFile> Files { get; }

        /// <summary>
        /// The total size of the files found in the NZB file.
        /// </summary>
        public long Size { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NzbDocument"/> class.
        /// </summary>
        /// <param name="metaData">A collection of metadata elements foudn in the NZB file.</param>
        /// <param name="files">A collection of files found in the NZB file.</param>
        public NzbDocument(ILookup<string, string> metaData, IList<NzbFile> files)
        {
            MetaData = metaData ?? Enumerable.Empty<string>().ToLookup(x => default(string));
            Files = files ?? new List<NzbFile>(0);
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
    }
}
