namespace Usenet.Yenc
{
    /// <summary>
    /// Represents the Yenc footer (=yend) line.
    /// </summary>
    public class YencFooter
    {
        /// <summary>
        /// In case of multiple parts this returns the part size; otherwise it returns the file size.
        /// </summary>
        public long PartSize { get; }

        /// <summary>
        /// In case of multiple parts this contains the part number; otherwise 0.
        /// </summary>
        public int PartNumber { get; }

        /// <summary>
        /// A 32-bit Cyclic Redundancy Check (CRC) value,
        /// to assist in verifying the integrity of the encoded binary data.
        /// </summary>
        public uint? Crc32 { get; }

        /// <summary>
        /// A 32-bit Cyclic Redundancy Check (CRC) value,
        /// to assist in verifying the integrity of the preceeding encoded part.
        /// </summary>
        public uint? PartCrc32 { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="YencFooter"/> class.
        /// </summary>
        /// <param name="partSize">Size of the part or single-part file.</param>
        /// <param name="partNumber">Number of the part or 0 in case of a single-part file.</param>
        /// <param name="crc32">The 32-bit CRC value of the complete file.</param>
        /// <param name="partCrc32">The 32-bit CRC value of the part.</param>
        public YencFooter(long partSize, int partNumber, uint? crc32, uint? partCrc32)
        {
            PartSize = partSize;
            PartNumber = partNumber;
            Crc32 = crc32;
            PartCrc32 = partCrc32;
        }
    }
}
