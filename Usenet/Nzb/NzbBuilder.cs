using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.FileProviders;
using Usenet.Nntp.Builders;
using Usenet.Nntp.Models;
using Usenet.Util;

namespace Usenet.Nzb
{
    /// <summary>
    /// Represents a mutable <see cref="NzbDocument"/>.
    /// </summary>
    public class NzbBuilder
    {
        private readonly List<File> files;
        private readonly NntpGroupsBuilder groupsBuilder;
        private readonly MultiValueDictionary<string, string> metaData;
        private string messageBase = "unknown.com";
        private string documentPoster = "Anonymous <anonymous@unknown.com>";
        private long partSize = 384_000;

        /// <summary>
        /// Creates a new instance of the <see cref="NzbBuilder"/> class.
        /// </summary>
        public NzbBuilder()
        {
            files = new List<File>();
            groupsBuilder = new NntpGroupsBuilder();
            metaData = new MultiValueDictionary<string, string>();
        }

        /// <summary>
        /// Sets the NZB document's default poster. Will be used for every file.
        /// </summary>
        /// <param name="value">The poster value. Usually an e-mail address.</param>
        /// <returns>The <see cref="NzbBuilder"/> so that additional calls can be chained.</returns>
        public NzbBuilder SetPoster(string value)
        {
            Guard.ThrowIfNullOrWhiteSpace(value, nameof(value));
            documentPoster = value;
            return this;
        }

        /// <summary>
        /// Sets the NZB document's part size. Will be used for every segment.
        /// </summary>
        /// <param name="value">The segment size in bytes.</param>
        /// <returns>The <see cref="NzbBuilder"/> so that additional calls can be chained.</returns>
        public NzbBuilder SetPartSize(long value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            partSize = value;
            return this;
        }

        /// <summary>
        /// Sets the NZB document's message base. Will be used in every segment's message id.
        /// Message id's format: part{partNr}of{totalPart}.{guid}@{messageBase}.
        /// For example: part1of10.4c7cb35fd4004511b981a69500697aac@random.local.
        /// </summary>
        /// <param name="value">The message base to use.</param>
        /// <returns>The <see cref="NzbBuilder"/> so that additional calls can be chained.</returns>
        public NzbBuilder SetMessageBase(string value)
        {
            Guard.ThrowIfNullOrWhiteSpace(value, nameof(value));
            messageBase = value;
            return this;
        }

        /// <summary>
        /// Adds a file to the NZB document. Additional newsgroups may be provided.
        /// Optionally the default poster van be overriden.
        /// </summary>
        /// <param name="fileInfo">The file to add.</param>
        /// <param name="groups">The newsgroups to post the file in.</param>
        /// <param name="poster">Can be used to override the default poster.</param>
        /// <returns>The <see cref="NzbBuilder"/> so that additional calls can be chained.</returns>
        public NzbBuilder AddFile(IFileInfo fileInfo, NntpGroups groups = null, string poster = null)
        {
            Guard.ThrowIfNull(fileInfo, nameof(fileInfo));
            files.Add(new File(fileInfo, groups ?? NntpGroups.Empty, poster));
            return this;
        }

        /// <summary>
        /// Adds default newsgroups to the NZB document. All files will be posted to these newsgroups.
        /// </summary>
        /// <param name="groups">The newsgroups to post all the files in.</param>
        /// <returns>The <see cref="NzbBuilder"/> so that additional calls can be chained.</returns>
        public NzbBuilder AddGroups(params NntpGroups[] groups)
        {
            Guard.ThrowIfNull(groups, nameof(groups));
            foreach (NntpGroups group in groups)
            {
                groupsBuilder.Add(group);
            }
            return this;
        }

        /// <summary>
        /// Adds metadata to the NZB document.
        /// </summary>
        /// <param name="key">The metadata key.</param>
        /// <param name="value">The metadata value.</param>
        /// <returns>The <see cref="NzbBuilder"/> so that additional calls can be chained.</returns>
        public NzbBuilder AddMetaData(string key, string value)
        {
            metaData.Add(key, value);
            return this;
        }

        /// <summary>
        /// Creates a <see cref="NzbDocument"/> with al the properties from the <see cref="NzbBuilder"/>.
        /// </summary>
        /// <returns>The <see cref="NzbDocument"/>.</returns>
        public NzbDocument Build() => new NzbDocument(GetMetaData(), GetFiles());

        private MultiValueDictionary<string, string> GetMetaData()
        {
            IEnumerable<Tuple<string, string>> headers = 
                from pair in metaData
                from val in pair.Value
                select new Tuple<string, string>(pair.Key, val);

            var dict = new MultiValueDictionary<string, string>();
            foreach (Tuple<string, string> header in headers)
            {
                dict.Add(header.Item1, header.Item2);
            }
            return dict;
        }

        private List<NzbFile> GetFiles()
        {
            DateTimeOffset date = DateTimeOffset.UtcNow;
            return files
                .Select(f => new NzbFile(
                    f.Poster ?? documentPoster, 
                    GetSubject(f.FileInfo), 
                    f.FileInfo.Name, 
                    date, 
                    new NntpGroupsBuilder().Add(f.Groups).Add(groupsBuilder.Groups).Build(), 
                    GetSegments(f.FileInfo)))
                .ToList();
        }

        private string GetSubject(IFileInfo fileInfo)
        {
            string segmentCount = GetSegmentCount(fileInfo).ToString();
            string one = "1".PadLeft(segmentCount.Length, '0');
            return $"\"{fileInfo.Name}\" yEnc ({one}/{segmentCount})";
        }

        private IEnumerable<NzbSegment> GetSegments(IFileInfo fileInfo)
        {
            Guid fileGuid = Guid.NewGuid();
            var segments = new List<NzbSegment>();
            int segmentCount = GetSegmentCount(fileInfo);
            var offset = 0L;
            for (var number = 1; number <= segmentCount; number++)
            {
                string messageId = $"part{number}of{segmentCount}.{fileGuid:n}@{messageBase}";
                long size = number < segmentCount ? partSize : fileInfo.Length - offset;
                segments.Add(new NzbSegment(number, offset, size, messageId));
                offset += size;
            }
            return segments;
        }

        private int GetSegmentCount(IFileInfo fileInfo)
        {
            var count = (int)(fileInfo.Length / partSize);
            if (count * partSize < fileInfo.Length)
            {
                count++;
            }
            return count;
        }

        private class File
        {
            public IFileInfo FileInfo { get; }
            public NntpGroups Groups { get; }
            public string Poster { get; }

            public File(IFileInfo fileInfo, NntpGroups groups, string poster)
            {
                FileInfo = fileInfo;
                Groups = groups;
                Poster = poster;
            }
        }
    }
}

