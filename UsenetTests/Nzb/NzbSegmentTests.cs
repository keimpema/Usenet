using Usenet.Nzb;
using Xunit;

namespace UsenetTests.Nzb
{
    public class NzbSegmentTests
    {
        [Fact]
        public void EqualsWithSameValuesShouldReturnTrue()
        {
            var expected = new NzbSegment(1, 1000, 1200, "1234567890@base.msg");
            var actual = new NzbSegment(1, 1000, 1200, "1234567890@base.msg");
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 1000, 1200, "nomatch@bla.bla")]
        [InlineData(1, 1000, 1300, "1234567890@base.msg")]
        [InlineData(1, 1100, 1200, "1234567890@base.msg")]
        [InlineData(2, 1000, 1200, "1234567890@base.msg")]
        public void EqualsWithDifferentValuesShouldReturnFalse(int number, long offset, long size, string messageId)
        {
            var expected = new NzbSegment(1, 1000, 1200, "1234567890@base.msg");
            var actual = new NzbSegment(number, offset, size, messageId);
            Assert.NotEqual(expected, actual);
        }

    }
}
