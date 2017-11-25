using System.Collections.Generic;
using System.Linq;
using Lib;
using Usenet.Nntp.Models;
using Usenet.Nntp.Parsers;
using Usenet.Nntp.Responses;
using Xunit;

namespace UsenetTests.Nntp.Parsers
{
    public class GroupsResponseParserTests
    {
        public static IEnumerable<object[]> MultiLineParseData = new[]
        {
            new object[]
            {
                231, "list of new newsgroups follows", 231, (int)GroupStatusRequestType.Basic,
                new []
                {
                    "alt.rfc-writers.recovery 4 1 y",
                    "tx.natives.recovery 89 56 y"
                },
                new XSerializable<NntpGroup[]>(new []
                {
                    new NntpGroup("alt.rfc-writers.recovery", 0, 1, 4, NntpPostingStatus.PostingPermitted, string.Empty, new List<long>(0)),
                    new NntpGroup("tx.natives.recovery", 0, 56, 89, NntpPostingStatus.PostingPermitted, string.Empty, new List<long>(0))
                })
            },
            new object[]
            {
                231, "list of new newsgroups follows", 231, (int)GroupStatusRequestType.Basic,
                new string[0], new XSerializable<NntpGroup[]>(new NntpGroup[0])
            },
            new object[]
            {
                215, "list of new newsgroups follows", 215, (int)GroupStatusRequestType.Extended,
                new []
                {
                    "misc.test 3002322 3000234 1234 y",
                    "rec.food.drink.tea 100 51 3 y"
                },
                new XSerializable<NntpGroup[]>(new []
                {
                    new NntpGroup("misc.test", 1234, 3000234, 3002322, NntpPostingStatus.PostingPermitted, string.Empty, new List<long>(0)),
                    new NntpGroup("rec.food.drink.tea", 3, 51, 100, NntpPostingStatus.PostingPermitted, string.Empty, new List<long>(0))
                })
            },
            new object[]
            {
                215, "list of new newsgroups follows", 215, (int)GroupStatusRequestType.Extended,
                new string[0], new XSerializable<NntpGroup[]>(new NntpGroup[0])
            }
        };

        [Theory]
        [MemberData(nameof(MultiLineParseData))]
        public void MultiLineResponseShouldBeParsedCorrectly(
            int responseCode,
            string responseMessage,
            int validCode,
            int requestType,
            string[] lines,
            XSerializable<NntpGroup[]> expectedGroups)
        {
            NntpGroupsResponse response = new GroupsResponseParser(validCode, (GroupStatusRequestType)requestType)
                .Parse(responseCode, responseMessage, lines.ToList());

            Assert.Equal(expectedGroups.Object, response.Groups);
        }
    }
}
