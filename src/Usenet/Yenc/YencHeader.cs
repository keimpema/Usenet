namespace Usenet.Yenc
{
    /// <summary>
    /// Represents the combined information of the yEnc header (=ybegin) line 
    /// and the yEnc part header (=ypart) line if present.
    /// </summary>
    public class YencHeader
    {
        /// <summary>
        /// Name of the file.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Size of the file in bytes.
        /// </summary>
        public long FileSize { get; }

        /// <summary>
        /// Length of the encoded lines.
        /// </summary>
        public int LineLength { get; }

        /// <summary>
        /// In case of multiple parts this contains the part number; otherwise 0.
        /// </summary>
        public int PartNumber { get; }

        /// <summary>
        /// In case of multiple parts this contains the total number of parts; otherwise 1.
        /// </summary>
        public int TotalParts { get; }

        /// <summary>
        /// In case of multiple parts this returns the part size; otherwise it returns the file size.
        /// </summary>
        public long PartSize { get; }

        /// <summary>
        /// In case of multiple parts this returns the part offset; otherwise it returns 0.
        /// </summary>
        public long PartOffset { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="YencHeader"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileSize">Size of the file in bytes.</param>
        /// <param name="lineLength">Length of the encoded lines.</param>
        /// <param name="partNumber">Number of the part or 0 in case of a single-part file.</param>
        /// <param name="totalParts">Total number of parts or 1 in case of a single-part file.</param>
        /// <param name="partSize">Size of the part or entire file.</param>
        /// <param name="partOffset">Offset of the part or 0 in case of a single-part file.</param>
        public YencHeader(string fileName, long fileSize, int lineLength, int partNumber, int totalParts, long partSize, long partOffset)
        {
            FileName = fileName;
            FileSize = fileSize;
            LineLength = lineLength;
            PartNumber = partNumber;
            TotalParts = totalParts;
            PartSize = partSize;
            PartOffset = partOffset;
        }

        /// <summary>
        /// In case of multiple parts this returns true; otherwise false.
        /// </summary>
        public bool IsFilePart => PartNumber > 0;
    }
}
