using System;
using System.Collections.Generic;
using Usenet.Nntp.Parsers;
using Xunit;

namespace UsenetTests.Nntp.Parsers
{
    public class HeaderDateParserTests
    {
        public static IEnumerable<object[]> ParseData = new[]
        {
            new object[] {"01 May 2017 13:55:33 +0000", new DateTimeOffset(2017, 5, 1, 13, 55, 33, TimeSpan.Zero)},
            new object[] {"01 May 2017 13:55:33 -0000", new DateTimeOffset(2017, 5, 1, 13, 55, 33, TimeSpan.Zero)},
            new object[] {"01 May 2017 13:55:33 +0100", new DateTimeOffset(2017, 5, 1, 13, 55, 33, TimeSpan.FromHours(1))},
            new object[] {"01 May 2017 13:55:33 -0100", new DateTimeOffset(2017, 5, 1, 13, 55, 33, TimeSpan.FromHours(-1))},

            new object[] {"01 May 2017 13:55 +1030", new DateTimeOffset(2017, 5, 1, 13, 55, 0, TimeSpan.FromMinutes(10 * 60 + 30))},
            new object[] {"01 May 2017 13:55 -1030", new DateTimeOffset(2017, 5, 1, 13, 55, 0, -TimeSpan.FromMinutes(10 * 60 + 30))},

            new object[] {"1 Jan 2017 00:00:00 +0000", new DateTimeOffset(2017, 1, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {"1 Feb 2017 00:00:00 +0000", new DateTimeOffset(2017, 2, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {"1 Mar 2017 00:00:00 +0000", new DateTimeOffset(2017, 3, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {"1 Apr 2017 00:00:00 +0000", new DateTimeOffset(2017, 4, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {"1 May 2017 00:00:00 +0000", new DateTimeOffset(2017, 5, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {"1 Jun 2017 00:00:00 +0000", new DateTimeOffset(2017, 6, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {"1 Jul 2017 00:00:00 +0000", new DateTimeOffset(2017, 7, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {"1 Aug 2017 00:00:00 +0000", new DateTimeOffset(2017, 8, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {"1 Sep 2017 00:00:00 +0000", new DateTimeOffset(2017, 9, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {"1 Oct 2017 00:00:00 +0000", new DateTimeOffset(2017, 10, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {"1 Nov 2017 00:00:00 +0000", new DateTimeOffset(2017, 11, 1, 0, 0, 0, TimeSpan.Zero)},
            new object[] {"1 Dec 2017 00:00:00 +0000", new DateTimeOffset(2017, 12, 1, 0, 0, 0, TimeSpan.Zero)},

            new object[] {"01 May 2017 13 : 55 : 33 +0000", new DateTimeOffset(2017, 5, 1, 13, 55, 33, TimeSpan.Zero)},
            new object[] {"01 May 2017 13 : 55 : 33 +0000", new DateTimeOffset(2017, 5, 1, 13, 55, 33, TimeSpan.Zero)},

        };

        public static IEnumerable<object[]> CenturyData = new[]
        {
            new object[] {$"01 May 16 13:55:33 +0000", new DateTimeOffset(2016, 5, 1, 13, 55, 33, TimeSpan.Zero)},
            new object[] {$"01 May 99 13:55:33 +0000", new DateTimeOffset(1999, 5, 1, 13, 55, 33, TimeSpan.Zero)},

        };

        [Theory]
        [MemberData(nameof(ParseData))]
        public void HeaderDateShouldBeParsedCorrectly(string headerDate, DateTimeOffset expectedDateTime)
        {
            DateTimeOffset? actualDateTime = HeaderDateParser.Parse(headerDate);
            Assert.Equal(expectedDateTime, actualDateTime);
        }

        [Fact]
        public void ObsoleteTwoDigitYearBeforeCurrentDateShouldBeParsedCorrectly()
        {
            DateTimeOffset yesterday = new DateTimeOffset(DateTime.UtcNow.Date.AddDays(-1), TimeSpan.Zero);
            DateTimeOffset expectedDate = yesterday;
            string headerValue = $"{yesterday:dd MMM yy HH:mm:ss} +0000";
            DateTimeOffset actualDate = HeaderDateParser.Parse(headerValue).GetValueOrDefault();
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ObsoleteTwoDigitYearAfterCurrentDateShouldBeParsedCorrectly()
        {
            DateTimeOffset tomorrow = new DateTimeOffset(DateTime.UtcNow.Date.AddDays(+1), TimeSpan.Zero);
            DateTimeOffset expectedDate = tomorrow.AddYears(-100);
            string headerValue = $"{tomorrow:dd MMM yy HH:mm:ss} +0000";
            DateTimeOffset actualDate = HeaderDateParser.Parse(headerValue).GetValueOrDefault();
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ObsoleteTwoDigitYearOnCurrentDateShouldBeParsedCorrectly()
        {
            var today = new DateTimeOffset(DateTime.UtcNow.Date, TimeSpan.Zero);
            DateTimeOffset expectedDate = today;
            string headerValue = $"{expectedDate:dd MMM yy HH:mm:ss} +0000";
            DateTimeOffset actualDate = HeaderDateParser.Parse(headerValue).GetValueOrDefault();
            Assert.Equal(expectedDate, actualDate);
        }

    }
}
