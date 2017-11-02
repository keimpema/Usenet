using System;
using System.Collections.Generic;
using Usenet.Exceptions;
using Usenet.Extensions;

namespace Usenet.Yenc
{
    internal class YencMeta
    {
        public static IDictionary<string, string> GetHeaders(IEnumerator<string> enumerator)
        {
            if (enumerator == null)
            {
                throw new InvalidYencDataException(Resources.Yenc.MissingHeader);
            }
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.StartsWith(YencKeywords.Header))
                {
                    return ParseLine(enumerator.Current);
                }
            }
            throw new InvalidYencDataException(Resources.Yenc.MissingHeader);
        }

        public static IDictionary<string, string> GetPartHeaders(IEnumerator<string> enumerator)
        {
            if (enumerator == null)
            {
                throw new InvalidYencDataException(Resources.Yenc.MissingPartHeader);
            }
            if (enumerator.MoveNext() && enumerator.Current.StartsWith(YencKeywords.PartHeader))
            {
                return ParseLine(enumerator.Current);
            }
            throw new InvalidYencDataException(Resources.Yenc.MissingPartHeader);
        }

        public static YencHeader ParseHeader(IDictionary<string, string> headers)
        {
            string name = headers.GetOrDefault(YencKeywords.Name);
            long size = headers.GetAndConvert(YencKeywords.Size, long.Parse);
            int line = headers.GetAndConvert(YencKeywords.Line, int.Parse);
            int part = headers.GetAndConvert(YencKeywords.Part, int.Parse);
            int total = headers.GetAndConvert(YencKeywords.Total, int.Parse);
            long begin = headers.GetAndConvert(YencKeywords.Begin, long.Parse);
            long end = headers.GetAndConvert(YencKeywords.End, long.Parse);

            return new YencHeader(
                name,
                size > 0 ? size : 0,
                line > 0 ? line : 0,
                part > 0 ? part : 0,
                part > 0 ? total : 1,
                part > 0 ? end - begin + 1 : size,
                part > 0 ? begin - 1 : 0);
        }

        public static YencFooter ParseFooter(IDictionary<string, string> footer)
        {
            long size = footer.GetAndConvert(YencKeywords.Size, long.Parse);
            int part = footer.GetAndConvert(YencKeywords.Part, int.Parse);
            var crc32 = footer.GetAndConvert<uint?>(YencKeywords.Crc32, crc => Convert.ToUInt32(crc, 16));
            var partCrc32 = footer.GetAndConvert<uint?>(YencKeywords.PartCrc32, crc => Convert.ToUInt32(crc, 16));

            return new YencFooter(
                size > 0 ? size : 0,
                part > 0 ? part : 0,
                crc32,
                partCrc32);
        }

        public static Dictionary<string, string> ParseLine(string line)
        {
            if (line == null)
            {
                return new Dictionary<string, string>(0);
            }

            // name is always last item on the header line
            string[] nameSplit = line.Split(new[] { $"{YencKeywords.Name}=" }, StringSplitOptions.RemoveEmptyEntries);
            if (nameSplit.Length == 0)
            {
                return new Dictionary<string, string>(0);
            }

            var dictionary = new Dictionary<string, string>();
            if (nameSplit.Length > 1)
            {
                // found name
                dictionary.Add(YencKeywords.Name, nameSplit[1].Trim());
            }

            // parse other items
            string[] pairs = nameSplit[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (pairs.Length == 0)
            {
                return dictionary;
            }
            foreach (string pair in pairs)
            {
                string[] parts = pair.Split('=');
                if (parts.Length < 2)
                {
                    continue;
                }
                dictionary.Add(parts[0], parts[1]);
            }
            return dictionary;
        }
    }
}
