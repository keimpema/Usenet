using System.Collections.Generic;

namespace Usenet.Util
{
    internal static class EmptyList<T>
    {
        public static List<T> Instance => new List<T>(0);
    }
}
