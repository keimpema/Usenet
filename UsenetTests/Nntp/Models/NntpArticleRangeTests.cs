using System.Collections.Generic;
using Usenet.Nntp.Models;
using Xunit;

namespace UsenetTests.Nntp.Models
{
    public class NntpArticleRangeTests
    {
        [Fact]
        public void SingleArticleShouldHaveCorrectStringRepresentation()
        {
            const string expected = "8";
            string actual = NntpArticleRange.SingleArticle(8).ToString();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AllFollowingShouldHaveCorrectStringRepresentation()
        {
            const string expected = "8-";
            string actual = NntpArticleRange.AllFollowing(8).ToString();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RangeShouldHaveCorrectStringRepresentation()
        {
            const string expected = "8-88";
            string actual = NntpArticleRange.Range(8, 88).ToString();
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> EqualsWithSameValues = new[]
        {
            new object[]
            {
                new XSerializable<NntpArticleRange>(NntpArticleRange.SingleArticle(8)),
                new XSerializable<NntpArticleRange>(NntpArticleRange.SingleArticle(8)),
            },
            new object[]
            {
                new XSerializable<NntpArticleRange>(NntpArticleRange.Range(8, 88)),
                new XSerializable<NntpArticleRange>(NntpArticleRange.Range(8, 88)),
            },
            new object[]
            {
                new XSerializable<NntpArticleRange>(NntpArticleRange.AllFollowing(8)),
                new XSerializable<NntpArticleRange>(NntpArticleRange.AllFollowing(8)),
            },
        };

        [Theory]
        [MemberData(nameof(EqualsWithSameValues))]
        public void EqualsWithSameValuesShouldReturnTrue(XSerializable<NntpArticleRange> range1, XSerializable<NntpArticleRange> range2)
        {
            Assert.Equal(range1.Object, range2.Object);
        }
        public static IEnumerable<object[]> EqualsWithDifferentValues = new[]
        {
            new object[]
            {
                new XSerializable<NntpArticleRange>(NntpArticleRange.SingleArticle(8)),
                new XSerializable<NntpArticleRange>(NntpArticleRange.SingleArticle(9)),
            },
            new object[]
            {
                new XSerializable<NntpArticleRange>(NntpArticleRange.Range(8, 88)),
                new XSerializable<NntpArticleRange>(NntpArticleRange.Range(9, 88)),
            },
            new object[]
            {
                new XSerializable<NntpArticleRange>(NntpArticleRange.AllFollowing(8)),
                new XSerializable<NntpArticleRange>(NntpArticleRange.AllFollowing(9)),
            },
        };

        [Theory]
        [MemberData(nameof(EqualsWithDifferentValues))]
        public void EqualsWithDifferentValuesShouldReturnFalse(XSerializable<NntpArticleRange> range1, XSerializable<NntpArticleRange> range2)
        {
            Assert.NotEqual(range1.Object, range2.Object);
        }
    }
}
