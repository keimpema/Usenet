using Usenet.Nntp.Parsers;
using Usenet.Nntp.Responses;
using Xunit;

namespace UsenetTests.Nntp.Parsers
{
    public class LastResponseParserTests
    {
        [Theory]
        [InlineData(223, "123 <123@poster.com> retrieved", NntpLastResponseType.ArticleExists, 123, "123@poster.com")]
        [InlineData(412, "No newsgroup selected", NntpLastResponseType.NoGroupSelected, 0, "")]
        [InlineData(420, "No current article selected", NntpLastResponseType.CurrentArticleInvalid, 0, "")]
        [InlineData(422, "No previous article to retrieve", NntpLastResponseType.NoPreviousArticleInGroup, 0, "")]
        [InlineData(999, "Unspecified response", NntpLastResponseType.Unknown, 0, "")]
        public void ResponseShouldBeParsedCorrectly(
            int responseCode, 
            string responseMessage,
            NntpLastResponseType expectedResponseType, 
            int expectedArticleNumber, 
            string expectedMessageId)
        {
            NntpLastResponse lastResponse = new LastResponseParser().Parse(responseCode, responseMessage);
            Assert.Equal(expectedResponseType, lastResponse.ResponseType);
            Assert.Equal(expectedArticleNumber, lastResponse.Number);
            Assert.Equal(expectedMessageId, lastResponse.MessageId.Value);
        }
    }
}
