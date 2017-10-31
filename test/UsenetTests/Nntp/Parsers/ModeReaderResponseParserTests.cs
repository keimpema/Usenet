using Usenet.Nntp.Parsers;
using Usenet.Nntp.Responses;
using Xunit;

namespace UsenetTests.Nntp.Parsers
{
    public class ModeReaderResponseParserTests
    {
        [Theory]
        [InlineData(200, "Reader mode, posting permitted", NntpModeReaderResponseType.PostingAllowed)]
        [InlineData(201, "NNTP Service Ready, posting prohibited", NntpModeReaderResponseType.PostingProhibited)]
        [InlineData(502, "Transit service only", NntpModeReaderResponseType.ReadingServiceUnavailable)]
        [InlineData(999, "", NntpModeReaderResponseType.Unknown)]
        public void ResponseShouldBeParsedCorrectly(
            int responseCode, 
            string responseMessage, 
            NntpModeReaderResponseType expectedResponseType)
        {
            NntpModeReaderResponse modeReaderResponse = new ModeReaderResponseParser().Parse(responseCode, responseMessage);
            Assert.Equal(expectedResponseType, modeReaderResponse.ResponseType);
        }
    }
}
