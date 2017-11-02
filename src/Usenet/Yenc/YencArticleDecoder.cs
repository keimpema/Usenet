using System.Collections.Generic;
using System.Text;
using Usenet.Extensions;
using Usenet.Util;

namespace Usenet.Yenc
{
    /// <summary>
    /// Represents a Yenc-encoded article decoder.
    /// The article is completely decoded in memory.
    /// </summary>
    public class YencArticleDecoder
    {
        /// <summary>
        /// Decodes Yenc-encoded text into a <see cref="YencArticle"/>
        /// using the default Usenet character encoding.
        /// </summary>
        /// <param name="encodedLines">The Yenc encoded lines to decode.</param>
        /// <returns>A <see cref="YencArticle"/> containing the decoded binary data and meta-data.</returns>
        public static YencArticle Decode(IEnumerable<string> encodedLines) =>
            Decode(encodedLines, UsenetEncoding.Default);

        /// <summary>
        /// Decodes Yenc-encoded text into a <see cref="YencArticle"/>
        /// using the specified charcter encoding.
        /// </summary>
        /// <param name="encodedLines">The Yenc-encoded lines to decode.</param>
        /// <param name="encoding">The charcter encoding to use.</param>
        /// <returns>A <see cref="YencArticle"/> containing the decoded binary data and meta-data.</returns>
        public static YencArticle Decode(IEnumerable<string> encodedLines, Encoding encoding)
        {
            Guard.ThrowIfNull(encodedLines, nameof(encodedLines));
            Guard.ThrowIfNull(encoding, nameof(encoding));

            using (IEnumerator<string> enumerator = encodedLines.GetEnumerator())
            {
                IDictionary<string, string> headers = YencMeta.GetHeaders(enumerator);
                int part = headers.GetAndConvert(YencKeywords.Part, int.Parse);
                if (part > 0)
                {
                    headers.Merge(YencMeta.GetPartHeaders(enumerator), false);
                }

                YencHeader header = YencMeta.ParseHeader(headers);
                YencFooter footer = null;

                // create buffer for part or entire file if single part
                var decodedBytes = new byte[header.PartSize];
                var decodedBytesIndex = 0;

                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.StartsWith(YencKeywords.Footer))
                    {
                        footer = YencMeta.ParseFooter(YencMeta.ParseLine(enumerator.Current));

                        // skip rest if there is some
                        while (enumerator.MoveNext()){}
                        break;
                    }
                    byte[] encodedBytes = encoding.GetBytes(enumerator.Current);
                    decodedBytesIndex += YencLineDecoder.Decode(encodedBytes, decodedBytes, decodedBytesIndex);
                }

                return new YencArticle(header, footer, decodedBytes);
            }
        }
    }
}
