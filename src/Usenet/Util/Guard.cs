using System;

namespace Usenet.Util
{
    internal static class Guard
    {
        /// <summary>
        /// Throws an <exception cref="ArgumentNullException">ArgumentNullException</exception> if obj is null.
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <param name="name">The name of the object</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
        public static void ThrowIfNull(object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name, Resources.Util.NullValueNotAllowed);
            }
        }

        /// <summary>
        /// Throws an <exception cref="ArgumentNullException">ArgumentNullException</exception> if the specified string is null.
        /// Throws an <exception cref="ArgumentException">ArgumentException</exception> if the specified string is empty.
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <param name="name">The name of the string</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
        /// <exception cref="ArgumentException">ArgumentException</exception>
        public static void ThrowIfNullOrEmpty(string str, string name)
        {
            ThrowIfNull(str, name);
            if (str.Length == 0)
            {
                throw new ArgumentException(Resources.Util.EmptyStringNotAllowed, name);
            }
        }

        /// <summary>
        /// Throws an <exception cref="ArgumentNullException">ArgumentNullException</exception> if the specified string is null.
        /// Throws an <exception cref="ArgumentException">ArgumentException</exception> if the specified string is empty or if it consists only of white-space characters.
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <param name="name">The name of the string</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
        /// <exception cref="ArgumentException">ArgumentException</exception>
        public static void ThrowIfNullOrWhiteSpace(string str, string name)
        {
            ThrowIfNullOrEmpty(str, name);
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException(Resources.Util.OnlyWhiteSpaceCharactersNotAllowed, name);
            }
        }
    }
}
