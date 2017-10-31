using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace TestLib.Extensions
{
    public static class FileInfoExtensions
    {
        public static string ReadAllText(this IFileInfo fileInfo, Encoding encoding)
        {
            using (Stream stream = fileInfo.CreateReadStream())
            using (var reader = new StreamReader(stream, encoding))
            {
                return reader.ReadToEnd();
            }
        }

        public static List<string> ReadAllLines(this IFileInfo fileInfo, Encoding encoding)
        {
            using (Stream stream = fileInfo.CreateReadStream())
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                var lines = new List<string>();
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
                return lines;
            }
        }

        public static byte[] ReadAllBytes(this IFileInfo fileInfo)
        {
            using (Stream stream = fileInfo.CreateReadStream())
            {
                if (stream.CanSeek)
                {
                    stream.Seek(0L, SeekOrigin.Begin);
                }
                if (stream is MemoryStream memoryStream)
                {
                    return memoryStream.ToArray();
                }
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

    }
}
