using System.IO;
using Usenet.Extensions;

namespace Usenet.Yenc
{
    public class YencArticle
    {
        public YencHeader Header { get; }

        public YencFooter Footer { get; }
        
        public byte[] Data { get; set; }

        public YencArticle(YencHeader header, YencFooter footer, byte[] data)
        {
            Header = header.ThrowIfNull(nameof(header));
            Footer = footer;
            Data = data.ThrowIfNull(nameof(data));
        }
    }
}
