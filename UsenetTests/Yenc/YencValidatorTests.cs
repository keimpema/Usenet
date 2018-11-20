using System.Linq;
using Usenet.Util;
using Usenet.Yenc;
using UsenetTests.Extensions;
using Xunit;

namespace UsenetTests.Yenc
{
    public class YencValidatorTests : IClassFixture<TestData>
    {
        private readonly TestData testData;

        public YencValidatorTests(TestData testData)
        {
            this.testData = testData.Initialize(typeof(YencValidatorTests));
        }

        [Theory]
        [InlineData(@"yenc.singlepart.00000005.ntx")]
        [InlineData(@"yenc.multipart.00000020.ntx")]
        [InlineData(@"yenc.multipart.00000021.ntx")]
        [InlineData(@"yenc.singlepart.test (1.2).ntx")]
        [InlineData(@"yenc.multipart.test (1.2).ntx")]
        public void ArticleShouldBeValid(string fileName)
        {
            YencArticle article = YencArticleDecoder.Decode(
                testData.GetEmbeddedFile(fileName).ReadAllLines(UsenetEncoding.Default));


            bool actual = YencValidator.Validate(article).IsValid;
            Assert.True(actual);
        }

        [Theory]
        [InlineData(@"yenc.singlepart.00000005 (checksum mismatch).ntx", YencValidationErrorCodes.ChecksumMismatch)]
        [InlineData(@"yenc.singlepart.00000005 (missing checksum).ntx", YencValidationErrorCodes.MissingChecksum)]
        [InlineData(@"yenc.singlepart.00000005 (size mismatch).ntx", YencValidationErrorCodes.SizeMismatch)]
        [InlineData(@"yenc.multipart.00000021 (checksum mismatch).ntx", YencValidationErrorCodes.ChecksumMismatch)]
        [InlineData(@"yenc.multipart.00000021 (missing checksum).ntx", YencValidationErrorCodes.MissingChecksum)]
        [InlineData(@"yenc.multipart.00000021 (part mismatch).ntx", YencValidationErrorCodes.PartMismatch)]
        [InlineData(@"yenc.multipart.00000021 (size mismatch).ntx", YencValidationErrorCodes.SizeMismatch)]
        public void ArticleShouldBeInvalid(string fileName, string errorCode)
        {
            YencArticle article = YencArticleDecoder.Decode(
                testData.GetEmbeddedFile(fileName).ReadAllLines(UsenetEncoding.Default));

            ValidationResult result = YencValidator.Validate(article);
            Assert.False(result.IsValid);
            Assert.Equal(errorCode, result.Failures.Single().Code);
        }
    }
}
