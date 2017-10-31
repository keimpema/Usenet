using System.Linq;

namespace Usenet.Util
{
    internal static class EmptyLookup<TKey, TElement>
    {
        public static ILookup<TKey, TElement> Instance => Enumerable.Empty<TElement>().ToLookup(x => default(TKey));
    }
}
