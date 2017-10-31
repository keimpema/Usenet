namespace Usenet.Yenc
{
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

        public uint? Crc32 { get; }

        public uint? PartCrc32 { get; }

        public YencFooter(long partSize, int partNumber, uint? crc32, uint? partCrc32)
        {
            PartSize = partSize;
            PartNumber = partNumber;
            Crc32 = crc32;
            PartCrc32 = partCrc32;
        }
    }
}
