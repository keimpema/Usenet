using Usenet.Nntp.Parsers;
using Usenet.Nntp.Responses;
using Xunit;

namespace UsenetTests.Nntp.Parsers
{
    public class StatResponseParserTests
    {
        [Theory]
        [InlineData(223, "123 <123@poster.com>", NntpStatResponseType.ArticleExists, 123, "123@poster.com")]
        [InlineData(412, "No newsgroup selected", NntpStatResponseType.NoGroupSelected, 0, "")]
        [InlineData(420, "No current article selected", NntpStatResponseType.CurrentArticleInvalid, 0, "")]
        [InlineData(423, "No article with that number", NntpStatResponseType.NoArticleWithThatNumber, 0, "")]
        [InlineData(430, "No such article found", NntpStatResponseType.NoArticleWithThatMessageId, 0, "")]
        [InlineData(999, "Unspecified response", NntpStatResponseType.Unknown, 0, "")]
        public void ResponseShouldBeParsedCorrectly(
            int responseCode, 
            string responseMessage, 
            NntpStatResponseType expectedResponseType,
            long expectedArticleNumber, 
            string expectedMessageId)
        {
            NntpStatResponse statResponse = new StatResponseParser().Parse(responseCode, responseMessage);
            Assert.Equal(expectedResponseType, statResponse.ResponseType);
            Assert.Equal(expectedArticleNumber, statResponse.Number);
            Assert.Equal(expectedMessageId, statResponse.MessageId.Value);
        }
    }
}
