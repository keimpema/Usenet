using System;
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
        public void BuildWithoutMessageIdShouldThrow()
        {
            var exception = Assert.Throws<NntpException>(() =>
            {
                new NntpArticleBuilder()
                    .SetFrom("superuser")
                    .SetSubject("subject")
                    .AddGroups("alt.test")
                    .Build();
            });
            Assert.Equal($"{NntpHeaders.MessageId} header not set", exception.Message);
        }

        [Fact]
        public void BuildWithoutFromShouldThrow()
        {
            var exception = Assert.Throws<NntpException>(() =>
            {
                new NntpArticleBuilder()
                    .SetMessageId("123@hhh.net")
                    .SetSubject("subject")
                    .AddGroups("alt.test")
                    .Build();
            });
            Assert.Equal($"{NntpHeaders.From} header not set", exception.Message);
        }

        [Fact]
        public void BuildWithoutSubjectShouldThrow()
        {
            var exception = Assert.Throws<NntpException>(() =>
            {
                new NntpArticleBuilder()
                    .SetMessageId("123@hhh.net")
                    .SetFrom("superuser")
                    .AddGroups("alt.test")
                    .Build();
            });
            Assert.Equal($"{NntpHeaders.Subject} header not set", exception.Message);
        }

        [Fact]
        public void BuildWithoutGroupShouldThrow()
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
        public void SettingHeaderWithReservedKeyShouldThrow(string headerKey)
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
        public void BuildShouldBuildArticle()
        {
            var expected = new NntpArticle(0, "123@hhh.net", "alt.test;alt.testclient", new MultiValueDictionary<string, string>
            {
                {NntpHeaders.Subject, "subject"},
                {NntpHeaders.From, "superuser"},
                {"Header1", "Value1" },
                {"Header1", "Value2" },
            }, null);

            NntpArticle actual = new NntpArticleBuilder()
                .SetMessageId("123@hhh.net")
                .SetFrom("superuser")
                .AddHeader("Header1", "Value2")
                .SetSubject("subject")
                .AddGroups("alt.test")
                .AddGroups("alt.testclient")
                .AddHeader("Header1", "Value1")
                .Build();


            Assert.Equal(expected, actual);
            Assert.True(expected.Equals(actual));
            Assert.True(expected == actual);
        }

        [Fact]
        public void BuildInitializedFromExistingArticleShouldBuildSameArticle()
        {
            var expected = new NntpArticle(0, "123@hhh.net", "alt.test;alt.testclient", new MultiValueDictionary<string, string>
            {
                {NntpHeaders.Subject, "subject"},
                {NntpHeaders.From, "superuser"},
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
