using Usenet.Nntp.Parsers;
using Usenet.Nntp.Responses;
using Xunit;

namespace UsenetTests.Nntp.Parsers
{
    public class NextResponseParserTests
    {
        [Theory]
        [InlineData(223, "123 <123@poster.com> retrieved", NntpNextResponseType.ArticleExists, 123, "123@poster.com")]
        [InlineData(412, "No newsgroup selected", NntpNextResponseType.NoGroupSelected, 0, "")]
        [InlineData(420, "No current article selected", NntpNextResponseType.CurrentArticleInvalid, 0, "")]
        [InlineData(421, "No next article to retrieve", NntpNextResponseType.NoNextArticleInGroup, 0, "")]
        [InlineData(999, "Unspecified response", NntpNextResponseType.Unknown, 0, "")]
        public void ResponseShouldBeParsedCorrectly(
            int responseCode, 
            string responseMessage,
            NntpNextResponseType expectedResponseType,
            long expectedArticleNumber, 
            string expectedMessageId)
        {
            NntpNextResponse nextResponse = new NextResponseParser().Parse(responseCode, responseMessage);
            Assert.Equal(expectedResponseType, nextResponse.ResponseType);
            Assert.Equal(expectedArticleNumber, nextResponse.Number);
            Assert.Equal(expectedMessageId, nextResponse.MessageId.Value);
        }
    }
}
