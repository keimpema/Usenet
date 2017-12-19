using System;
using System.Collections.Generic;
using Usenet.Nntp.Models;
using Xunit;

namespace UsenetTests.Nntp.Models
{
    public class NntpDateTimeTests
    {
        public static IEnumerable<object[]> DateTimeData = new[]
        {
            new object[] { "20170523 153211 GMT", new DateTime(2017, 5, 23, 15, 32, 11, DateTimeKind.Utc) },
            new object[] { "20170523 153211 GMT", new DateTime(2017, 5, 23, 15, 32, 11, DateTimeKind.Utc).ToLocalTime() }
        };

        [Theory]
        [MemberData(nameof(DateTimeData))]
        public void DateTimeShouldBeConvertedToUsenetString(string expected, DateTime dateTime)
        {
            var actual = (NntpDateTime) dateTime;
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> DateTimeOffsetData = new[]
        {
            new object[] { "20170523 153211 GMT", new DateTimeOffset(2017, 5, 23, 15, 32, 11, TimeSpan.Zero),  },
            new object[] { "20170523 153211 GMT", new DateTimeOffset(2017, 5, 23, 17, 32, 11, TimeSpan.FromHours(+2)) }
        };

        [Theory]
        [MemberData(nameof(DateTimeOffsetData))]
        public void DateTimeOffsetShouldBeConvertedToUsenetString(string expected, DateTimeOffset dateTime)
        {
            var actual = (NntpDateTime)dateTime;
            Assert.Equal(expected, actual);
        }
    }
}
