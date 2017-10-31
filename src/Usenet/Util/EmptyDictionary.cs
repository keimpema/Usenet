using System.Collections.Generic;

namespace Usenet.Util
{
    internal static class EmptyDictionary<TKey, TElement>
    {
        public static Dictionary<TKey, TElement> Instance => new Dictionary<TKey, TElement>(0);
    }
}
