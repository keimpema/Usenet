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
    }
}
