using System;
using Usenet.Util;

namespace Usenet.Extensions
{
    /// <summary>
    /// Object extension methods.
    /// </summary>
    internal static class ObjectExtensions
    {
        /// <summary>
        /// Returns the object or throws an <exception cref="ArgumentNullException"/> if obj is null.
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="obj">The object to check</param>
        /// <param name="name">The name of the object</param>
        /// <returns>The object</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T ThrowIfNull<T>(this T obj, string name)
        {
            Guard.ThrowIfNull(obj, name);
            return obj;
        }
    }
}
