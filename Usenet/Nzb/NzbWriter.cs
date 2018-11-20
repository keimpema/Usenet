using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Usenet.Nzb
{
    /// <summary>
    /// Represents an NZB document writer.
    /// </summary>
    public class NzbWriter
    {
        private readonly TextWriter textWriter;

        /// <summary>
        /// Creates a new instance of the <see cref="NzbWriter"/> class that will use
        /// the specified <see cref="TextWriter"/> for writing.
        /// </summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to use for writing.</param>
        public NzbWriter(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        /// <summary>
        /// Writes the specified <see cref="NzbDocument"/> asynchronously to the stream.
        /// </summary>
        /// <param name="nzbDocument">The NZB document to write.</param>
        /// <returns>A <see cref="Task"/> that can be awaited.</returns>
        public async Task WriteAsync(NzbDocument nzbDocument)
        {
            using (XmlWriter writer = GetXmlWriter())
            {
                await writer.WriteDocTypeAsync(
                    NzbKeywords.Nzb, 
                    NzbKeywords.PubId,
                    NzbKeywords.SysId, 
                    null);

                await writer.WriteStartElementAsync(
                    null,
                    NzbKeywords.Nzb, 
                    NzbKeywords.Namespace);

                await WriteHeadAsync(writer, nzbDocument);
                await WriteFilesAsync(writer, nzbDocument);
                await writer.WriteEndElementAsync();
                await writer.WriteEndDocumentAsync();
                await writer.FlushAsync();
            }
        }

        /// <summary>
        /// Writes the specified <see cref="NzbDocument"/> to the stream.
        /// </summary>
        /// <param name="nzbDocument">The NZB document to write.</param>
        /// <returns>A <see cref="Task"/> that can be awaited.</returns>
        public void Write(NzbDocument nzbDocument)
        {
            using (XmlWriter writer = GetXmlWriter())
            {
                writer.WriteDocType(
                    NzbKeywords.Nzb,
                    NzbKeywords.PubId,
                    NzbKeywords.SysId,
                    null);

                writer.WriteStartElement(
                    NzbKeywords.Nzb,
                    NzbKeywords.Namespace);

                WriteHead(writer, nzbDocument);
                WriteFiles(writer, nzbDocument);
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
            }
        }

        private XmlWriter GetXmlWriter() => XmlWriter.Create(textWriter, new XmlWriterSettings
        {
            Encoding = textWriter.Encoding,
            Async = true,
            Indent = true
        });

        private static async Task WriteHeadAsync(XmlWriter writer, NzbDocument nzbDocument)
        {
            await writer.WriteStartElementAsync(null, NzbKeywords.Head, null);
            foreach (KeyValuePair<string, ImmutableHashSet<string>> header in nzbDocument.MetaData)
            {
                foreach (string value in header.Value)
                {
                    await writer.WriteStartElementAsync(null, NzbKeywords.Meta, null);
                    await writer.WriteAttributeStringAsync(null, NzbKeywords.Type, null, header.Key);
                    await writer.WriteStringAsync(value);
                    await writer.WriteEndElementAsync();
                }
            }
            await writer.WriteEndElementAsync();
        }

        private static void WriteHead(XmlWriter writer, NzbDocument nzbDocument)
        {
            writer.WriteStartElement(NzbKeywords.Head);
            foreach (KeyValuePair<string, ImmutableHashSet<string>> header in nzbDocument.MetaData)
            {
                foreach (string value in header.Value)
                {
                    writer.WriteStartElement(NzbKeywords.Meta);
                    writer.WriteAttributeString(NzbKeywords.Type, header.Key);
                    writer.WriteString(value);
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
        }

        private static async Task WriteFilesAsync(XmlWriter writer, NzbDocument nzbDocument)
        {
            foreach (NzbFile file in nzbDocument.Files)
            {
                await writer.WriteStartElementAsync(null, NzbKeywords.File, null);
                await writer.WriteAttributeStringAsync(null, NzbKeywords.Poster, null, file.Poster);
                await writer.WriteAttributeStringAsync(null, NzbKeywords.Date, null, file.Date.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(null, NzbKeywords.Subject, null, file.Subject);
                await WriteGroupsAsync(writer, file);
                await WriteSegmentsAsync(writer, file);
                await writer.WriteEndElementAsync();
            }
        }

        private static void WriteFiles(XmlWriter writer, NzbDocument nzbDocument)
        {
            foreach (NzbFile file in nzbDocument.Files)
            {
                writer.WriteStartElement(NzbKeywords.File);
                writer.WriteAttributeString(NzbKeywords.Poster, file.Poster);
                writer.WriteAttributeString(NzbKeywords.Date, file.Date.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture));
                writer.WriteAttributeString(NzbKeywords.Subject, file.Subject);
                WriteGroups(writer, file);
                WriteSegments(writer, file);
                writer.WriteEndElement();
            }
        }

        private static async Task WriteGroupsAsync(XmlWriter writer, NzbFile file)
        {
            await writer.WriteStartElementAsync(null, NzbKeywords.Groups, null);
            foreach (string group in file.Groups)
            {
                await writer.WriteElementStringAsync(null, NzbKeywords.Group, null, group);
            }
            await writer.WriteEndElementAsync();
        }

        private static void WriteGroups(XmlWriter writer, NzbFile file)
        {
            writer.WriteStartElement(NzbKeywords.Groups);
            foreach (string group in file.Groups)
            {
                writer.WriteElementString(NzbKeywords.Group, group);
            }
            writer.WriteEndElement();
        }

        private static async Task WriteSegmentsAsync(XmlWriter writer, NzbFile file)
        {
            await writer.WriteStartElementAsync(null, NzbKeywords.Segments, null);
            foreach (NzbSegment segment in file.Segments)
            {
                await writer.WriteStartElementAsync(null, NzbKeywords.Segment, null);
                await writer.WriteAttributeStringAsync(null, NzbKeywords.Bytes, null, segment.Size.ToString(CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(null, NzbKeywords.Number, null, segment.Number.ToString(CultureInfo.InvariantCulture));
                await writer.WriteStringAsync(segment.MessageId.Value);
                await writer.WriteEndElementAsync();
            }
            await writer.WriteEndElementAsync();
        }

        private static void WriteSegments(XmlWriter writer, NzbFile file)
        {
            writer.WriteStartElement(NzbKeywords.Segments);
            foreach (NzbSegment segment in file.Segments)
            {
                writer.WriteStartElement(NzbKeywords.Segment);
                writer.WriteAttributeString(NzbKeywords.Bytes, segment.Size.ToString(CultureInfo.InvariantCulture));
                writer.WriteAttributeString(NzbKeywords.Number, segment.Number.ToString(CultureInfo.InvariantCulture));
                writer.WriteString(segment.MessageId.Value);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}
