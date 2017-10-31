using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Usenet.Exceptions;
using Usenet.Util;

namespace Usenet.Nzb
{
    /// <summary>
    /// Represents a <a href="https://sabnzbd.org/wiki/extra/nzb-spec">NZB</a> document parser. 
    /// It takes an xml string as input and parses it into an instance of the <see cref="NzbDocument"/> class.
    /// </summary>
    public class NzbParser
    {
        /// <summary>
        /// Represents the NZB namespace.
        /// </summary>
        public static readonly XNamespace NzbNamespace = "http://www.newzbin.com/DTD/2003/nzb";

        /// <summary>
        /// Parses the xml input string into an instance of the <see cref="NzbDocument"/> class.
        /// </summary>
        /// <param name="text">An xml string representing the NZB document.</param>
        /// <returns>A parsed <see cref="NzbDocument"/>.</returns>
        /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
        /// <exception cref="ArgumentException">ArgumentException</exception>
        /// <exception cref="InvalidNzbDataException">InvalidNzbDataException</exception>
        public static NzbDocument Parse(string text)
        {
            Guard.ThrowIfNullOrEmpty(text, nameof(text));

            XDocument doc = XDocument.Parse(text);
            XNamespace ns = NzbNamespace;
            XElement nzbElement = doc.Element(ns + NzbKeywords.Nzb);

            if (nzbElement == null)
            {
                ns = XNamespace.None;
            }
            nzbElement = doc.Element(ns + NzbKeywords.Nzb);

            if (nzbElement == null)
            {
                throw new InvalidNzbDataException(Resources.Nzb.MissingNzbElement);
            }

            var context = new NzbParserContext
            {
                Namespace = ns
            };

            ILookup<string, string> metaData = GetMetaData(context, nzbElement);
            List<NzbFile> files = GetFiles(context, nzbElement);

            return new NzbDocument(metaData, files);
        }

        private static ILookup<string, string> GetMetaData(NzbParserContext context, XContainer nzbElement)
        {
            XElement headElement = nzbElement.Element(context.Namespace + NzbKeywords.Head);
            if (headElement == null)
            {
                return null;
            }

            IEnumerable<Tuple<string, string>> metaData = 
                from metaElement in headElement.Elements(context.Namespace + NzbKeywords.Meta)
                let typeAttribute = metaElement.Attribute(NzbKeywords.Type)
                where typeAttribute != null
                select new Tuple<string, string>(typeAttribute.Value, metaElement.Value);

            return metaData.ToLookup(tuple => tuple.Item1, tuple => tuple.Item2);
        }

        private static List<NzbFile> GetFiles(NzbParserContext context, XContainer nzbElement)
        {
            return nzbElement
                .Elements(context.Namespace + NzbKeywords.File)
                .Select(f => GetFile(context, f))
                .ToList();
        }

        private static NzbFile GetFile(NzbParserContext context, XElement fileElement)
        {
            string poster = (string) fileElement.Attribute(NzbKeywords.Poster) ?? string.Empty;
            if (!long.TryParse((string)fileElement.Attribute(NzbKeywords.Date) ?? "0", out long unixTimestamp))
            {
                throw new InvalidNzbDataException(Resources.Nzb.InvalidDateAttriubute);
            }
            DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
            string subject = (string) fileElement.Attribute(NzbKeywords.Subject) ?? string.Empty;
            List<string> groups = GetGroups(context, fileElement.Element(context.Namespace + NzbKeywords.Groups));
            List<NzbSegment> segments = GetSegments(context, fileElement.Element(context.Namespace + NzbKeywords.Segments));

            return new NzbFile(poster, subject, date, groups, segments);
        }

        private static List<string> GetGroups(NzbParserContext context, XContainer groupsElement)
        {
            return groupsElement?
                .Elements(context.Namespace + NzbKeywords.Group)
                .Select(g => g.Value)
                .ToList();
        }

        private static List<NzbSegment> GetSegments(NzbParserContext context, XContainer segentsElement)
        {
            return segentsElement?
                .Elements(context.Namespace + NzbKeywords.Segment)
                .Select(GetSegment)
                .ToList();
        }

        private static NzbSegment GetSegment(XElement element)
        {
            if (!int.TryParse((string)element.Attribute(NzbKeywords.Number), out int number))
            {
                throw new InvalidNzbDataException(Resources.Nzb.InvalidOrMissingNumberAttribute);
            }
            if (!long.TryParse((string)element.Attribute(NzbKeywords.Bytes), out long size))
            {
                throw new InvalidNzbDataException(Resources.Nzb.InvalidOrMissingBytesAttribute);
            }
            string messageId = element.Value;

            return new NzbSegment(number, size, messageId);
        }
    }
}
