using System;
using System.Collections.Generic;
using System.Linq;
using Usenet.Util;

namespace Usenet.Nzb
{
    /// <summary>
    /// Represents a file in a <a href="https://sabnzbd.org/wiki/extra/nzb-spec">NZB</a> document.
    /// </summary>
    public class NzbFile
    {
        /// <summary>
        /// The name of the poster. This is a copy of the article's From header field.
        /// </summary>
        public string Poster { get; }

        /// <summary>
        /// A slightly munged copy of the article's subject. The segment counter (xx/yy) 
        /// usually found at the end, is replaced with (1/yy). You can use the yy to 
        /// confirm all segments are present.
        /// </summary>
        public string Subject { get; }

        /// <summary>
        /// The date the server saw this article.
        /// </summary>
        public DateTimeOffset Date { get; }

        /// <summary>
        /// The list of groups that reference this file.
        /// </summary>
        public IList<string> Groups { get; }

        /// <summary>
        /// The list of segments that make up this file.
        /// </summary>
        public IList<NzbSegment> Segments { get; }

        /// <summary>
        /// The size of the file is calculated as the sum of the segment sizes.
        /// </summary>
        public long Size { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="NzbFile"/> class.
        /// </summary>
        /// <param name="poster">The name of the poster. This is a copy of the article's From header field.</param>
        /// <param name="subject">A slightly munged copy of the article's subject. The segment counter (xx/yy) 
        /// usually found at the end, is replaced with (1/yy). You can use the yy to 
        /// confirm all segments are present.</param>
        /// <param name="date">The date the server saw this article.</param>
        /// <param name="groups">The list of groups that reference this file.</param>
        /// <param name="segments">The list of segments that make up this file.</param>
        public NzbFile(
            string poster, 
            string subject, 
            DateTimeOffset date, 
            IList<string> groups,
            IList<NzbSegment> segments)
        {
            Poster = poster;
            Subject = subject;
            Date = date;
            Groups = groups ?? EmptyList<string>.Instance;
            Segments = segments ?? EmptyList<NzbSegment>.Instance;
            Size = Segments.Sum(s => s.Size);   
        }
    }
}
