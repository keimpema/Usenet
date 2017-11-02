using System.Collections.Generic;
using System.Linq;
using Lib;
using Usenet.Nntp.Models;
using Usenet.Nntp.Parsers;
using Usenet.Nntp.Responses;
using Xunit;

namespace UsenetTests.Nntp.Parsers
{
    public class ListGroupsReponseParserTest
    {
        public static IEnumerable<object[]> MultiLineParseData = new[]
        {
            new object[]
            {
                211, "1234 3000234 3002322 misc.test list follows", new string[0],
                new XSerializable<NntpGroup>(new NntpGroup("misc.test", 1234, 3000234, 3002322, NntpPostingStatus.Unknown, string.Empty, new List<int>(0)))
            },
            new object[]
            {
                211, "1234 3000234 3000236 misc.test list follows", new [] {"3000234", "3000235", "3000236"},
                new XSerializable<NntpGroup>(new NntpGroup("misc.test", 1234, 3000234, 3000236, NntpPostingStatus.Unknown, string.Empty, new[] {3000234, 3000235, 3000236}))
            },
            new object[]
            {
                411, "example.is.sob.bradner.or.barber is unknown", new string[0],
                new XSerializable<NntpGroup>(new NntpGroup("", 0, 0, 0, NntpPostingStatus.Unknown, string.Empty, new List<int>(0)))
            },
            new object[]
            {
                412, "no newsgroup selected", new string[0],
                new XSerializable<NntpGroup>(new NntpGroup("", 0, 0, 0, NntpPostingStatus.Unknown, string.Empty, new List<int>(0)))
            }
        };

        [Theory]
        [MemberData(nameof(MultiLineParseData))]
        public void MultiLineResponseShouldBeParsedCorrectly(
            int responseCode,
            string responseMessage,
            string[] lines,
            XSerializable<NntpGroup> expectedGroup)
        {
            NntpGroupResponse groupResponse = new ListGroupResponseParser().Parse(responseCode, responseMessage, lines.ToList());
            Assert.Equal(expectedGroup.Object, groupResponse.Group);
        }
    }
}
