using System;
using System.Collections.Generic;
using Usenet.Nzb;
using Xunit;

namespace UsenetTests.Nzb
{
    public class NzbFileTests
    {
        public static IEnumerable<object[]> EqualsWithSameValues = new[]
        {
            new object[]
            {
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName1",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null)),
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName1",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null))
            },
            new object[]
            {
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName2",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", new []
                    {
                        new NzbSegment(1, 1000, 1200, "1234567890@base.msg"),
                        new NzbSegment(2, 2000, 2200, "aaaaaaaaaa@base.msg"),
                    })),
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName2",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", new []
                    {
                        new NzbSegment(1, 1000, 1200, "1234567890@base.msg"),
                        new NzbSegment(2, 2000, 2200, "aaaaaaaaaa@base.msg"),
                    }))
            },
            new object[]
            {
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName3",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", new []
                    {
                        new NzbSegment(1, 1000, 1200, "1234567890@base.msg"),
                        new NzbSegment(2, 2000, 2200, "aaaaaaaaaa@base.msg"),
                    })),
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName3",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", new []
                    {
                        new NzbSegment(2, 2000, 2200, "aaaaaaaaaa@base.msg"),
                        new NzbSegment(1, 1000, 1200, "1234567890@base.msg"),
                    }))
            },
        };

        [Theory]
        [MemberData(nameof(EqualsWithSameValues))]
        public void EqualsWithSameValuesShouldReturnTrue(XSerializable<NzbFile> expected, XSerializable<NzbFile> actual)
        {
            Assert.Equal(expected.Object, actual.Object);
        }

        public static IEnumerable<object[]> EqualsWithDifferentValues = new[]
        {
            new object[]
            {
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null)),
                new XSerializable<NzbFile>(new NzbFile("blabla", "subject", "fileName",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null))
            },
            new object[]
            {
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null)),
                new XSerializable<NzbFile>(new NzbFile("poster", "blabla", "fileName",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null))
            },
            new object[]
            {
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null)),
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "blabla",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null))
            },
            new object[]
            {
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null)),
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName",
                    DateTimeOffset.MinValue, "group1;group2", null))
            },
            new object[]
            {
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null)),
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "blabla", null))
            },
            new object[]
            {
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", new []
                    {
                        new NzbSegment(1, 1000, 1200, "1234567890@base.msg"),
                        new NzbSegment(2, 2000, 2200, "aaaaaaaaaa@base.msg"),
                    })),
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", new []
                    {
                        new NzbSegment(1, 1000, 1200, "1234567890@base.msg"),
                    }))
            },
            new object[]
            {
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", new []
                    {
                        new NzbSegment(2, 2000, 2200, "aaaaaaaaaa@base.msg"),
                    })),
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", new []
                    {
                        new NzbSegment(2, 2000, 2200, "aaaaaaaaaa@base.msg"),
                        new NzbSegment(1, 1000, 1200, "1234567890@base.msg"),
                    }))
            },
            new object[]
            {
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", new []
                    {
                        new NzbSegment(2, 2000, 2200, "aaaaaaaaaa@base.msg"),
                    })),
                new XSerializable<NzbFile>(new NzbFile("poster", "subject", "fileName",
                    new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", new []
                    {
                        new NzbSegment(2, 2000, 2200, "bbbbbbbbbb@base.msg"),
                    }))
            },
        };

        [Theory]
        [MemberData(nameof(EqualsWithDifferentValues))]
        public void EqualsWithDifferentValuesShouldReturnFalse(XSerializable<NzbFile> expected, XSerializable<NzbFile> actual)
        {
            Assert.NotEqual(expected.Object, actual.Object);
        }

    }
}
