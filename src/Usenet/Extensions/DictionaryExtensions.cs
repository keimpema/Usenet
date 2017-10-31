using System;
using System.Collections.Generic;
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
        /// <returns></returns>
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
        /// <returns></returns>
        public static TValue GetOrDefault<TValue>(this IDictionary<string, TValue> dictionary, string key)
        {
            Guard.ThrowIfNull(dictionary, nameof(dictionary));
            Guard.ThrowIfNullOrEmpty(key, nameof(key));

            return dictionary.TryGetValue(key, out TValue value) ? value : default(TValue);
        }

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
    }
}
