using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Usenet.Exceptions;
using Usenet.Extensions;
using Usenet.Nntp.Models;
using Usenet.Util;

namespace Usenet.Nntp.Builders
{
    /// <summary>
    /// Represents a mutable <see cref="NntpArticle"/>.
    /// </summary>
    public class NntpArticleBuilder
    {
        private const string groupSeperator = ";";
        private const string dateFormat = "dd MMM yyyy HH:mm:ss";

        private static readonly string[] reservedHeaderKeys = {
            NntpHeaders.From,
            NntpHeaders.Subject,
            NntpHeaders.MessageId,
            NntpHeaders.Newsgroups
        };

        private NntpMessageId messageId;
        private string from;
        private string subject;
        private DateTimeOffset? dateTime;

        private readonly MultiValueDictionary<string, string> headers;
        private readonly List<string> body;
        private readonly List<string> groups;

        /// <summary>
        /// Creates a new instance of the <see cref="NntpArticleBuilder"/> class.
        /// </summary>
        public NntpArticleBuilder()
        {
            headers = new MultiValueDictionary<string, string>(() => new List<string>());
            body = new List<string>();
            groups = new List<string>();
        }

        /// <summary>
        /// Initialize the <see cref="NntpArticleBuilder"/> from the given <see cref="NntpArticle"/>.
        /// All properties are overwritten.
        /// </summary>
        /// <param name="article">The <see cref="NntpArticle"/> to initialize the <see cref="NntpArticleBuilder"/> with.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder InitializeFrom(NntpArticle article)
        {
            messageId = null;
            from = null;
            subject = null;

            headers.Clear();
            body.Clear();
            groups.Clear();

            foreach (KeyValuePair<string, ICollection<string>> header in article.Headers)
            {
                foreach (string value in header.Value)
                {
                    switch (header.Key)
                    {
                        case NntpHeaders.MessageId:
                            // skip additional messageid's
                            break;

                        case NntpHeaders.From:
                            if (from == null)
                            {
                                from = value;
                            }
                            break;

                        case NntpHeaders.Subject:
                            if (subject == null)
                            {
                                subject = value;
                            }
                            break;

                        case NntpHeaders.Date:
                            if (dateTime == null)
                            {

                                if (DateTimeOffset.TryParseExact(value, dateFormat, CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out DateTimeOffset headerDateTime))
                                {
                                    dateTime = headerDateTime;
                                }
                            }
                            break;

                        case NntpHeaders.Newsgroups:
                            // convert group header to list of groups, do not add as header
                            AddGroups(value);
                            break;

                        default:
                            headers.Add(header.Key, value);
                            break;
                    }
                }
            }

            body.AddRange(article.Body);
            messageId = article.MessageId;

            return this;
        }

        /// <summary>
        /// Sets the article's required <see cref="NntpHeaders.MessageId"/> header.
        /// </summary>
        /// <param name="value">The <see cref="NntpHeaders.MessageId"/> header value.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder SetMessageId(NntpMessageId value)
        {
            messageId = value.ThrowIfNullOrWhiteSpace(nameof(value));
            return this;
        }

        /// <summary>
        /// Sets the article's required <see cref="NntpHeaders.From"/> header.
        /// </summary>
        /// <param name="value">The <see cref="NntpHeaders.From"/> header value.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder SetFrom(string value)
        {
            from = value.ThrowIfNullOrWhiteSpace(nameof(value));
            return this;
        }

        /// <summary>
        /// Sets the article's required <see cref="NntpHeaders.Subject"/> header.
        /// </summary>
        /// <param name="value">The <see cref="NntpHeaders.Subject"/> header value.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder SetSubject(string value)
        {
            subject = value.ThrowIfNullOrWhiteSpace(nameof(value));
            return this;
        }

        /// <summary>
        /// Sets the required <see cref="NntpHeaders.Date"/> header.
        /// </summary>
        /// <param name="value">The <see cref="NntpHeaders.Date"/> header value.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder SetDate(DateTimeOffset value)
        {
            dateTime = value;
            return this;
        }

