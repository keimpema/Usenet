using System;
using System.Collections.Generic;
using Usenet.Nntp.Parsers;
using Usenet.Nntp.Responses;
using Xunit;

namespace UsenetTests.Nntp.Parsers
{
    public class DateResponseParserTests
    {
        public static IEnumerable<object[]> ParseData = new[]
        {
            new object[] {111, "20170614110733", new DateTimeOffset(2017, 6, 14, 11, 7, 33, TimeSpan.Zero)},
            new object[] {111, "2017xxxxx10733", DateTimeOffset.MinValue},
            new object[] {111, "", DateTimeOffset.MinValue},
            new object[] {999, "20170614110733", DateTimeOffset.MinValue}
        };

        [Theory]
        [MemberData(nameof(ParseData))]
        public void ResponseShouldBeParsedCorrectly(
            int responseCode, 
            string responseMessage, 
            DateTimeOffset expectedDateTime)
        {
            NntpDateResponse dateResponse = new DateResponseParser().Parse(responseCode, responseMessage);
            Assert.Equal(expectedDateTime, dateResponse.DateTime);
        }
    }
}
