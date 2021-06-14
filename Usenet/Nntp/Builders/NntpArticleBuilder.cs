using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Usenet.Exceptions;
using Usenet.Extensions;
using Microsoft.Extensions.Logging;
using Usenet.Nntp.Models;
using Usenet.Util;

namespace Usenet.Nntp.Builders
{
    /// <summary>
    /// Represents a mutable <see cref="NntpArticle"/>.
    /// </summary>
    public class NntpArticleBuilder
    {
        private readonly ILogger log = Logger.Create<NntpArticleBuilder>();

        private const string dateFormat = "dd MMM yyyy HH:mm:ss";

        private static readonly string[] reservedHeaderKeys = {
            NntpHeaders.Date,
            NntpHeaders.From,
            NntpHeaders.Subject,
            NntpHeaders.MessageId,
            NntpHeaders.Newsgroups
        };

        private MultiValueDictionary<string, string> headers;
        private NntpGroupsBuilder groupsBuilder;
        private NntpMessageId messageId;
        private string from;
        private string subject;
        private DateTimeOffset? dateTime;
        private IEnumerable<string> body;

        /// <summary>
        /// Creates a new instance of the <see cref="NntpArticleBuilder"/> class.
        /// </summary>
        public NntpArticleBuilder()
        {
            headers = new MultiValueDictionary<string, string>();
            body = new List<string>();
            groupsBuilder = new NntpGroupsBuilder();
            messageId = NntpMessageId.Empty;
        }

        /// <summary>
        /// Initialize the <see cref="NntpArticleBuilder"/> from the given <see cref="NntpArticle"/>.
        /// All properties are overwritten.
        /// </summary>
        /// <param name="article">The <see cref="NntpArticle"/> to initialize the <see cref="NntpArticleBuilder"/> with.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder InitializeFrom(NntpArticle article)
        {
            Guard.ThrowIfNull(article, nameof(article));

            messageId = new NntpMessageId(article.MessageId.Value);
            groupsBuilder = new NntpGroupsBuilder().Add(article.Groups);
            headers = new MultiValueDictionary<string, string>();
            from = null;
            subject = null;
            dateTime = null;
            body = null;

            foreach (KeyValuePair<string, ImmutableHashSet<string>> header in article.Headers)
            {
                foreach (string value in header.Value)
                {
                    switch (header.Key)
                    {
                        case NntpHeaders.MessageId:
                            if (!messageId.HasValue)
                            {
                                messageId = value;
                            }
                            else
                            {
                                log.LogWarning("Found more than 1 {messageId} header. Skipping it.", NntpHeaders.MessageId);
                            }
                            break;

                        case NntpHeaders.From:
                            if (from == null)
                            {
                                from = value;
                            }
                            else
                            {
                                log.LogWarning("Found more than 1 {from} header. Skipping it.", NntpHeaders.From);
                            }
                            break;

                        case NntpHeaders.Subject:
                            if (subject == null)
                            {
                                subject = value;
                            }
                            else
                            {
                                log.LogWarning("Found more than 1 {subject} header. Skipping it.", NntpHeaders.Subject);
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
                                else
                                {
                                    log.LogWarning("{date} header has invalid value {value}. Skipping it.", NntpHeaders.Date, value);
                                }
                            }
                            else
                            {
                                log.LogWarning("Found more than 1 {date} header. Skipping it.", NntpHeaders.Date);
                            }
                            break;

                        case NntpHeaders.Newsgroups:
                            // convert group header to list of groups, do not add as header
                            groupsBuilder.Add(value);
                            break;

                        default:
                            headers.Add(header.Key, value);
                            break;
                    }
                }
            }

            // make copy of body
            body = article.Body.ToList();

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
        /// Sets the article's body to the provided enumerable collection of string lines.
        /// </summary>
        /// <param name="lines">An enumerable collection of string lines.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder SetBody(IEnumerable<string> lines)
        {
            body = lines.ThrowIfNull(nameof(lines));
            return this;
        }

        /// <summary>
        /// Add newsgroups to the required <see cref="NntpHeaders.Newsgroups"/> header.
        /// </summary>
        /// <param name="values">The groups to add to the <see cref="NntpHeaders.Newsgroups"/> header.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder AddGroups(params NntpGroups[] values)
        {
            Guard.ThrowIfNull(values, nameof(values));
            foreach (NntpGroups value in values)
            {
                groupsBuilder.Add(value);
            }
            return this;
        }

        /// <summary>
        /// Removes newsgroups from the <see cref="NntpHeaders.Newsgroups"/> header.
        /// </summary>
        /// <param name="values">The groups to remove from the <see cref="NntpHeaders.Newsgroups"/> header.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder RemoveGroups(params NntpGroups[] values)
        {
            Guard.ThrowIfNull(values, nameof(values));
            foreach (NntpGroups value in values)
            {
                groupsBuilder.Remove(value);
            }
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
        /// Adds a line to the body of the article. This will memoize the internal body collection if needed.
        /// </summary>
        /// <param name="line">The text line to add to the body.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder AddLine(string line)
        {
            Guard.ThrowIfNull(line, nameof(line));
            ICollection<string> bodyList = EnsureMemoizedBody();
            bodyList.Add(line);
            return this;
        }

        /// <summary>
        /// Adds multiple lines to the body of the article. This will memoize the internal body collection if needed.
        /// </summary>
        /// <param name="lines">The text lines to add to the body.</param>
        /// <returns>The <see cref="NntpArticleBuilder"/> so that additional calls can be chained.</returns>
        public NntpArticleBuilder AddLines(IEnumerable<string> lines)
        {
            Guard.ThrowIfNull(lines, nameof(lines));
            ICollection<string> bodyList = EnsureMemoizedBody();
            foreach (string line in lines)
            {
                bodyList.Add(line);
            }
            return this;
        }

        /// <summary>
        /// Creates a <see cref="NntpArticle"/> with al the properties from the <see cref="NntpArticleBuilder"/>.
        /// </summary>
        /// <returns>The <see cref="NntpArticle"/>.</returns>
        public NntpArticle Build()
        {
            if (!messageId.HasValue)
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
            if (groupsBuilder.IsEmpty)
            {
                throw new NntpException(Resources.Nntp.NewsgroupsHeaderNotSet);
            }

            NntpGroups groups = groupsBuilder.Build();

            headers.Add(NntpHeaders.From, from);
            headers.Add(NntpHeaders.Subject, subject);            

            if (dateTime.HasValue)
            {
                string formattedDate = dateTime.Value.ToUniversalTime().ToString(dateFormat);
                headers.Add(NntpHeaders.Date, $"{formattedDate} +0000");
            }

            return new NntpArticle(0, messageId, groups, headers, body);
        }

        private ICollection<string> EnsureMemoizedBody()
        {
            if (!(body is ICollection<string>))
            {
                // memoize the body lines
                body = body.ToList();
            }
            return (ICollection<string>)body;
        }
    }
}
