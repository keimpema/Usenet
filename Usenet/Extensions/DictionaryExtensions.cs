using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Usenet.Util;

namespace Usenet.Extensions
{
    /// <summary>
    /// Dictionary extension methods.
    /// </summary>
    internal static class DictionaryExtensions
    {
        /// <summary>
        /// Gets a value from the string dictionary and converts it using the specified converter.
        /// </summary>
        /// <typeparam name="TValue">Type of the dictionary value.</typeparam>
        /// <param name="dictionary">The dictionary to search.</param>
        /// <param name="key">The key to find.</param>
        /// <param name="converter">The converter function to use.</param>
        /// <returns>The value if the key was found. Otherwise the default value of type <typeparamref name="TValue"/>.</returns>
        public static TValue GetAndConvert<TValue>(this IDictionary<string, string> dictionary, string key, Func<string, TValue> converter)
        {
            Guard.ThrowIfNull(dictionary, nameof(dictionary));
            Guard.ThrowIfNullOrEmpty(key, nameof(key));
            Guard.ThrowIfNull(converter, nameof(converter));

            return dictionary.TryGetValue(key, out string stringValue) ? converter(stringValue) : default(TValue);
        }

        /// <summary>
        /// Gets a value from the dictionary or a default value if the key was not found.
        /// </summary>
        /// <typeparam name="TValue">Type of the dictionary value.</typeparam>
        /// <param name="dictionary">The dictionary to search.</param>
        /// <param name="key">The key to find.</param>
        /// <returns>The value if the key was found. Otherwise the default value of type <typeparamref name="TValue"/>.</returns>
        public static TValue GetOrDefault<TValue>(this IDictionary<string, TValue> dictionary, string key)
        {
            Guard.ThrowIfNull(dictionary, nameof(dictionary));
            Guard.ThrowIfNullOrEmpty(key, nameof(key));

            return dictionary.TryGetValue(key, out TValue value) ? value : default(TValue);
        }

        /// <summary>
        /// Merges the source dictionary into the target dictionary.
        /// </summary>
        /// <typeparam name="TKey">Type of the dictionary key.</typeparam>
        /// <typeparam name="TValue">Type of the dictionary value.</typeparam>
        /// <param name="target">The target dictionary.</param>
        /// <param name="source">The source dictionary.</param>
        /// <param name="overwriteExistingKeys">A value indicating whether existing keys should be overwritten or not.</param>
        /// <returns>The target dictionary.</returns>
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> target,
            IDictionary<TKey, TValue> source, bool overwriteExistingKeys)
        {
            Guard.ThrowIfNull(target, nameof(target));
            Guard.ThrowIfNull(source, nameof(source));

            foreach (KeyValuePair<TKey, TValue> item in source)
            {
                if (!overwriteExistingKeys && target.ContainsKey(item.Key))
                {
                    continue;
                }
                target[item.Key] = item.Value;
            }
            return target;
        }

        /// <summary>
        /// Produces an immutable multi-value dictionary with immutable hashset collections containing the values.
        /// </summary>
        /// <returns>An immutable multi-value dictionary with immutable hashset collections containing the values.</returns>
        public static ImmutableDictionary<TKey, ImmutableHashSet<TValue>> ToImmutableDictionaryWithHashSets<TKey, TValue>(
            this IDictionary<TKey, ICollection<TValue>> multiValueDictionary) =>
            multiValueDictionary.ToImmutableDictionary(x => x.Key, x => x.Value.ToImmutableHashSet());

        /// <summary>
        /// Produces an immutable multi-value dictionary with immutable lists containing the values.
        /// </summary>
        /// <returns>An immutable multi-value dictionary with immutable lists containing the values.</returns>
        public static ImmutableDictionary<TKey, ImmutableList<TValue>> ToImmutableDictionaryWithLists<TKey, TValue>(
            this IDictionary<TKey, ICollection<TValue>> multiValueDictionary) =>
            multiValueDictionary.ToImmutableDictionary(x => x.Key, x => x.Value.ToImmutableList());
    }
}
