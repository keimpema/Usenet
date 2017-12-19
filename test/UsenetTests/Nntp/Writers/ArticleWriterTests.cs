using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib;
using Usenet.Nntp;
using Usenet.Nntp.Models;
using Usenet.Nntp.Parsers;
using Usenet.Nntp.Writers;
using Usenet.Util;
using Xunit;

namespace UsenetTests.Nntp.Writers
{
    public class ArticleWriterTests
    {
        public static IEnumerable<object[]> ArticleWriteData = new[]
        {
            new object[] 
            {
                new XSerializable<NntpArticle>(new NntpArticle(0, "1@example.com", "group", null, new List<string>(0))), new []
                {
                    "Message-ID: <1@example.com>",
                    "Newsgroups: group",
                    "",
                    "."
                }
            },
            new object[]
            {
                new XSerializable<NntpArticle>(new NntpArticle(0, "<2@example.com>", "group", null, new List<string>(0))), new []
                {
                    "Message-ID: <2@example.com>",
                    "Newsgroups: group",
                    "",
                    "."
                }
            },
            new object[]
            {
                new XSerializable<NntpArticle>(new NntpArticle(0, "3@example.com", "group", new MultiValueDictionary<string, string>
                {
                    { "From", "\"Demo User\" <nobody@example.net>"},
                }, new List<string>
                {
                    "This is just a test article."
                })), new []
                {
                    "Message-ID: <3@example.com>",
                    "Newsgroups: group",
                    "From: \"Demo User\" <nobody@example.net>",
                    "",
                    "This is just a test article.",
                    "."
                },
            },
            new object[]
            {
                new XSerializable<NntpArticle>(new NntpArticle(0, "4@example.com", "group", new MultiValueDictionary<string, string>
                {
                    { "Message-ID", "<9999@example.com>"}, // not allowed, should be ignored
                }, new List<string>
                {
                    "This is just a test article."
                })), new []
                {
                    "Message-ID: <4@example.com>",
                    "Newsgroups: group",
                    "",
                    "This is just a test article.",
                    "."
                },
            },
            new object[]
            {
                new XSerializable<NntpArticle>(new NntpArticle(0, "5@example.com", "group", new MultiValueDictionary<string, string>
                {
                    { "Message-ID", "9999@example.com"}, // not allowed, should be ignored
                }, new List<string>
                {
                    "This is just a test article."
                })), new []
                {
                    "Message-ID: <5@example.com>",
                    "Newsgroups: group",
                    "",
                    "This is just a test article.",
                    "."
                },
            },
        };

        [Theory]
        [MemberData(nameof(ArticleWriteData))]
        public void ArticleShouldBeWrittenCorrectly(XSerializable<NntpArticle> article, string[] expectedLines)
        {
            var connection = new MockConnection();
            ArticleWriter.Write(connection, article.Object);
            Assert.Equal(expectedLines, connection.GetLines());
        }
    }

    public class MockConnection : INntpConnection
    {
        private readonly List<string> lines = new List<string>();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> ConnectAsync<TResponse>(string hostname, int port, bool useSsl, IResponseParser<TResponse> parser)
        {
            throw new NotImplementedException();
        }

        public TResponse Command<TResponse>(string command, IResponseParser<TResponse> parser)
        {
            throw new NotImplementedException();
        }

        public TResponse MultiLineCommand<TResponse>(string command, IMultiLineResponseParser<TResponse> parser)
        {
            throw new NotImplementedException();
        }

        public void WriteLine(string line)
        {
            lines.Add(line);
        }

        public TResponse GetResponse<TResponse>(IResponseParser<TResponse> parser)
        {
            throw new NotImplementedException();
        }

        public string[] GetLines()
        {
            return lines.ToArray();
        }
    }
}
