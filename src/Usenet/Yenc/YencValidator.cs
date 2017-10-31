using System.Collections.Generic;
using Usenet.Util;

namespace Usenet.Yenc
{
    public class YencValidator
    {
        public static ValidationResult Validate(YencArticle article)
        {
            var failures = new List<ValidationFailure>();

            YencHeader header = article.Header;
            YencFooter footer = article.Footer;

            if (footer == null)
            {
                // nothing to validate
                return new ValidationResult(failures);
            }

            if (header.PartNumber > 0)
            {
                ValidatePart(article, failures);
                return new ValidationResult(failures);
            }

            if (footer.PartSize != article.Data.Length)
            {
                failures.Add(new ValidationFailure(
                    YencValidationErrorCodes.SizeMismatch, Resources.Yenc.SizeMismatch,
                    new { DataSize = article.Data.Length, FooterSize = footer.PartSize }));
            }

            if (!footer.Crc32.HasValue)
            {
                failures.Add(new ValidationFailure(
                    YencValidationErrorCodes.MissingChecksum, Resources.Yenc.MissingChecksum));
                return new ValidationResult(failures);
            }

            uint calculatedCrc32 = Crc32.CalculateChecksum(article.Data);
            if (calculatedCrc32 != footer.Crc32.Value)
            {
                failures.Add(new ValidationFailure(
                    YencValidationErrorCodes.ChecksumMismatch, Resources.Yenc.ChecksumMismatch,
                    new { CalculatedChecksum = calculatedCrc32, FooterChecksum = footer.Crc32.Value }));
            }
            return new ValidationResult(failures);
        }

        private static void ValidatePart(YencArticle article, ICollection<ValidationFailure> failures)
        {
            YencHeader header = article.Header;
            YencFooter footer = article.Footer;

            if (header.PartNumber != footer.PartNumber)
            {
                failures.Add(new ValidationFailure(
                    YencValidationErrorCodes.PartMismatch, Resources.Yenc.PartMismatch,
                    new { HeaderPart = header.PartNumber, FooterPart = footer.PartNumber }));
            }

            if (!(footer.PartSize == article.Data.Length && footer.PartSize == header.PartSize))
            {
                failures.Add(new ValidationFailure(
                    YencValidationErrorCodes.SizeMismatch, Resources.Yenc.PartSizeMismatch,
                    new { DataSize = article.Data.Length, HeaderSize = header.PartSize, FooterSize = footer.PartSize }));
            }

            if (!footer.PartCrc32.HasValue)
            {
                failures.Add(new ValidationFailure(
                    YencValidationErrorCodes.MissingChecksum, Resources.Yenc.MissingPartChecksum));
                return;
            }

            uint calculatedCrc32 = Crc32.CalculateChecksum(article.Data);
            if (calculatedCrc32 != footer.PartCrc32.Value)
            {
                failures.Add(new ValidationFailure(
                    YencValidationErrorCodes.ChecksumMismatch, Resources.Yenc.PartChecksumMismatch,
                    new { CalculatedChecksum = calculatedCrc32, FooterChecksum = footer.PartCrc32 }));
            }
        }
    }
}
