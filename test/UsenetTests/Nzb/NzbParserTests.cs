using System;
using System.Linq;
using System.Xml;
using Lib;
using Lib.Extensions;
using Usenet.Exceptions;
using Usenet.Nzb;
using Usenet.Util;
using Xunit;

namespace UsenetTests.Nzb
{
    public class NzbParserTests : IClassFixture<TestData>
    {
        private readonly TestData testData;

        public NzbParserTests(TestData testData)
        {
            this.testData = testData.Initialize(typeof(NzbParserTests));
        }

        [Theory]
        [InlineData(@"nzb.sabnzbd.nzb")]
        [InlineData(@"nzb.sabnzbd-no-namespace.nzb")]
        public void ValidNzbDataShouldBeParsed(string fileName)
        {
            string nzbData = testData.GetEmbeddedFile(fileName).ReadAllText(UsenetEncoding.Default);
            NzbDocument actualDocument = NzbParser.Parse(nzbData);

            Assert.Equal("Your File!", actualDocument.MetaData["title"].Single());
            Assert.Equal("secret", actualDocument.MetaData["password"].Single());
            Assert.Equal("HD", actualDocument.MetaData["tag"].Single());
            Assert.Equal("TV", actualDocument.MetaData["category"].Single());
            Assert.Equal(106895, actualDocument.Size);
        }

        [Fact]
        public void MinimalNzbDataShouldBeParsed()
        {
            const string nzbText = @"<nzb xmlns=""http://www.newzbin.com/DTD/2003/nzb""></nzb>";
            NzbDocument actualDocument = NzbParser.Parse(nzbText);

            Assert.Empty(actualDocument.MetaData);
            Assert.Empty(actualDocument.Files);
        }

        [Fact]
        public void MultipleMetaDataKeysShouldBeParsed()
        {
            const string nzbText = @"
<nzb xmlns=""http://www.newzbin.com/DTD/2003/nzb"">
  <head>
    <meta type=""tag"">SD</meta>
    <meta type=""tag"">avi</meta>
  </head>
</nzb>";
            NzbDocument actualDocument = NzbParser.Parse(nzbText);

            Assert.Single(actualDocument.MetaData);
            Assert.Equal(2, actualDocument.MetaData["tag"].Count);
            Assert.NotNull(actualDocument.MetaData["tag"].SingleOrDefault(m => m == "SD"));
            Assert.NotNull(actualDocument.MetaData["tag"].SingleOrDefault(m => m == "avi"));
        }

        [Fact]
        public void MinimalFileShouldBeParsed()
        {
            const string nzbText = @"
<nzb xmlns=""http://www.newzbin.com/DTD/2003/nzb"">
  <file></file>
</nzb>";

            NzbDocument actualDocument = NzbParser.Parse(nzbText);

            Assert.Empty(actualDocument.MetaData);
            Assert.Single(actualDocument.Files);
        }

        [Fact]
        public void FileDateShouldBeParsed()
        {
            DateTimeOffset expected = DateTimeOffset.Parse(@"2017-06-01T06:49:13+00:00");
            const string nzbText = @"
<nzb xmlns=""http://www.newzbin.com/DTD/2003/nzb"">
  <file date=""1496299753""></file>
</nzb>";

            NzbDocument actualDocument = NzbParser.Parse(nzbText);
            Assert.Equal(expected, actualDocument.Files[0].Date);
        }

        [Fact]
        public void InvalidFileDateShouldThrow()
        {
            const string nzbText = @"
<nzb xmlns=""http://www.newzbin.com/DTD/2003/nzb"">
  <file date=""1496xxx753""></file>
</nzb>";

            Assert.Throws<InvalidNzbDataException>(() => NzbParser.Parse(nzbText));
        }

        [Fact]
        public void InvalidSegmentNumberShouldThrow()
        {
            const string nzbText = @"
<nzb xmlns=""http://www.newzbin.com/DTD/2003/nzb"">
  <file>
    <segments>
      <segment number=""123ffg45""></segment>
    </segments>
  </file>
</nzb>";

            Assert.Throws<InvalidNzbDataException>(() => NzbParser.Parse(nzbText));
        }
        [Fact]
        public void MissingSegmentNumberShouldThrow()
        {
            const string nzbText = @"
<nzb xmlns=""http://www.newzbin.com/DTD/2003/nzb"">
  <file>
    <segments>
      <segment bytes=""1000""></segment>
    </segments>
  </file>
</nzb>";

            Assert.Throws<InvalidNzbDataException>(() => NzbParser.Parse(nzbText));
        }


        [Fact]
        public void InvalidSegmentSizeShouldThrow()
        {
            const string nzbText = @"
<nzb xmlns=""http://www.newzbin.com/DTD/2003/nzb"">
  <file>
    <segments>
      <segment bytes=""123ffg45""></segment>
    </segments>
  </file>
</nzb>";

            Assert.Throws<InvalidNzbDataException>(() => NzbParser.Parse(nzbText));
        }

        [Fact]
        public void MissingSegmentSizeShouldThrow()
        {
            const string nzbText = @"
<nzb xmlns=""http://www.newzbin.com/DTD/2003/nzb"">
  <file>
    <segments>
      <segment number=""1""></segment>
    </segments>
  </file>
</nzb>";

            Assert.Throws<InvalidNzbDataException>(() => NzbParser.Parse(nzbText));
        }

        [Fact]
        public void InvalidXmlShouldThrow()
        {
            const string nzbText = @"sdfsfasfasdfasdf";
            Assert.Throws<XmlException>(() => NzbParser.Parse(nzbText));
        }

        [Fact]
        public void InvalidNzbShouldThrow()
        {
            const string nzbText = @"<html></html>";
            Assert.Throws<InvalidNzbDataException>(() => NzbParser.Parse(nzbText));
        }

        [Fact]
        public void FileShouldBeExtractedFromSubjectWhenQuoted()
        {
            const string nzbText = @"
<nzb xmlns=""http://www.newzbin.com/DTD/2003/nzb"">
  <file subject=""(TWD151 - 153)[2 / 9] - &quot;TWD151 - 153.rar&quot; yEnc (001 / 249)""></file>
</nzb>";

            NzbDocument actualDocument = NzbParser.Parse(nzbText);
            Assert.Equal("TWD151 - 153.rar", actualDocument.Files.Single().FileName);
        }

        [Fact]
        public void FileShouldBeExtractedFromSubjectWhenNotQuoted()
        {
            const string nzbText = @"
<nzb xmlns=""http://www.newzbin.com/DTD/2003/nzb"">
  <file subject=""(TWD151 - 153)[2 / 9] - TWD151 - 153.rar yEnc (001 / 249)""></file>
</nzb>";

            NzbDocument actualDocument = NzbParser.Parse(nzbText);
            Assert.Equal("(TWD151 - 153)[2 / 9] - TWD151 - 153.rar", actualDocument.Files.Single().FileName);
        }

        [Fact]
        public void FileShouldBeExtractedFromSubjectWhenNotQuotedAndNoParenthesis()
        {
            const string nzbText = @"
<nzb xmlns=""http://www.newzbin.com/DTD/2003/nzb"">
  <file subject=""[2 / 9] - TWD151 - 153.rar yEnc""></file>
</nzb>";

            NzbDocument actualDocument = NzbParser.Parse(nzbText);
            Assert.Equal("[2 / 9] - TWD151 - 153.rar", actualDocument.Files.Single().FileName);
        }
    }
}
