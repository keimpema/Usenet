using System.IO;
using Usenet.Extensions;
using Usenet.Util;
using Usenet.Yenc;
using UsenetTests.Extensions;
using Xunit;

namespace UsenetTests.Yenc
{
    public class YencStreamDecoderTests : IClassFixture<TestData>
    {
        private readonly TestData testData;

        public YencStreamDecoderTests(TestData testData)
        {
            this.testData = testData.Initialize(typeof(YencStreamDecoderTests));
        }

        [Fact]
        public void SinglePartFileShouldBeDecoded()
        {
            byte[] expectedData = testData.GetEmbeddedFile(@"yenc.singlepart.testfile.txt").ReadAllBytes();

            YencStream actualStream = YencStreamDecoder.Decode(
                testData.GetEmbeddedFile(@"yenc.singlepart.00000005.ntx").ReadAllLines(UsenetEncoding.Default));

            byte[] actualData = actualStream.ReadAllBytes();

            Assert.False(actualStream.Header.IsFilePart);
            Assert.Equal(128, actualStream.Header.LineLength);
            Assert.Equal(584, actualStream.Header.FileSize);
            Assert.Equal("testfile.txt", actualStream.Header.FileName);
            Assert.Equal(584, actualData.Length);
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public void FilePartShouldBeDecoded()
        {
            const int expectedDataLength = 11250;
            YencStream actualStream = YencStreamDecoder.Decode(
                testData.GetEmbeddedFile(@"yenc.multipart.00000020.ntx").ReadAllLines(UsenetEncoding.Default));
            byte[] actualData = actualStream.ReadAllBytes();

            Assert.True(actualStream.Header.IsFilePart);
            Assert.Equal(expectedDataLength, actualData.Length);
        }

        [Fact]
        public void MultiPartFileShouldBeDecoded()
        {
            const string expectedFileName = "joystick.jpg";
            byte[] expected = testData.GetEmbeddedFile(@"yenc.multipart.joystick.jpg").ReadAllBytes();

            YencStream part1 = YencStreamDecoder.Decode(
                testData.GetEmbeddedFile(@"yenc.multipart.00000020.ntx").ReadAllLines(UsenetEncoding.Default));
            YencStream part2 = YencStreamDecoder.Decode(
                testData.GetEmbeddedFile(@"yenc.multipart.00000021.ntx").ReadAllLines(UsenetEncoding.Default));

            using (var actual = new MemoryStream())
            {
                actual.Seek(part1.Header.PartOffset, SeekOrigin.Begin);
                part1.CopyTo(actual);

                actual.Seek(part2.Header.PartOffset, SeekOrigin.Begin);
                part2.CopyTo(actual);

                string actualFileName = part1.Header.FileName;

                Assert.Equal(expectedFileName, actualFileName);
                Assert.Equal(expected, actual.ToArray());
            }
        }

    }
}