        /// <summary>
        /// Adds a newsgroup to the required <see cref="NntpHeaders.Newsgroups"/> header.
        /// </summary>
        /// <param name="value">The group to add to the <see cref="NntpHeaders.Newsgroups"/> header.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder AddGroup(string value)
        {
            Guard.ThrowIfNull(value, nameof(value));
            AddGroups(value);
            return this;
        }

        /// <summary>
        /// Removes a newsgroup from the <see cref="NntpHeaders.Newsgroups"/> header.
        /// </summary>
        /// <param name="value">The group to remove from the <see cref="NntpHeaders.Newsgroups"/> header.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder RemoveGroup(string value)
        {
            Guard.ThrowIfNull(value, nameof(value));
            RemoveGroups(value);
            return this;
        }

        /// <summary>
        /// Adds a header to the article.
        /// </summary>
        /// <param name="key">The key of the header to add.</param>
        /// <param name="value">The value of the header to add.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder AddHeader(string key, string value)
        {
            Guard.ThrowIfNullOrWhiteSpace(key, nameof(key));
            Guard.ThrowIfNull(value, nameof(value));
            if (reservedHeaderKeys.Contains(key))
            {
                throw new NntpException(Resources.Nntp.ReservedHeaderKeyNotAllowed);
            }
            headers.Add(key, value);
            return this;
        }

        /// <summary>
        /// Removes a header from the article with the given key and value. If no
        /// value is provided all headers with the given key are removed.
        /// </summary>
        /// <param name="key">The key of the header(s) to remove.</param>
        /// <param name="value">The value of the header to remove.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder RemoveHeader(string key, string value = null)
        {
            Guard.ThrowIfNullOrWhiteSpace(key, nameof(key));
            if (reservedHeaderKeys.Contains(key))
            {
                throw new NntpException(Resources.Nntp.ReservedHeaderKeyNotAllowed);
            }
            headers.Remove(key, value);
            return this;
        }

        /// <summary>
        /// Adds a line to the body of the article.
        /// </summary>
        /// <param name="line">The text line to add to the body.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder AddLine(string line)
        {
            Guard.ThrowIfNull(line, nameof(line));
            body.Add(line);
            return this;
        }

        /// <summary>
        /// Adds multiple lines to the body of the article.
        /// </summary>
        /// <param name="lines">The text lines to add to the body.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder AddLines(IEnumerable<string> lines)
        {
            Guard.ThrowIfNull(lines, nameof(lines));
            body.AddRange(lines);
            return this;
        }

        /// <summary>
        /// Creates a <see cref="NntpArticle"/> with al the properties from the <see cref="NntpArticleBuilder"/>.
        /// </summary>
        /// <returns>The <see cref="NntpArticle"/>.</returns>
        public NntpArticle Build()
        {
            if (string.IsNullOrWhiteSpace(messageId))
            {
                throw new NntpException(Resources.Nntp.MessageIdHeaderNotSet);
            }
            if (string.IsNullOrWhiteSpace(from))
            {
                throw new NntpException(Resources.Nntp.FromHeaderNotSet);
            }
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new NntpException(Resources.Nntp.SubjectHeaderNotSet);
            }
            if (groups.Count == 0)
            {
                throw new NntpException(Resources.Nntp.NewsgroupsHeaderNotSet);
            }

            headers.Add(NntpHeaders.From, from);
            headers.Add(NntpHeaders.Subject, subject);            
            headers.Add(NntpHeaders.Newsgroups, string.Join(groupSeperator, groups));

            if (dateTime.HasValue)
            {
                string formattedDate = dateTime.Value.ToUniversalTime().ToString(dateFormat);
                headers.Add(NntpHeaders.Date, $"{formattedDate} +0000");
            }

            return new NntpArticle(0, messageId, headers, body);
        }

        private void AddGroups(string value)
        {
            foreach (string group in value.Split(new[] { groupSeperator }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!groups.Contains(group))
                {
                    groups.Add(group);
                }
            }
        }

        private void RemoveGroups(string value)
        {
            foreach (string group in value.Split(new[] { groupSeperator }, StringSplitOptions.RemoveEmptyEntries))
            {
                groups.RemoveAll(g => g == group);
            }
        }
    }
}
