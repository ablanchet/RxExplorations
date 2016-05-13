using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Windows.Input;
using LogExample.Ex;

namespace LogExample.ViewModel
{
    public class SubWindowViewModel : IDisposable
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(SubWindowViewModel));

        public SubWindowViewModel()
        {
            LogService.Instance.Logs
                                .Where(l => l.LoggerName == typeof(SubWindowViewModel).Name)
                                .ObserveOnDispatcher()
                                .Subscribe(l => MyLogs.Add(FormatLog(l)));

            CreateLog = new RelayCommand(() => _log.Info(Path.GetRandomFileName()));
        }

        private string FormatLog(LogEntry log)
        {
            return $"{log.Timestamp.ToLongTimeString()} - {log.LogLevel} - {log.Message}";
        }

        public ICommand CreateLog { get; }
        public ObservableCollection<string> MyLogs { get; } = new ObservableCollection<string>();

        public void Dispose()
        {
            _log.Dispose();
        }
    }
}