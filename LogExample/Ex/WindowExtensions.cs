using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace LogExample.Ex
{
    public static class WindowExtensions
    {
        public static void OpenOnNewThread(this Func<Window> windowFactory)
        {
            var thread = new Thread(() =>
            {
                var newDispatcher = Dispatcher.CurrentDispatcher;
                SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(newDispatcher));

                var window = windowFactory();
                window.Show();
                window.Closed += (o, args) => Dispatcher.ExitAllFrames();

                Dispatcher.Run();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }
    }
}