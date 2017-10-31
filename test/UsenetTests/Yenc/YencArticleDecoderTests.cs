using System.IO;
using System.Linq;
using Usenet.Util;
using Usenet.Yenc;
using Xunit;

namespace UsenetTests.Yenc
{
    public class YencDecoderTests : IClassFixture<TestData>
    {
        private readonly TestData testData;

        public YencDecoderTests(TestData testData)
        {
            this.testData = testData;
        }

        [Fact]
        public void SinglePartFileShouldBeDecoded()
        {
            byte[] expectedData = File.ReadAllBytes(testData.GetFullPath(@"yenc\singlepart\testfile.txt"));

            var actualArticle = YencArticleDecoder.Decode(
                File.ReadAllLines(testData.GetFullPath(@"yenc\singlepart\00000005.ntx"), UsenetEncoding.Default));

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
            YencArticle actual = YencArticleDecoder.Decode(
                File.ReadAllLines(testData.GetFullPath(@"yenc\multipart\00000020.ntx"), UsenetEncoding.Default));
            Assert.True(actual.Header.IsFilePart);
            Assert.Equal(expectedDataLength, actual.Data.Length);
        }

        [Fact]
        public void MultiPartFileShouldBeDecoded()
        {
            const string expectedFileName = "joystick.jpg";
            byte[] expected = File.ReadAllBytes(testData.GetFullPath(@"yenc\multipart\joystick.jpg"));

            YencArticle part1 = YencArticleDecoder.Decode(
                File.ReadAllLines(testData.GetFullPath(@"yenc\multipart\00000020.ntx"), UsenetEncoding.Default));
            YencArticle part2 = YencArticleDecoder.Decode(
                File.ReadAllLines(testData.GetFullPath(@"yenc\multipart\00000021.ntx"), UsenetEncoding.Default));

            using (var actual = new MemoryStream())
            {
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
}
