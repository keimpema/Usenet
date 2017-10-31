namespace Usenet.Nntp
{
    /// <summary>
    /// Defines NNTP <a href="https://tools.ietf.org/html/rfc5536#section-3">news header fields</a>.
    /// </summary>
    public static class NntpHeaders
    {
        /// <summary>
        /// Mandatory <a href="https://tools.ietf.org/html/rfc5536#section-3.1.1">Date</a> header.
        /// </summary>
        public const string Date = "Date";

        /// <summary>
        /// Mandatory <a href="https://tools.ietf.org/html/rfc5536#section-3.1.2">From</a> header
        /// </summary>
        public const string From = "From";

        /// <summary>
        /// Mandatory <a href="https://tools.ietf.org/html/rfc5536#section-3.1.3">Message-ID</a> header
        /// </summary>
        public const string MessageId = "Message-ID";

        /// <summary>
        /// Mandatory <a href="https://tools.ietf.org/html/rfc5536#section-3.1.4">Newsgroups</a> header
        /// </summary>
        public const string Newsgroups = "Newsgroups";

        /// <summary>
        /// Mandatory <a href="https://tools.ietf.org/html/rfc5536#section-3.1.5">Path</a> header
        /// </summary>
        public const string Path = "Path";

        /// <summary>
        /// Mandatory <a href="https://tools.ietf.org/html/rfc5536#section-3.1.6">Subject</a> header
        /// </summary>
        public const string Subject = "Subject";

        /// <summary>
        /// Optional <a href="https://tools.ietf.org/html/rfc5536#section-3.2.1">Approved</a> header
        /// </summary>
        public const string Approved = "Approved";

        /// <summary>
        /// Optional <a href="https://tools.ietf.org/html/rfc5536#section-3.2.2">Archive</a> header
        /// </summary>
        public const string Archive = "Archive";

        /// <summary>
        /// Optional <a href="https://tools.ietf.org/html/rfc5536#section-3.2.3">Control</a> header
        /// </summary>
        public const string Control = "Control";

        /// <summary>
        /// Optional <a href="https://tools.ietf.org/html/rfc5536#section-3.2.4">Distribution</a> header
        /// </summary>
        public const string Distribution = "Distribution";

        /// <summary>
        /// Optional <a href="https://tools.ietf.org/html/rfc5536#section-3.2.5">Expires</a> header
        /// </summary>
        public const string Expires = "Expires";

        /// <summary>
        /// Optional <a href="https://tools.ietf.org/html/rfc5536#section-3.2.6">Followup-To</a> header
        /// </summary>
        public const string FollowupTo = "Followup-To";

        /// <summary>
        /// Optional <a href="https://tools.ietf.org/html/rfc5536#section-3.2.7">Injection-Date</a> header
        /// </summary>
        public const string InjectionDate = "Injection-Date";

        /// <summary>
        /// Optional <a href="https://tools.ietf.org/html/rfc5536#section-3.2.8">Injection-Info</a> header
        /// </summary>
        public const string InjectionInfo = "Injection-Info";

        /// <summary>
        /// Optional <a href="https://tools.ietf.org/html/rfc5536#section-3.2.9">Organization</a> header
        /// </summary>
        public const string Organization = "Organization";

        /// <summary>
        /// Optional <a href="https://tools.ietf.org/html/rfc5536#section-3.2.10">References</a> header
        /// </summary>
        public const string References = "References";

        /// <summary>
        /// Optional <a href="https://tools.ietf.org/html/rfc5536#section-3.2.11">Summary</a> header
        /// </summary>
        public const string Summary = "Summary";

        /// <summary>
        /// Optional <a href="https://tools.ietf.org/html/rfc5536#section-3.2.12">Superseded</a> header
        /// </summary>
        public const string Supersedes = "Supersedes";

        /// <summary>
        /// Optional <a href="https://tools.ietf.org/html/rfc5536#section-3.2.13">User-Agent</a> header
        /// </summary>
        public const string UserAgent = "User-Agent";

        /// <summary>
        /// Optional <a href="https://tools.ietf.org/html/rfc5536#section-3.2.14">Xref</a> header
        /// </summary>
        public const string Xref = "Xref";
    }
}
