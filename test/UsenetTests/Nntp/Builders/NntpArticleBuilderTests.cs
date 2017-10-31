using System;
using System.Collections.Generic;
using Usenet.Exceptions;
using Usenet.Nntp;
using Usenet.Nntp.Builders;
using Usenet.Nntp.Models;
using Usenet.Util;
using Xunit;

namespace UsenetTests.Nntp.Builders
{
    public class NntpArticleBuilderTests
    {
        [Fact]
        public void BuildingArticleWithoutMessageIdShouldThrow()
        {
            var exception = Assert.Throws<NntpException>(() =>
            {
                new NntpArticleBuilder()
                    .SetFrom("superuser")
                    .SetSubject("subject")
                    .AddGroup("alt.test")
                    .Build();
            });
            Assert.Equal($"{NntpHeaders.MessageId} header not set", exception.Message);
        }

        [Fact]
        public void BuildingArticleWithoutFromShouldThrow()
        {
            var exception = Assert.Throws<NntpException>(() =>
            {
                new NntpArticleBuilder()
                    .SetMessageId("123@hhh.net")
                    .SetSubject("subject")
                    .AddGroup("alt.test")
                    .Build();
            });
            Assert.Equal($"{NntpHeaders.From} header not set", exception.Message);
        }

        [Fact]
        public void BuildingArticleWithoutSubjectShouldThrow()
        {
            var exception = Assert.Throws<NntpException>(() =>
            {
                new NntpArticleBuilder()
                    .SetMessageId("123@hhh.net")
                    .SetFrom("superuser")
                    .AddGroup("alt.test")
                    .Build();
            });
            Assert.Equal($"{NntpHeaders.Subject} header not set", exception.Message);
        }

        [Fact]
        public void BuildingArticleWithoutGroupShouldThrow()
        {
            var exception = Assert.Throws<NntpException>(() =>
            {
                new NntpArticleBuilder()
                    .SetMessageId("123@hhh.net")
                    .SetFrom("superuser")
                    .SetSubject("subject")
                    .Build();
            });
            Assert.Equal($"{NntpHeaders.Newsgroups} header not set", exception.Message);
        }

        [Theory]
        [InlineData(NntpHeaders.Subject)]
        [InlineData(NntpHeaders.MessageId)]
        [InlineData(NntpHeaders.Newsgroups)]
        [InlineData(NntpHeaders.From)]
        public void BuildingArticleWithReservedHeaderKeyShouldThrow(string headerKey)
        {
            var exception = Assert.Throws<NntpException>(() =>
            {
                new NntpArticleBuilder().AddHeader(headerKey, "val");
            });
            Assert.Equal("Reserved header key not allowed", exception.Message);
        }

        [Theory]
        [InlineData("", "Val", "key")]
        [InlineData(" ", "Val", "key")]
        public void SettingHeaderWithEmptyParametersShouldThrow(string key, string value, string expectedParamName)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                new NntpArticleBuilder().AddHeader(key, value);
            });
            Assert.Equal(expectedParamName, exception.ParamName);
        }

        [Theory]
        [InlineData("Key", null, "value")]
        [InlineData(null, "Val", "key")]
        public void SettingHeaderWithNullParametersShouldThrow(string key, string value, string expectedParamName)
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new NntpArticleBuilder().AddHeader(key, value);
            });
            Assert.Equal(expectedParamName, exception.ParamName);
        }

        [Fact]
        public void ArticleBuilderShouldBuildArticle()
        {
            var expected = new NntpArticle(0, "123@hhh.net", new MultiValueDictionary<string, string>(() => new List<string>())
            {
                {NntpHeaders.Subject, "subject"},
                {NntpHeaders.From, "superuser"},
                {NntpHeaders.Newsgroups, "alt.test;alt.testclient"},
                {"Header1", "Value1" },
                {"Header1", "Value2" },
            }, null);

            NntpArticle actual = new NntpArticleBuilder()
                .SetMessageId("123@hhh.net")
                .SetFrom("superuser")
                .AddHeader("Header1", "Value2")
                .SetSubject("subject")
                .AddGroup("alt.test")
                .AddGroup("alt.testclient")
                .AddHeader("Header1", "Value1")
                .Build();


            Assert.Equal(expected, actual);
            Assert.True(expected.Equals(actual));
            Assert.True(expected == actual);
        }

        [Fact]
        public void ArticleBuilderInitializedFromExistingArticleShouldBuildSameArticle()
        {
            var expected = new NntpArticle(0, "123@hhh.net", new MultiValueDictionary<string, string>(() => new List<string>())
            {
                {NntpHeaders.Subject, "subject"},
                {NntpHeaders.From, "superuser"},
                {NntpHeaders.Newsgroups, "alt.test;alt.testclient"},
                {"Header1", "Value1" },
                {"Header1", "Value2" },
            }, null);

            NntpArticle actual = new NntpArticleBuilder()
                .InitializeFrom(expected)
                .Build();


            Assert.Equal(expected, actual);
            Assert.True(expected.Equals(actual));
            Assert.True(expected == actual);
        }

    }
}
