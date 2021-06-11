using System;
using System.Threading.Tasks;
using Usenet.Extensions;
using Usenet.Nntp.Models;
using Usenet.Nntp.Parsers;
using Usenet.Nntp.Responses;
using Usenet.Nntp.Writers;
using Usenet.Util;

namespace Usenet.Nntp
{
    public partial class NntpClient
    {
        /// <summary>
        /// Attempts to establish a <a href="https://tools.ietf.org/html/rfc3977#section-5.1">connection</a> with a usenet server.
        /// </summary>
        /// <param name="hostname">The hostname of the usenet server.</param>
        /// <param name="port">The port to use.</param>
        /// <param name="useSsl">A value to indicate whether or not to use SSL encryption.</param>
        /// <returns>true if a connection was made; otherwise false</returns>
        public async Task<bool> ConnectAsync(string hostname, int port, bool useSsl)
        {
            Guard.ThrowIfNullOrWhiteSpace(hostname, nameof(hostname));
            NntpResponse response = await connection.ConnectAsync(hostname, port, useSsl, new ResponseParser(200, 201));
            return response.Success;
        }

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-5.2">CAPABILITIES</a> 
        /// command allows a client to determine the capabilities of the server at any given time.
        /// </summary>
        /// <returns>A multi-line response containing the capabilities.</returns>
        public NntpMultiLineResponse Capabilities() => connection.MultiLineCommand("CAPABILITIES", new MultiLineResponseParser(101));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-5.2">CAPABILITIES</a> 
        /// command allows a client to determine the capabilities of the server at any given time.
        /// </summary>
        /// <param name="keyword">Can be provided for additional features if supported by the server.</param>
        /// <returns>A multi-line response containing the capabilities.</returns>
        public NntpMultiLineResponse Capabilities(string keyword) =>
            connection.MultiLineCommand($"CAPABILITIES {keyword.ThrowIfNullOrWhiteSpace(nameof(keyword))}", new MultiLineResponseParser(101));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-5.3">MODE READER</a> 
        /// (<a href="https://tools.ietf.org/html/rfc2980#section-2.3">ad 1</a>) command 
        /// instructs a mode-switching server to switch modes, as described in Section 3.4.2.
        /// </summary>
        /// <returns>A mode reader response object.</returns>
        public NntpModeReaderResponse ModeReader() => connection.Command("MODE READER", new ModeReaderResponseParser());

