using System;
using System.Collections.Generic;
using System.Linq;
using Usenet.Nntp.Models;
using Usenet.Nntp.Parsers;
using Usenet.Nntp.Responses;
using Xunit;

namespace UsenetTests.Nntp.Parsers
{
    public class GroupOriginsResponseParserTests
    {
        public static IEnumerable<object[]> MultiLineParseData = new[]
        {
            new object[]
            {
                215, "information follows",
                new []
                {
                    "misc.test 930445408 <creatme@isc.org>",
                    "alt.rfc-writers.recovery 930562309 <m@example.com>",
                    "tx.natives.recovery 930678923 <sob@academ.com>"
                },
                new XSerializable<NntpGroupOrigin[]>(new []
                {
                    new NntpGroupOrigin("misc.test", new DateTimeOffset(1999, 6, 27, 1, 3, 28, 0, TimeSpan.Zero), "<creatme@isc.org>"),
                    new NntpGroupOrigin("alt.rfc-writers.recovery", new DateTimeOffset(1999, 6, 28, 9, 31, 49, 0, TimeSpan.Zero), "<m@example.com>"),
                    new NntpGroupOrigin("tx.natives.recovery", new DateTimeOffset(1999, 6, 29, 17, 55, 23, 0, TimeSpan.Zero), "<sob@academ.com>")
                })
            }
        };

        [Theory]
        [MemberData(nameof(MultiLineParseData))]
        public void ResponseShouldBeParsedCorrectly(
            int responseCode, 
            string responseMessage, 
            string[] lines,
            XSerializable<NntpGroupOrigin[]> expectedGroupOrigins)
        {
            NntpGroupOriginsResponse response = new GroupOriginsResponseParser().Parse(
                responseCode, responseMessage, lines.ToList());
            Assert.Equal(expectedGroupOrigins.Object, response.GroupOrigins);
        }
    }
}
