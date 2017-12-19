using System.Collections.Generic;
using System.Linq;

namespace Usenet.Util
{
    /// <summary>
    /// Represents a helper class for creating hash codes.
    /// Source: https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
    /// </summary>
    public static class HashCode
    {
        /// <summary>
        /// Starting value.
        /// </summary>
        public const int Start = 17;

        /// <summary>
        /// Combine current hash code with the hash code of the specified object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="hash">The current hash code.</param>
        /// <param name="obj">The object to calculate the hash code for.</param>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public static int Hash<T>(this int hash, T obj) => 
            unchecked(hash * 23 + EqualityComparer<T>.Default.GetHashCode(obj));

        /// <summary>
        /// Combine the current hash code with the combined hash codes of all the elements in the collection.
        /// Collections with the same elements in different order will yield differtent hash codes.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="hash">The current hash code.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public static int Hash<T>(this int hash, IEnumerable<T> collection) => collection?.Aggregate(hash, Hash) ?? hash;

        /// <summary>
        /// Combine the current hash code with the combined hash codes of all the keys and values in the
        /// dictioary. Dictionaries with the same keys and values may yield different hash codes because
        /// dictionaries are unordered.
        /// </summary>
        /// <typeparam name="TKey">The type of the dictionary keys.</typeparam>
        /// <typeparam name="TValue">The type fo the dictionary values.</typeparam>
        /// <param name="hash">The current hash code.</param>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        public static int Hash<TKey, TValue>(this int hash, IDictionary<TKey, TValue> dictionary) => 
            dictionary.Aggregate(hash, (current, pair) => current.Hash(pair.Key).Hash(pair.Value));
    }
}
