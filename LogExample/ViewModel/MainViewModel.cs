using System;
using System.IO;
using System.Windows.Input;
using LogExample.Ex;
using LogExample.View;

namespace LogExample.ViewModel
{
    public class MainViewModel : IDisposable
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(MainViewModel));

        public MainViewModel()
        {
            OpenLogWindow = new RelayCommand(() => ((Func<LogWindow>)(() => new LogWindow())).OpenOnNewThread());
            OpenSubWindow = new RelayCommand(() => ((Func<SubWindow>)(() => new SubWindow())).OpenOnNewThread());
            TriggerGC = new RelayCommand(GC.Collect);
            CreateLog = new RelayCommand(() => _log.Info(Path.GetRandomFileName()));
        }

        public ICommand OpenLogWindow { get; }
        public ICommand OpenSubWindow { get; }
        public ICommand TriggerGC { get; }
        public ICommand CreateLog { get; }

        public void Dispose()
        {
            _log.Dispose();
        }
    }
}