        /// <summary>
        /// The client uses the 
        /// <a href="https://tools.ietf.org/html/rfc3977#section-5.4">QUIT</a>
        /// command to terminate the session.
        /// </summary>
        /// <returns>A response object.</returns>
        public NntpResponse Quit() => connection.Command("QUIT", new ResponseParser(205));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.1.1">GROUP</a> 
        /// command selects a newsgroup as the currently selected newsgroup and returns summary information about it.
        /// </summary>
        /// <param name="group">The name of the group to select.</param>
        /// <returns>A group response object.</returns>
        public NntpGroupResponse Group(string group) =>
            connection.Command($"GROUP {group.ThrowIfNullOrWhiteSpace(nameof(group))}", new GroupResponseParser());

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.1.2">LISTGROUP</a> 
        /// command selects a newsgroup in the same manner as the
        /// GROUP command (see Section 6.1.1) but also provides a list of article
        /// numbers in the newsgroup. Only article numbers in the specified range are included in the list.
        /// </summary>
        /// <param name="group">The name of the group to select.</param>
        /// <param name="range">Only include article numbers within this range in the list.</param>
        /// <returns>A group response object.</returns>
        public NntpGroupResponse ListGroup(string group, NntpArticleRange range) =>
            connection.MultiLineCommand($"LISTGROUP {group.ThrowIfNullOrWhiteSpace(nameof(group))} {range}", new ListGroupResponseParser());

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.1.2">LISTGROUP</a> 
        /// command selects a newsgroup in the same manner as the
        /// GROUP command (see Section 6.1.1) but also provides a list of article
        /// numbers in the newsgroup.
        /// </summary>
        /// <param name="group">The name of the group to select.</param>
        /// <returns>A group response object.</returns>
        public NntpGroupResponse ListGroup(string group) =>
            connection.MultiLineCommand($"LISTGROUP {group.ThrowIfNullOrWhiteSpace(nameof(group))}", new ListGroupResponseParser());

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.1.2">LISTGROUP</a> 
        /// command without no group specified provides a list of article
        /// numbers in the newsgroup.
        /// </summary>
        /// <returns>A group response object.</returns>
        public NntpGroupResponse ListGroup() =>
            connection.MultiLineCommand("LISTGROUP", new ListGroupResponseParser());

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.1.3">LAST</a> command.
        /// If the currently selected newsgroup is valid, the current article
        /// number will be set to the previous article in that newsgroup (that
        /// is, the highest existing article number less than the current article
        /// number).
        /// </summary>
        /// <returns>A last response object.</returns>
        public NntpLastResponse Last() => connection.Command("LAST", new LastResponseParser());

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.1.4">NEXT</a> command.
        /// If the currently selected newsgroup is valid, the current article
        /// number will be set to the next article in that newsgroup (that is,
        /// the lowest existing article number greater than the current article
        /// number).
        /// </summary>
        /// <returns>A next response object.</returns>
        public NntpNextResponse Next() => connection.Command("NEXT", new NextResponseParser());

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.2.1">ARTICLE</a> command 
        /// selects an article according to the arguments and
        /// presents the entire article (that is, the headers, an empty line, and
        /// the body, in that order) to the client.
        /// </summary>
        /// <param name="messageId">The message-id of the article to received from the server.</param>
        /// <returns>An article response object.</returns>
        public NntpArticleResponse Article(NntpMessageId messageId) =>
            connection.MultiLineCommand(
                $"ARTICLE {messageId.ThrowIfNullOrWhiteSpace(nameof(messageId))}", 
                new ArticleResponseParser(ArticleRequestType.Article));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.2.1">ARTICLE</a> command 
        /// selects an article according to the arguments and
        /// presents the entire article (that is, the headers, an empty line, and
        /// the body, in that order) to the client.
        /// </summary>
        /// <param name="number">The number of the article to receive from the server.</param>
        /// <returns>An article response object.</returns>
        public NntpArticleResponse Article(long number) =>
            connection.MultiLineCommand($"ARTICLE {number}", new ArticleResponseParser(ArticleRequestType.Article));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.2.1">ARTICLE</a> command 
        /// selects an article according to the arguments and
        /// presents the entire article (that is, the headers, an empty line, and
        /// the body, in that order) to the client.
        /// </summary>
        /// <returns>An article response object.</returns>
        public NntpArticleResponse Article() =>
            connection.MultiLineCommand("ARTICLE", new ArticleResponseParser(ArticleRequestType.Article));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.2.2">HEAD</a> 
        /// command behaves identically to the ARTICLE command except
        /// that, if the article exists, the response code is 221 instead of 220
        /// and only the headers are presented (the empty line separating the
        /// headers and body MUST NOT be included).
        /// </summary>
        /// <param name="messageId">The message-id of the article to received from the server.</param>
        /// <returns>An article response object.</returns>
        public NntpArticleResponse Head(NntpMessageId messageId) =>
            connection.MultiLineCommand(
                $"HEAD {messageId.ThrowIfNullOrWhiteSpace(nameof(messageId))}", 
                new ArticleResponseParser(ArticleRequestType.Head));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.2.2">HEAD</a> 
        /// command behaves identically to the ARTICLE command except
        /// that, if the article exists, the response code is 221 instead of 220
        /// and only the headers are presented (the empty line separating the
        /// headers and body MUST NOT be included).
        /// </summary>
        /// <param name="number">The number of the article to receive from the server.</param>
        /// <returns>An article response object.</returns>
        public NntpArticleResponse Head(long number) =>
            connection.MultiLineCommand($"HEAD {number}", new ArticleResponseParser(ArticleRequestType.Head));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.2.2">HEAD</a> 
        /// command behaves identically to the ARTICLE command except
        /// that, if the article exists, the response code is 221 instead of 220
        /// and only the headers are presented (the empty line separating the
        /// headers and body MUST NOT be included).
        /// </summary>
        /// <returns>An article response object.</returns>
        public NntpArticleResponse Head() =>
            connection.MultiLineCommand("HEAD", new ArticleResponseParser(ArticleRequestType.Head));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.2.3">BODY</a> 
        /// command behaves identically to the ARTICLE command except
        /// that, if the article exists, the response code is 222 instead of 220
        /// and only the body is presented (the empty line separating the headers
        /// and body MUST NOT be included).
        /// </summary>
        /// <param name="messageId">The message-id of the article to received from the server.</param>
        /// <returns>An article response object.</returns>
        public NntpArticleResponse Body(NntpMessageId messageId) =>
            connection.MultiLineCommand(
                $"BODY {messageId.ThrowIfNullOrWhiteSpace(nameof(messageId))}", 
                new ArticleResponseParser(ArticleRequestType.Body));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.2.3">BODY</a> 
        /// command behaves identically to the ARTICLE command except
        /// that, if the article exists, the response code is 222 instead of 220
        /// and only the body is presented (the empty line separating the headers
        /// and body MUST NOT be included).
        /// See <a href="https://tools.ietf.org/html/rfc3977#section-6.2.3">RFC 3977</a> for more information.
        /// </summary>
        /// <param name="number">The number of the article to receive from the server.</param>
        /// <returns>An article response object.</returns>
        public NntpArticleResponse Body(long number) =>
            connection.MultiLineCommand($"BODY {number}", new ArticleResponseParser(ArticleRequestType.Body));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.2.3">BODY</a> 
        /// command behaves identically to the ARTICLE command except
        /// that, if the article exists, the response code is 222 instead of 220
        /// and only the body is presented (the empty line separating the headers
        /// and body MUST NOT be included).
        /// See <a href="https://tools.ietf.org/html/rfc3977#section-6.2.3">RFC 3977</a> for more information.
        /// </summary>
        /// <returns>An article response object.</returns>
        public NntpArticleResponse Body() =>
            connection.MultiLineCommand("BODY", new ArticleResponseParser(ArticleRequestType.Body));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.2.4">STAT</a> 
        /// command behaves identically to the ARTICLE command except
        /// that, if the article exists, it is NOT presented to the client and
        /// the response code is 223 instead of 220.  Note that the response is
        /// NOT multi-line.
        /// </summary>
        /// <param name="messageId">The message-id of the article to received from the server.</param>
        /// <returns>A stat response object.</returns>
        public NntpStatResponse Stat(NntpMessageId messageId) =>
            connection.Command($"STAT {messageId.ThrowIfNullOrWhiteSpace(nameof(messageId))}", new StatResponseParser());

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.2.4">STAT</a> 
        /// command behaves identically to the ARTICLE command except
        /// that, if the article exists, it is NOT presented to the client and
        /// the response code is 223 instead of 220.  Note that the response is
        /// NOT multi-line.
        /// </summary>
        /// <param name="number">The number of the article to receive from the server.</param>
        /// <returns>A stat response object.</returns>
        public NntpStatResponse Stat(long number) => connection.Command($"STAT {number}", new StatResponseParser());

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.2.4">STAT</a> 
        /// command behaves identically to the ARTICLE command except
        /// that, if the article exists, it is NOT presented to the client and
        /// the response code is 223 instead of 220.  Note that the response is
        /// NOT multi-line.
        /// </summary>
        /// <returns>A stat response object.</returns>
        public NntpStatResponse Stat() => connection.Command("STAT", new StatResponseParser());

