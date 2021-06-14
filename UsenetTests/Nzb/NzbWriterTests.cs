using System.IO;
using Microsoft.Extensions.FileProviders;
using Usenet.Nzb;
using Usenet.Util;
using UsenetTests.Extensions;
using UsenetTests.TestHelpers;
using Xunit;

namespace UsenetTests.Nzb
{
    public class NzbWriterTests : IClassFixture<TestData>
    {
        private readonly TestData testData;

        public NzbWriterTests(TestData testData)
        {
            this.testData = testData;
        }

        [Theory]
        [InlineData(@"nzb.sabnzbd.nzb")]
        [InlineData(@"nzb.sabnzbd-no-namespace.nzb")]
        public void ShouldWriteDocumentToFile(string fileName)
        {
            IFileInfo file = testData.GetEmbeddedFile(fileName);
            NzbDocument expected = NzbParser.Parse(file.ReadAllText(UsenetEncoding.Default));

            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, UsenetEncoding.Default);
            using var reader = new StreamReader(stream, UsenetEncoding.Default);

            // write to file and read back for comparison
            writer.WriteNzbDocument(expected);
            stream.Position = 0;
            NzbDocument actual = NzbParser.Parse(reader.ReadToEnd());

            // compare
            Assert.Equal(expected, actual);
        }
    }
}
