using System;
using System.Reactive.Subjects;

namespace LogExample
{
    public class LogService
    {
        private static LogService _instance;
        private readonly ReplaySubject<LogEntry> _logs;

        private LogService()
        {
            _logs = new ReplaySubject<LogEntry>();
        }

        public static LogService Instance => _instance ?? (_instance = new LogService());

        public IObservable<LogEntry> Logs => _logs;

        internal IDisposable Register(IObservable<LogEntry> logger)
        {
            return logger.Subscribe(l => _logs.OnNext(l));
        }
    }

    public static class LogManager
    {
        public static ILogger GetLogger(Type callerType)
        {
            return new Logger(callerType.Name);
        }
    }

    public interface ILogger : IDisposable
    {
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Critical(string message);
    }

    internal class Logger : ILogger
    {
        private readonly Subject<LogEntry> _logs = new Subject<LogEntry>();
        private readonly IDisposable _disposable;

        public Logger(string typeName)
        {
            TypeName = typeName;
            _disposable = LogService.Instance.Register(_logs);
        }

        public string TypeName { get; }

        public void Debug(string message)
        {
            _logs.OnNext(new LogEntry(this, LogLevel.Debug, message));
        }

        public void Info(string message)
        {
            _logs.OnNext(new LogEntry(this, LogLevel.Info, message));
        }

        public void Warn(string message)
        {
            _logs.OnNext(new LogEntry(this, LogLevel.Warn, message));
        }

        public void Error(string message)
        {
            _logs.OnNext(new LogEntry(this, LogLevel.Error, message));
        }

        public void Critical(string message)
        {
            _logs.OnNext(new LogEntry(this, LogLevel.Critical, message));
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }

    public class LogEntry
    {
        internal LogEntry(Logger logger, LogLevel logLevel, string message)
        {
            Timestamp = DateTime.UtcNow;
            LoggerName = logger.TypeName;
            LogLevel = logLevel;
            Message = message;
        }

        public DateTime Timestamp { get; }

        public string LoggerName { get; }

        public LogLevel LogLevel { get; }

        public string Message { get; }
    }

    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Critical
    }
}