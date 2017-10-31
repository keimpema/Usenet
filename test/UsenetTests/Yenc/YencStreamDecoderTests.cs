using System.IO;
using Usenet.Extensions;
using Usenet.Util;
using Usenet.Yenc;
using Xunit;

namespace UsenetTests.Yenc
{
    public class YencStreamDecoderTests : IClassFixture<TestData>
    {
        private readonly TestData testData;

        public YencStreamDecoderTests(TestData testData)
        {
            this.testData = testData;
        }

        [Fact]
        public void SinglePartFileShouldBeDecoded()
        {
            byte[] expectedData = File.ReadAllBytes(testData.GetFullPath(@"yenc\singlepart\testfile.txt"));

            YencStream actualStream = YencStreamDecoder.Decode(
                File.ReadAllLines(testData.GetFullPath(@"yenc\singlepart\00000005.ntx"), UsenetEncoding.Default));
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
                File.ReadAllLines(testData.GetFullPath(@"yenc\multipart\00000020.ntx"), UsenetEncoding.Default));
            byte[] actualData = actualStream.ReadAllBytes();

            Assert.True(actualStream.Header.IsFilePart);
            Assert.Equal(expectedDataLength, actualData.Length);
        }

        [Fact]
        public void MultiPartFileShouldBeDecoded()
        {
            const string expectedFileName = "joystick.jpg";
            byte[] expected = File.ReadAllBytes(testData.GetFullPath(@"yenc\multipart\joystick.jpg"));

            YencStream part1 = YencStreamDecoder.Decode(
                File.ReadAllLines(testData.GetFullPath(@"yenc\multipart\00000020.ntx"), UsenetEncoding.Default));
            YencStream part2 = YencStreamDecoder.Decode(
                File.ReadAllLines(testData.GetFullPath(@"yenc\multipart\00000021.ntx"), UsenetEncoding.Default));

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
