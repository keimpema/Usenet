using System;
using System.Collections.Generic;
using System.Text;
using Usenet.Extensions;
using Usenet.Util;

namespace Usenet.Yenc
{
    public class YencStreamDecoder
    {
        public static YencStream Decode(IEnumerable<string> encodedLines) =>
            Decode(encodedLines, UsenetEncoding.Default);

        public static YencStream Decode(IEnumerable<string> encodedLines, Encoding encoding)
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
                return new YencStream(YencMeta.ParseHeader(headers), EnumerateData(enumerator, encoding));
            }
        }

        private static IEnumerable<byte[]> EnumerateData(IEnumerator<string> enumerator, Encoding encoding)
        {
            var buffer = new byte[4096];
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.StartsWith(YencKeywords.Footer))
                {
                    // skip rest if there is some
                    while (enumerator.MoveNext()) { }
                    yield break;
                }

                byte[] encodedBytes = encoding.GetBytes(enumerator.Current);
                int decodedCount = YencLineDecoder.Decode(encodedBytes, buffer, 0);
                var decodedBytes = new byte[decodedCount];
                Buffer.BlockCopy(buffer, 0, decodedBytes, 0, decodedCount);
                yield return decodedBytes;
            }
        }
    }
}
