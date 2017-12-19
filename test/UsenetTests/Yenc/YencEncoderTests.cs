using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lib;
using Lib.Extensions;
using Usenet.Util;
using Usenet.Yenc;
using Xunit;

namespace UsenetTests.Yenc
{
    public class YencEncoderTests : IClassFixture<TestData>
    {
        private readonly TestData testData;

        public YencEncoderTests(TestData testData)
        {
            this.testData = testData.Initialize(typeof(YencEncoderTests));
        }

        [Fact]
        public void ShouldBeEncodedAsSinglePartFile()
        {
            List<string> expectedText = testData
                .GetEmbeddedFile(@"yenc.singlepart.test (1.2).ntx")
                .ReadAllLines(UsenetEncoding.Default)
                .Skip(3)
                .Take(9)
                .ToList();

            byte[] data = testData.GetEmbeddedFile(@"yenc.singlepart.test (1.2).dat").ReadAllBytes();
            using (var stream = new MemoryStream(data))
            {
                var header = new YencHeader("test (1.2).txt", data.Length, 10, 0, 1, data.Length, 0);
                List<string> actualText = YencEncoder.Encode(header, stream).ToList();

                Assert.Equal(expectedText, actualText);
            }
        }

        [Fact]
        public void ShouldBeEncodedAsPartOfMultiPartFile()
        {
            List<string> expectedText = testData
                .GetEmbeddedFile(@"yenc.multipart.test (1.2).ntx")
                .ReadAllLines(UsenetEncoding.Default)
                .Skip(3)
                .Take(10)
                .ToList();

            byte[] data = testData.GetEmbeddedFile(@"yenc.multipart.test (1.2).dat").ReadAllBytes();
            using (var stream = new MemoryStream(data))
            {
                var header = new YencHeader("test (1.2).txt", 120, 10, 1, 2, data.Length, 0);
                List<string> actualText = YencEncoder.Encode(header, stream).ToList();

                Assert.Equal(expectedText, actualText);
            }
        }

    }
}
