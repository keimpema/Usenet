using System;
using Usenet.Util;

namespace Usenet.Extensions
{
    /// <summary>
    /// String extension methods.
    /// </summary>
    internal static class StringExtensions
    {
        public static string ThrowIfNullOrEmpty(this string str, string name)
        {
            Guard.ThrowIfNullOrEmpty(str, name);
            return str;
        }
        
        public static string ThrowIfNullOrWhiteSpace(this string str, string name)
        {
            Guard.ThrowIfNullOrWhiteSpace(str, name);
            return str;
        }

        public static int? ToIntSafe(this string str)
        {
            if (str == null)
            {
                return null;
            }
            int.TryParse(str, out int value);
            return value;
        }

        public static int ToIntSafe(this string str, int defaultValue)
        {
            if (str == null)
            {
                return defaultValue;
            }
            return int.TryParse(str, out int value) ? value : defaultValue;
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
