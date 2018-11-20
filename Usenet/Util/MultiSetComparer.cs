using System.Collections.Generic;

namespace Usenet.Util
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a multi set comparer.
    /// (<a href="https://stackoverflow.com/questions/50098/comparing-two-collections-for-equality-irrespective-of-the-order-of-items-in-the">Source</a>)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class MultiSetComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        private readonly IEqualityComparer<T> comparer;

        /// <summary>
        /// Creates a new instance of the <see cref="MultiSetComparer{T}"/> class
        /// using the specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="comparer">The equality comparer to use.</param>
        public MultiSetComparer(IEqualityComparer<T> comparer)
        {
            this.comparer = comparer ?? EqualityComparer<T>.Default;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MultiSetComparer{T}"/> class
        /// using the default <see cref="IEqualityComparer{T}"/> for the type specified by the generic argument.
        /// </summary>
        public MultiSetComparer() : this(EqualityComparer<T>.Default)
        {            
        }

        /// <summary>
        /// Determines whether the first collection is equal to the second collection irrespective
        /// of the order of items in the collections.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public int GetHashCode(IEnumerable<T> enumerable) => HashCode.Start.Hash(enumerable);

        /// <summary>
        /// A singleton instance of the <see cref="MultiSetComparer{T}"/> class that
        /// uses the default <see cref="IEqualityComparer{T}"/> for the type specified by the generic argument.
        /// </summary>
        public static MultiSetComparer<T> Instance { get; } = new MultiSetComparer<T>();
    }
}
