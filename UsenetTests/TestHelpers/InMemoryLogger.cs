using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace UsenetTests.TestHelpers
{
    public class InMemoryLogger : ILogger
    {
        public class Entry
        {
            public LogLevel LogLevel;
            public EventId EventId;
            public string? Message;
        }

        public LogLevel MinLogLevel { get; set; }

        public List<Entry> Buffer { get; }

        public InMemoryLogger()
        {
            MinLogLevel = LogLevel.Trace;
            Buffer = new List<Entry>();
        }
        public IDisposable? BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= MinLogLevel;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                var str = formatter(state, exception);
                Buffer.Add(new Entry
                {
                    LogLevel = logLevel,
                    EventId = eventId,
                    Message = str
                });
            }
        }

        public void FlushTo(ILogger logger)
        {
            foreach (var entry in Buffer)
            {
                logger.Log(entry.LogLevel, entry.EventId, entry.Message);
            }
            Buffer.Clear();
        }
    }
}