        /// <summary>
        /// Post an article.
        /// <a href="https://tools.ietf.org/html/rfc3977#section-6.3.1">POST</a> an article.
        /// </summary>
        public bool Post(NntpArticle article)
        {
            NntpResponse initialResponse = connection.Command("POST", new ResponseParser(340));
            if (!initialResponse.Success)
            {
                return false;
            }
            ArticleWriter.Write(connection, article);
            NntpResponse subsequentResponse = connection.GetResponse(new ResponseParser(240));
            return subsequentResponse.Success;
        }

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-6.3.2">IHAVE</a> command 
        /// informs the server that the client has an article with the specified message-id.
        /// </summary>
        public bool Ihave(NntpArticle article)
        {
            NntpResponse initialResponse = connection.Command("IHAVE", new ResponseParser(335));
            if (!initialResponse.Success)
            {
                return false;
            }
            ArticleWriter.Write(connection, article);
            NntpResponse subsequentResponse = connection.GetResponse(new ResponseParser(235));
            return subsequentResponse.Success;
        }

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-7.1">DATE</a> 
        /// command exists to help clients find out the current Coordinated
        /// Universal Time [TF.686-1] from the server's perspective.
        /// </summary>
        /// <returns>A date response object.</returns>
        public NntpDateResponse Date() => connection.Command("DATE", new DateResponseParser());

