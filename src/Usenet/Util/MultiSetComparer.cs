using System.Collections.Generic;
using System.Linq;

namespace Usenet.Util
{
    /// <inheritdoc />
    /// <summary>
    /// MultiSetComparer
    /// Source: https://stackoverflow.com/questions/50098/comparing-two-collections-for-equality-irrespective-of-the-order-of-items-in-the
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class MultiSetComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        private readonly IEqualityComparer<T> comparer;

        public MultiSetComparer(IEqualityComparer<T> comparer = null)
        {
            this.comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public bool Equals(IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first == null)
            {
                return second == null;
            }
            if (second == null)
            {
                return false;
            }
            if (ReferenceEquals(first, second))
            {
                return true;
            }
            if (!(first is ICollection<T> firstCollection) || 
                !(second is ICollection<T> secondCollection))
            {
                return !HaveMismatchedElement(first, second);
            }
            if (firstCollection.Count != secondCollection.Count)
            {
                return false;
            }
            if (firstCollection.Count == 0)
            {
                return true;
            }

            return !HaveMismatchedElement(first, second);
        }

        private bool HaveMismatchedElement(IEnumerable<T> first, IEnumerable<T> second)
        {
            Dictionary<T, int> firstElementCounts = GetElementCounts(first, out int firstNullCount);
            Dictionary<T, int> secondElementCounts = GetElementCounts(second, out int secondNullCount);

            if (firstNullCount != secondNullCount || firstElementCounts.Count != secondElementCounts.Count)
            {
                return true;
            }
            foreach (KeyValuePair<T, int> pair in firstElementCounts)
            {
                int firstElementCount = pair.Value;
                secondElementCounts.TryGetValue(pair.Key, out int secondElementCount);
                if (firstElementCount != secondElementCount)
                {
                    return true;
                }
            }
            return false;
        }

        private Dictionary<T, int> GetElementCounts(IEnumerable<T> enumerable, out int nullCount)
        {
            nullCount = 0;
            var dictionary = new Dictionary<T, int>(comparer);
            foreach (T element in enumerable)
            {
                if (element == null)
                {
                    nullCount++;
                }
                else
                {
                    dictionary.TryGetValue(element, out int num);
                    num++;
                    dictionary[element] = num;
                }
            }
            return dictionary;
        }

        public int GetHashCode(IEnumerable<T> enumerable)
        {
            Guard.ThrowIfNull(enumerable, nameof(enumerable));
            return enumerable
                .OrderBy(x => x)
                .Aggregate(17, (current, val) => current * 23 + (val?.GetHashCode() ?? 42));
        }

        public static MultiSetComparer<T> Instance => new MultiSetComparer<T>();
    }
}
