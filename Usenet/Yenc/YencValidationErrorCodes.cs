namespace Usenet.Yenc
{
    /// <summary>
    /// yEnc validation error codes.
    /// Based on Kristian Hellang's yEnc project https://github.com/khellang/yEnc.
    /// </summary>
    internal class YencValidationErrorCodes
    {
        public const string MissingChecksum = "MissingChecksum";
        public const string ChecksumMismatch = "ChecksumMismatch";
        public const string SizeMismatch = "SizeMismatch";
        public const string PartMismatch = "PartMismatch";
    }
}