        /// <summary>
        /// The <a herf="https://tools.ietf.org/html/rfc3977#section-7.2">HELP</a> 
        /// command provides a short summary of the commands that are
        /// understood by this implementation of the server.
        /// </summary>
        /// <returns>A multi-line response with a short summary of the available commands.</returns>
        public NntpMultiLineResponse Help() =>
            connection.MultiLineCommand("HELP", new MultiLineResponseParser(100));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-7.3">NEWGROUPS</a> 
        /// command returns a list of newsgroups created on the server since
        /// the specified date and time.
        /// </summary>
        /// <param name="sinceDateTime">List all newsgroups created since this date and time.</param>
        /// <returns>A groups response object.</returns>
        public NntpGroupsResponse NewGroups(NntpDateTime sinceDateTime) =>
            connection.MultiLineCommand($"NEWGROUPS {sinceDateTime}", new GroupsResponseParser(231, GroupStatusRequestType.Basic));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-7.4">NEWNEWS</a> 
        /// command returns a list of message-ids of articles posted or
        /// received on the server, in the newsgroups whose names match the
        /// wildmat, since the specified date and time.
        /// </summary>
        /// <param name="wildmat">The wildmat to use for filtering the group names.</param>
        /// <param name="sinceDateTime">List all newsgroups that have new articles 
        /// posted or received since this date and time.</param>
        /// <returns>A multi-line response containing a list of message-ids.</returns>
        public NntpMultiLineResponse NewNews(string wildmat, NntpDateTime sinceDateTime) =>
            connection.MultiLineCommand($"NEWNEWS {wildmat} {sinceDateTime}", new MultiLineResponseParser(230));


        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-7.6.4">active.times list</a> 
        /// (<a href="https://tools.ietf.org/html/rfc2980#section-2.1.3">ad 1</a>)
        /// is maintained by some NNTP servers to contain
        /// information about who created a particular newsgroup and when.
        /// </summary>
        /// <returns>A group origins response.</returns>
        public NntpGroupOriginsResponse ListActiveTimes() =>
            connection.MultiLineCommand("LIST ACTIVE.TIMES", new GroupOriginsResponseParser());

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-7.6.4">active.times list</a> 
        /// (<a href="https://tools.ietf.org/html/rfc2980#section-2.1.3">ad 1</a>)
        /// is maintained by some NNTP servers to contain
        /// information about who created a particular newsgroup and when.
        /// </summary>
        /// <param name="wildmat">The wildmat to use for filtering the group names.</param>
        /// <returns>A group origins response.</returns>
        public NntpGroupOriginsResponse ListActiveTimes(string wildmat) =>
            connection.MultiLineCommand($"LIST ACTIVE.TIMES {wildmat}", new GroupOriginsResponseParser());

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-7.6.5">distrib.pats list</a> 
        /// (<a href="https://tools.ietf.org/html/rfc2980#section-2.1.5">ad 1</a>)
        /// is maintained by some NNTP servers to assist
        /// clients to choose a value for the content of the Distribution header
        /// of a news article being posted.
        /// </summary>
        /// <returns>A multi-line response object containing newsgroup distribution information.</returns>
        public NntpMultiLineResponse ListDistribPats() =>
            connection.MultiLineCommand("LIST DISTRIB.PATS", new MultiLineResponseParser(215));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-7.6.6">newsgroups list</a> 
        /// (<a href="https://tools.ietf.org/html/rfc2980#section-2.1.6">ad 1</a>)
        /// is maintained by NNTP servers to contain the name
        /// of each newsgroup that is available on the server and a short
        /// description about the purpose of the group.
        /// </summary>
        /// <returns>A multi-line response containing a list of newsgroups available on the server.</returns>
        public NntpMultiLineResponse ListNewsgroups() =>
            connection.MultiLineCommand("LIST NEWSGROUPS", new MultiLineResponseParser(215));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-7.6.6">newsgroups list</a> 
        /// (<a href="https://tools.ietf.org/html/rfc2980#section-2.1.6">ad 1</a>)
        /// is maintained by NNTP servers to contain the name
        /// of each newsgroup that is available on the server and a short
        /// description about the purpose of the group.
        /// </summary>
        /// <param name="wildmat">The wildmat to use for filtering the group names.</param>
        /// <returns>A multi-line response containing a list of newsgroups available on the server.</returns>
        public NntpMultiLineResponse ListNewsgroups(string wildmat) =>
            connection.MultiLineCommand($"LIST NEWSGROUPS {wildmat}", new MultiLineResponseParser(215));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-8.3">OVER</a> 
        /// command returns the contents of all the fields in the
        /// database for an article specified by message-id, or from a specified
        /// article or range of articles in the currently selected newsgroup.
        /// </summary>
        /// <param name="messageId">The message-id of the article to received from the server.</param>
        /// <returns>A multi-line response containing header fields.</returns>
        public NntpMultiLineResponse Over(NntpMessageId messageId) =>
            connection.MultiLineCommand($"OVER {messageId}", new MultiLineResponseParser(224));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-8.3">OVER</a> 
        /// command returns the contents of all the fields in the
        /// database for an article specified by message-id, or from a specified
        /// article or range of articles in the currently selected newsgroup.
        /// </summary>
        /// <param name="range">Only include article numbers within this range in the list.</param>
        /// <returns>A multi-line response containing header fields.</returns>
        public NntpMultiLineResponse Over(NntpArticleRange range) =>
            connection.MultiLineCommand($"OVER {range}", new MultiLineResponseParser(224));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-8.3">OVER</a> 
        /// command returns the contents of all the fields in the
        /// database for an article specified by message-id, or from a specified
        /// article or range of articles in the currently selected newsgroup.
        /// </summary>
        /// <returns>A multi-line response containing header fields.</returns>
        public NntpMultiLineResponse Over() => connection.MultiLineCommand("OVER", new MultiLineResponseParser(224));


        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-8.4">LIST OVERVIEW.FMT</a> 
        /// (<a href="https://tools.ietf.org/html/rfc2980#section-2.1.7">ad 1</a>)
        /// command returns a description of the fields in
        /// the database for which it is consistent (as described above).
        /// </summary>
        /// <returns>A multi-line response containing a description of the fields 
        /// in the overview database for which it is consistent.</returns>
        public NntpMultiLineResponse ListOverviewFormat() =>
            connection.MultiLineCommand("LIST OVERVIEW.FMT", new MultiLineResponseParser(215));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-8.5">HDR</a> 
        /// command provides access to specific fields from an article
        /// specified by message-id, or from a specified article or range of
        /// articles in the currently selected newsgroup.
        /// </summary>
        /// <param name="field">The header field to retrieve.</param>
        /// <param name="messageId">The message-id of the article to received from the server.</param>
        /// <returns>A multi-line response containing the specfied header fields.</returns>
        public NntpMultiLineResponse Hdr(string field, NntpMessageId messageId) =>
            connection.MultiLineCommand($"HDR {field} {messageId}", new MultiLineResponseParser(225));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-8.5">HDR</a> 
        /// command provides access to specific fields from an article
        /// specified by message-id, or from a specified article or range of
        /// articles in the currently selected newsgroup.
        /// </summary>
        /// <param name="field">The header field to retrieve.</param>
        /// <param name="range">Only include article numbers within this range in the list.</param>
        /// <returns>A multi-line response containing the specfied header fields.</returns>
        public NntpMultiLineResponse Hdr(string field, NntpArticleRange range) =>
            connection.MultiLineCommand($"HDR {field} {range}", new MultiLineResponseParser(225));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-8.5">HDR</a> 
        /// command provides access to specific fields from an article
        /// specified by message-id, or from a specified article or range of
        /// articles in the currently selected newsgroup.
        /// </summary>
        /// <param name="field">The header field to retrieve.</param>
        /// <returns>A multi-line response containing the specfied header fields.</returns>
        public NntpMultiLineResponse Hdr(string field) =>
            connection.MultiLineCommand($"HDR {field}", new MultiLineResponseParser(225));


        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-8.6">LIST HEADERS</a> 
        /// command returns a list of fields that may be
        /// retrieved using the HDR command.
        /// </summary>
        /// <param name="messageId">The message-id of the article to received from the server.</param>
        /// <returns>A multi-line response containg a list of header 
        /// fields that may be retrieved using the HDR command.</returns>
        public NntpMultiLineResponse ListHeaders(NntpMessageId messageId) =>
            connection.MultiLineCommand($"LIST HEADERS {messageId}", new MultiLineResponseParser(215));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-8.6">LIST HEADERS</a> 
        /// command returns a list of fields that may be
        /// retrieved using the HDR command.
        /// </summary>
        /// <param name="range">Only include article numbers within this range in the list.</param>
        /// <returns>A multi-line response containg a list of header 
        /// fields that may be retrieved using the HDR command.</returns>
        public NntpMultiLineResponse ListHeaders(NntpArticleRange range) =>
            connection.MultiLineCommand($"LIST HEADERS {range}", new MultiLineResponseParser(215));

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc3977#section-8.6">LIST HEADERS</a> 
        /// command returns a list of fields that may be
        /// retrieved using the HDR command.
        /// </summary>
        /// <returns>A multi-line response containg a list of header 
        /// fields that may be retrieved using the HDR command.</returns>
        public NntpMultiLineResponse ListHeaders() =>
            connection.MultiLineCommand("LIST HEADERS", new MultiLineResponseParser(215));
    }
}
