using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Reflection;
using UsenetTests.TestHelpers;
using Xunit;

namespace UsenetTests
{
    public class LoggerTests
    {
        [Fact]
        public void ShouldUseNullLogger()
        {
            Usenet.Logger.Factory = null;
            var logger = Usenet.Logger.Create<LoggerTests>();
            var actualLogger = GetActualLogger(logger);

            Assert.IsType<NullLogger>(actualLogger);
        }

        [Fact]
        public void ShouldUseSetLogger()
        {
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock
                .Setup(m => m.CreateLogger(It.IsAny<string>()))
                .Returns(new InMemoryLogger());

            Usenet.Logger.Factory = loggerFactoryMock.Object;
            var logger = Usenet.Logger.Create<LoggerTests>();

            Assert.IsType<InMemoryLogger>(logger);
        }

        private static object? GetActualLogger(ILogger logger)
        {
            var loggerType = logger.GetType();
            var field = loggerType.GetField("_logger", BindingFlags.Instance | BindingFlags.NonPublic);
            return field?.GetValue(logger);
        }
    }
}
