using System.Collections.Generic;
using System.Linq;
using TestLib;
using Usenet.Nntp.Models;
using Usenet.Nntp.Parsers;
using Usenet.Nntp.Responses;
using Usenet.Util;
using Xunit;

namespace UsenetTests.Nntp.Parsers
{
    public class ArticleResponseParserTests
    {
        public static IEnumerable<object[]> MultiLineParseData = new[]
        {
            new object[]
            {
                220, "123 <123@poster.com>", (int) ArticleRequestType.Article,
                new string[0],
                new XSerializable<NntpArticle>(new NntpArticle(123, "<123@poster.com>", null,
                    EmptyList<string>.Instance))
            },
            new object[]
            {
                220, "123 <123@poster.com>", (int) ArticleRequestType.Article,
                new[]
                {
                    "Path: pathost!demo!whitehouse!not-for-mail",
                    "From: \"Demo User\" <nobody@example.net>",
                    "",
                    "This is just a test article.",
                    "With two lines."
                },
                new XSerializable<NntpArticle>(new NntpArticle(123, "<123@poster.com>",
                    new MultiValueDictionary<string, string>(() => new List<string>())
                    {
                        {"Path", "pathost!demo!whitehouse!not-for-mail"},
                        {"From", "\"Demo User\" <nobody@example.net>"},
                    }, new List<string>
                    {
                        "This is just a test article.",
                        "With two lines."
                    }))
            },
            new object[]
            {
                222, "123 <123@poster.com>", (int) ArticleRequestType.Body,
                new[]
                {
                    "This is just a test article.",
                    "With two lines."
                },
                new XSerializable<NntpArticle>(new NntpArticle(123, "<123@poster.com>", null, new List<string>
                {
                    "This is just a test article.",
                    "With two lines."
                }))
            },
            new object[]
            {
                221, "123 <123@poster.com>", (int) ArticleRequestType.Head,
                new[]
                {
                    "Multi: line1",
                    " line2",
                    " line3",
                    "Path: pathost!demo!whitehouse!not-for-mail"
                },
                new XSerializable<NntpArticle>(new NntpArticle(123, "<123@poster.com>",
                    new MultiValueDictionary<string, string>(() => new List<string>())
                    {
                        {"Multi", "line1 line2 line3"},
                        {"Path", "pathost!demo!whitehouse!not-for-mail"},
                    }, EmptyList<string>.Instance))
            },
            new object[]
            {
                221, "123 <123@poster.com>", (int) ArticleRequestType.Head,
                new[]
                {
                    "Invalid header line",
                    "Path: pathost!demo!whitehouse!not-for-mail"
                },
                new XSerializable<NntpArticle>(new NntpArticle(123, "<123@poster.com>",
                    new MultiValueDictionary<string, string>(() => new List<string>())
                    {
                        {"Path", "pathost!demo!whitehouse!not-for-mail"},
                    }, EmptyList<string>.Instance))
            },
        };

        [Theory]
        [MemberData(nameof(MultiLineParseData))]
        public void MultiLineResponseShouldBeParsedCorrectly(
            int responseCode, 
            string responseMessage, 
            int requestType,
            string[] lines, 
            XSerializable<NntpArticle> expectedArticle)
        {
            NntpArticleResponse articleResponse = new ArticleResponseParser((ArticleRequestType)requestType)
                .Parse(responseCode, responseMessage, lines.ToList());
            NntpArticle actualArticle = articleResponse.Article;
            Assert.Equal(expectedArticle.Object, actualArticle);

            //Assert.Equal(expectedArticle.Number, actualArticle.Number);
            //Assert.Equal(expectedArticle.MessageId, actualArticle.MessageId);
            //Assert.Equal(expectedArticle.Headers, actualArticle.Headers);
            //Assert.Equal(expectedArticle.Body, actualArticle.Body);
        }

        public static IEnumerable<object[]> InvalidMultiLineParseData = new[]
        {
            new object[]
            {
                412, "No newsgroup selected", (int) ArticleRequestType.Article, new string[0],
            },
            new object[]
            {
                420, "No current article selected", (int) ArticleRequestType.Article, new string[0],
            },
            new object[]
            {
                423, "No article with that number", (int) ArticleRequestType.Article, new string[0],
            },
            new object[]
            {
                430, "No such article found", (int) ArticleRequestType.Article, new string[0],
            }
        };

        [Theory]
        [MemberData(nameof(InvalidMultiLineParseData))]
        public void InvalidMultiLineResponseWithShouldBeParsedCorrectly(
            int responseCode,
            string responseMessage,
            int requestType,
            string[] lines)
        {
            NntpArticleResponse articleResponse = new ArticleResponseParser((ArticleRequestType)requestType).Parse(
                responseCode, responseMessage, lines.ToList());
            Assert.Null(articleResponse.Article);
        }
    }
}
