using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace TestClient
{
    /// <summary>
    /// Host for a singleton <see cref="ILoggerFactory"/>
    /// (<a href="https://stackify.com/net-core-loggerfactory-use-correctly/">Source</a>).
    /// </summary>
    public static class ApplicationLogging
    {
        private static ILoggerFactory factoryInstance;

        /// <summary>
        /// Configure the <see cref="ILoggerFactory"/>.
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to configure.</param>
        public static void ConfigureLogger(ILoggerFactory factory)
        {
            // init serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.RollingFile("log-{Date}.txt")
                .CreateLogger();

            factory.AddSerilog();
        }

        /// <summary>
        /// The singleton <see cref="ILoggerFactory"/> to use throughout the entire application.
        /// </summary>
        public static ILoggerFactory Factory
        {
            get
            {
                if (factoryInstance != null)
                {
                    return factoryInstance;
                }

                factoryInstance = new LoggerFactory();
                ConfigureLogger(factoryInstance);

                return factoryInstance;
            }
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
