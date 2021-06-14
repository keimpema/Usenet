using System.IO;
using System.Linq;
using Usenet.Util;
using Usenet.Yenc;
using UsenetTests.Extensions;
using UsenetTests.TestHelpers;
using Xunit;

namespace UsenetTests.Yenc
{
    public class YencArticleDecoderTests : IClassFixture<TestData>
    {
        private readonly TestData testData;

        public YencArticleDecoderTests(TestData testData)
        {
            this.testData = testData;
        }

        [Fact]
        public void SinglePartFileShouldBeDecoded()
        {
            byte[] expectedData = testData.GetEmbeddedFile(@"yenc.singlepart.testfile.txt").ReadAllBytes();

            YencArticle actualArticle = YencArticleDecoder.Decode(
                testData.GetEmbeddedFile(@"yenc.singlepart.00000005.ntx").ReadAllLines(UsenetEncoding.Default));

            Assert.False(actualArticle.Header.IsFilePart);
            Assert.Equal(128, actualArticle.Header.LineLength);
            Assert.Equal(584, actualArticle.Header.FileSize);
            Assert.Equal("testfile.txt", actualArticle.Header.FileName);
            Assert.Equal(584, actualArticle.Footer.PartSize);
            Assert.Equal("ded29f4f", ((int) actualArticle.Footer.Crc32.GetValueOrDefault()).ToString("x"));
            Assert.Equal(expectedData, actualArticle.Data);
        }

        [Fact]
        public void FilePartShouldBeDecoded()
        {
            const int expectedDataLength = 11250;

            YencArticle actualArticle = YencArticleDecoder.Decode(
                testData.GetEmbeddedFile(@"yenc.multipart.00000020.ntx").ReadAllLines(UsenetEncoding.Default));

            Assert.True(actualArticle.Header.IsFilePart);
            Assert.Equal(expectedDataLength, actualArticle.Data.Length);
        }

        [Fact]
        public void MultiPartFileShouldBeDecoded()
        {
            const string expectedFileName = "joystick.jpg";
            byte[] expected = testData.GetEmbeddedFile(@"yenc.multipart.joystick.jpg").ReadAllBytes();

            YencArticle part1 = YencArticleDecoder.Decode(
                testData.GetEmbeddedFile(@"yenc.multipart.00000020.ntx").ReadAllLines(UsenetEncoding.Default));
            YencArticle part2 = YencArticleDecoder.Decode(
                testData.GetEmbeddedFile(@"yenc.multipart.00000021.ntx").ReadAllLines(UsenetEncoding.Default));

            using var actual = new MemoryStream();

            actual.Seek(part1.Header.PartOffset, SeekOrigin.Begin);
            actual.Write(part1.Data.ToArray(), 0, (int)part1.Header.PartSize);

            actual.Seek(part2.Header.PartOffset, SeekOrigin.Begin);
            actual.Write(part2.Data.ToArray(), 0, (int)part2.Header.PartSize);

            string actualFileName = part1.Header.FileName;

            Assert.Equal(expectedFileName, actualFileName);
            Assert.Equal(expected, actual.ToArray());
        }
    }
}
