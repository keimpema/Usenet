namespace Usenet.Yenc
{
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
        /// (1.2) In case of multiple parts this contains the total number of parts; otherwise 1.
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
        /// Ctor.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileSize"></param>
        /// <param name="lineLength"></param>
        /// <param name="partNumber"></param>
        /// <param name="totalParts"></param>
        /// <param name="partSize"></param>
        /// <param name="partOffset"></param>
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

        public bool IsFilePart => PartNumber > 0;
    }
}
