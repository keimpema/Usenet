using System;
using System.Collections.Generic;
using Lib;
using Usenet.Nntp.Models;
using Xunit;

namespace UsenetTests.Nntp.Models
{
    public class NntpGroupOriginTests
    {
        public static IEnumerable<object[]> EqualsWithSameValues = new[]
        {
            new object[]
            {
                new XSerializable<NntpGroupOrigin>(new NntpGroupOrigin("group1", new DateTimeOffset(2017, 5, 23, 15, 32, 11, TimeSpan.Zero), "me")),
                new XSerializable<NntpGroupOrigin>(new NntpGroupOrigin("group1", new DateTimeOffset(2017, 5, 23, 15, 32, 11, TimeSpan.Zero), "me")),
            },
        };

        [Theory]
        [MemberData(nameof(EqualsWithSameValues))]
        public void EqualsWithSameValuesShouldReturnTrue(XSerializable<NntpGroupOrigin> group1, XSerializable<NntpGroupOrigin> group2)
        {
            Assert.Equal(group1.Object, group2.Object);
        }

        public static IEnumerable<object[]> EqualsWithDifferentValues = new[]
        {
            new object[]
            {
                new XSerializable<NntpGroupOrigin>(new NntpGroupOrigin("group1", new DateTimeOffset(2017, 5, 23, 15, 32, 11, TimeSpan.Zero), "me")),
                new XSerializable<NntpGroupOrigin>(new NntpGroupOrigin("group2", new DateTimeOffset(2017, 5, 23, 15, 32, 11, TimeSpan.Zero), "me")),
            },
            new object[]
            {
                new XSerializable<NntpGroupOrigin>(new NntpGroupOrigin("group1", new DateTimeOffset(2017, 5, 23, 15, 32, 11, TimeSpan.Zero), "me")),
                new XSerializable<NntpGroupOrigin>(new NntpGroupOrigin("group1", new DateTimeOffset(2017, 5, 24, 15, 32, 11, TimeSpan.Zero), "me")),
            },
            new object[]
            {
                new XSerializable<NntpGroupOrigin>(new NntpGroupOrigin("group1", new DateTimeOffset(2017, 5, 23, 15, 32, 11, TimeSpan.Zero), "me")),
                new XSerializable<NntpGroupOrigin>(new NntpGroupOrigin("group1", new DateTimeOffset(2017, 5, 23, 15, 32, 11, TimeSpan.Zero), "not me")),
            },
        };

        [Theory]
        [MemberData(nameof(EqualsWithDifferentValues))]
        public void EqualsWithDifferentValuesShouldReturnFalse(XSerializable<NntpGroupOrigin> group1, XSerializable<NntpGroupOrigin> group2)
        {
            Assert.NotEqual(group1.Object, group2.Object);
        }
    }
}
