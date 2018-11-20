using System.Collections.Generic;
using System.IO;
using System.Text;
using Usenet.Util;

namespace Usenet.Yenc
{
    /// <summary>
    /// Represents an yEnc encoder.
    /// </summary>
    public static class YencEncoder
    {
        /// <summary>
        /// Encodes the binary data in the specified stream into yEnc-encoded text
        /// using the default Usenet character encoding.
        /// </summary>
        /// <param name="header">The yEnc header.</param>
        /// <param name="stream">The stream containing the binary data to encode.</param>
        /// <returns>The yEnc-encoded text.</returns>
        public static IEnumerable<string> Encode(YencHeader header, Stream stream) =>
            Encode(header, stream, UsenetEncoding.Default);

        /// <summary>
        /// Encodes the binary data in the specified stream into yEnc-encoded text
        /// using the specified character encoding.
        /// </summary>
        /// <param name="header">The yEnc header.</param>
        /// <param name="stream">The stream containing the binary data to encode.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <returns>The yEnc-encoded text.</returns>
        public static IEnumerable<string> Encode(YencHeader header, Stream stream, Encoding encoding)
        {
            yield return GetHeaderLine(header);
            if (header.IsFilePart)
            {
                yield return GetPartHeaderLine(header);
            }
            var encodedBytes = new byte[1024];
            var encodedOffset = 0;
            int lastCol = header.LineLength - 1;
            uint checksum = Crc32.Initialize();
            for (var offset = 0; offset < header.PartSize; offset++)
            {
                int @byte = stream.ReadByte();
                if (@byte == -1)
                {
                    // end of stream
                    break;
                }
                checksum = Crc32.Calculate(checksum, @byte);
                int val = (@byte + 42) % 256;

                // encode dot only in first column
                bool encodeDot = encodedOffset == 0;

                // encode white space only in first and last column
                bool encodeWhitespace = encodedOffset == 0 || encodedOffset == lastCol;

                // encode critical characters
                if (val == YencCharacters.Null || 
                    val == YencCharacters.Lf || 
                    val == YencCharacters.Cr || 
                    val == YencCharacters.Equal ||
                    val == YencCharacters.Dot && encodeDot ||
                    val == YencCharacters.Space && encodeWhitespace ||
                    val == YencCharacters.Tab && encodeWhitespace)
                {
                    encodedBytes[encodedOffset++] = YencCharacters.Equal;
                    val = (val + 64) % 256;
                }

                encodedBytes[encodedOffset++] = (byte)val;
                if (encodedOffset < header.LineLength)
                {
                    continue;
                }

                // return encoded line
                yield return encoding.GetString(encodedBytes, 0, encodedOffset);

                // reset offset
                encodedOffset = 0;
            }
            if (encodedOffset > 0)
            {
                // return remainder
                yield return encoding.GetString(encodedBytes, 0, encodedOffset);
            }
            checksum = Crc32.Finalize(checksum);
            yield return GetFooterLine(header, checksum);
        }

        private static string GetHeaderLine(YencHeader header)
        {
            var builder = new StringBuilder(YencKeywords.YBegin);

            if (header.IsFilePart)
            {
                builder.Append(' ').Append(YencKeywords.Part).Append('=').Append(header.PartNumber);
                builder.Append(' ').Append(YencKeywords.Total).Append('=').Append(header.TotalParts);
            }

            builder.Append(' ').Append(YencKeywords.Line).Append('=').Append(header.LineLength);
            builder.Append(' ').Append(YencKeywords.Size).Append('=').Append(header.FileSize);
            builder.Append(' ').Append(YencKeywords.Name).Append('=').Append(header.FileName);

            return builder.ToString();
        }

        private static string GetPartHeaderLine(YencHeader header)
        {
            long begin = header.PartOffset + 1;
            long end = header.PartOffset + header.PartSize;
            return $"{YencKeywords.YPart} {YencKeywords.Begin}={begin} {YencKeywords.End}={end}";
        }

        private static string GetFooterLine(YencHeader header, uint checksum)
        {
            var builder = new StringBuilder(YencKeywords.YEnd);

            if (header.IsFilePart)
            {
                builder.Append(' ').Append(YencKeywords.Size).Append('=').Append(header.PartSize);
                builder.Append(' ').Append(YencKeywords.Part).Append('=').Append(header.PartNumber);
                builder.Append(' ').Append(YencKeywords.PartCrc32).Append('=').AppendFormat("{0:x}", checksum);
            }
            else
            {
                builder.Append(' ').Append(YencKeywords.Size).Append('=').Append(header.FileSize);
                builder.Append(' ').Append(YencKeywords.Crc32).Append('=').AppendFormat("{0:x}", checksum);
            }

            return builder.ToString();
        }
    }
}
