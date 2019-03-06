namespace Usenet.Nzb
{
    /// <summary>
    /// Defines <a href="https://sabnzbd.org/wiki/extra/nzb-spec">NZB</a> xml elements and attributes.
    /// Based on Kristian Hellang's Nzb project https://github.com/khellang/Nzb.
    /// </summary>
    internal class NzbKeywords
    {
        /// <summary>
        /// NZB document public identifier.
        /// </summary>
        public const string PubId = "-//newzBin//DTD NZB 1.1//EN";

        /// <summary>
        /// NZB document system identifier.
        /// </summary>
        public const string SysId = "http://www.newzbin.com/DTD/nzb/nzb-1.1.dtd";

        /// <summary>
        /// NZB document namespace.
        /// </summary>
        public const string Namespace = "http://www.newzbin.com/DTD/2003/nzb";

        /// <summary>
        /// The nzb element.
        /// </summary>
        public const string Nzb = "nzb";

        /// <summary>
        /// The head element.
        /// </summary>
        public const string Head = "head";

        /// <summary>
        /// The meta element.
        /// </summary>
        public const string Meta = "meta";

        /// <summary>
        /// The file element.
        /// </summary>
        public const string File = "file";

        /// <summary>
        /// The groups element.
        /// </summary>
        public const string Groups = "groups";

        /// <summary>
        /// The segments element.
        /// </summary>
        public const string Segments = "segments";

        /// <summary>
        /// The group element.
        /// </summary>
        public const string Group = "group";

        /// <summary>
        /// The segment element.
        /// </summary>
        public const string Segment = "segment";

        /// <summary>
        /// The type attribute.
        /// </summary>
        public const string Type = "type";

        /// <summary>
        /// The poster attribute.
        /// </summary>
        public const string Poster = "poster";

        /// <summary>
        /// The date attribute.
        /// </summary>
        public const string Date = "date";

        /// <summary>
        /// The subject attribute.
        /// </summary>
        public const string Subject = "subject";

        /// <summary>
        /// The number attribute.
        /// </summary>
        public const string Number = "number";

        /// <summary>
        /// The bytes attribute.
        /// </summary>
        public const string Bytes = "bytes";
    }
}
