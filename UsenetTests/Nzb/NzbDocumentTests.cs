using System;
using System.Collections.Generic;
using Usenet.Nzb;
using UsenetTests.TestHelpers;
using Xunit;

namespace UsenetTests.Nzb
{
    public class NzbDocumentTests
    {
        public static IEnumerable<object[]> EqualsWithSameValues = new[]
        {
            new object[]
            {
                new XSerializable<NzbDocument>(new NzbDocument(null, null)),
                new XSerializable<NzbDocument>(new NzbDocument(null, null)),
            },
            new object[]
            {
                new XSerializable<NzbDocument>(new NzbDocument(null, new[]
                {
                    new NzbFile("poster", "subject", "fileName1", new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null),
                    new NzbFile("poster", "subject", "fileName2", new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null),
                })),
                new XSerializable<NzbDocument>(new NzbDocument(null, new[]
                {
                    new NzbFile("poster", "subject", "fileName1", new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null),
                    new NzbFile("poster", "subject", "fileName2", new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null),
                })),
            },
            new object[]
            {
                new XSerializable<NzbDocument>(new NzbDocument(null, new[]
                {
                    new NzbFile("poster", "subject", "fileName3", new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null),
                    new NzbFile("poster", "subject", "fileName4", new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null),
                })),
                new XSerializable<NzbDocument>(new NzbDocument(null, new[]
                {
                    new NzbFile("poster", "subject", "fileName4", new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null),
                    new NzbFile("poster", "subject", "fileName3", new DateTimeOffset(2017, 12, 8, 22, 44, 0, TimeSpan.Zero), "group1;group2", null),
                })),
            },
        };

        [Theory]
        [MemberData(nameof(EqualsWithSameValues))]
        public void EqualsWithSameValuesShouldReturnTrue(XSerializable<NzbDocument> expected, XSerializable<NzbDocument> actual)
        {
            Assert.Equal(expected.Object, actual.Object);
        }
    }
}
