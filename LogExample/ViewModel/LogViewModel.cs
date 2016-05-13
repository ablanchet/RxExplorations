using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace LogExample.ViewModel
{
    public class LogViewModel
    {
        public LogViewModel()
        {
            LogService.Instance.Logs
                                .ObserveOnDispatcher()
                                .Subscribe(log => Logs.Add(FormatLog(log)));
        }

        private string FormatLog(LogEntry log)
        {
            return $"{log.Timestamp.ToLongTimeString()} - {log.LoggerName} - {log.LogLevel} - {log.Message}";
        }

        public ObservableCollection<string> Logs { get; } = new ObservableCollection<string>();
    }

}