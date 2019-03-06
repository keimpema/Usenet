namespace Usenet.Yenc
{
    /// <summary>
    /// yEnc line decoder.
    /// Based on Kristian Hellang's yEnc project https://github.com/khellang/yEnc.
    /// </summary>
    internal class YencLineDecoder
    {
        public static int Decode(byte[] encodedBytes, byte[] decodedBytes, int decodedOffset) => 
            Decode(encodedBytes, 0, encodedBytes.Length, decodedBytes, decodedOffset);

        public static int Decode(byte[] encodedBytes, int encodedOffset, int encodedCount, byte[] decodedBytes, int decodedOffset)
        {
            int saveOffset = decodedOffset;
            var isEscaped = false;
            for (int index = encodedOffset; index < encodedCount; index++)
            {
                byte @byte = encodedBytes[index];
                if (@byte == 61 && !isEscaped)
                {
                    isEscaped = true;
                    continue;
                }
                if (isEscaped)
                {
                    isEscaped = false;
                    decodedBytes[decodedOffset++] = (byte) (@byte - 106);
                }
                else
                {
                    decodedBytes[decodedOffset++] = (byte) (@byte - 42);
                }
            }
            return decodedOffset - saveOffset;
        }
    }
}
