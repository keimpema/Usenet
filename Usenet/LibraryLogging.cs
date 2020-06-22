using Microsoft.Extensions.Logging;

namespace Usenet
{
    /// <summary>
    /// Host for a singleton <see cref="ILoggerFactory"/>
    /// (<a href="https://stackify.com/net-core-loggerfactory-use-correctly/">Source</a>).
    /// </summary>
    public static class LibraryLogging
    {
        private static ILoggerFactory factoryInstance;

        /// <summary>
        /// The singleton <see cref="ILoggerFactory"/> to use throughout the entire library.
        /// </summary>
        public static ILoggerFactory Factory
        {
            get => factoryInstance ?? (factoryInstance = new LoggerFactory());
            set => factoryInstance = value;
        }

        /// <summary>
        /// Uses the <see cref="ILoggerFactory"/> to create a logger for the given type.
        /// </summary>
        /// <typeparam name="T">The type to create the logger for.</typeparam>
        /// <returns>A logger for the given type.</returns>
        public static ILogger Create<T>() => Factory.CreateLogger<T>();
    }
}