using System;

namespace Usenet.Nntp.Formatters
{
    internal static class DateTimeFormatter
    {
        public static string Format(DateTimeOffset dateTime)
        {
            return $"{dateTime.ToUniversalTime():yyyyMMdd HHmmss} GMT";
        }
    }
}
