using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;

namespace MultiThreadedObservables
{
    class Program
    {
        private static readonly CompositeDisposable _disposable = new CompositeDisposable();

        static void Main(string[] args)
        {
            Run(DoesNotSlowDownOtherConsumersButStillShared);
        }

        private static void Run(Action action)
        {
            var source = new CancellationTokenSource();
            Console.CancelKeyPress += (o, e) =>
            {
                e.Cancel = true;
                source.Cancel();
                _disposable.Dispose();
            };

            action();
            source.Token.WaitHandle.WaitOne();
        }

        private static void DoesNotSlowDownOtherConsumers()
        {
            var observable = Observable.Interval(TimeSpan.FromMilliseconds(300));

            observable.Do(l => Thread.Sleep(1000))
                      .Subscribe(l => Console.WriteLine($"Slow subscriber {l}"))
                      .WithDisposable(_disposable);

            observable.Subscribe(l => Console.WriteLine($"Normal subscriber {l}"))
                      .WithDisposable(_disposable);
        }

        private static void SlowDownOtherConsumers()
        {
            var observable = Observable.Interval(TimeSpan.FromMilliseconds(300))
                                       .Publish()
                                       .RefCount();

            observable.Do(l => Thread.Sleep(1000))
                      .Subscribe(l => Console.WriteLine($"Slow subscriber {l}"))
                      .WithDisposable(_disposable);

            observable.Subscribe(l => Console.WriteLine($"Normal subscriber {l}"))
                      .WithDisposable(_disposable);
        }

        private static void DoesNotSlowDownOtherConsumersButStillShared()
        {
            var observable = Observable.Interval(TimeSpan.FromMilliseconds(300))
                                       .Publish()
                                       .RefCount()
                                       .ObserveOn(ThreadPoolScheduler.Instance);

            observable.Do(l => Thread.Sleep(1000))
                      .Subscribe(l => Console.WriteLine($"Slow subscriber {l}"))
                      .WithDisposable(_disposable);

            observable.Subscribe(l => Console.WriteLine($"Normal subscriber {l}"))
                      .WithDisposable(_disposable);
        }
    }
}
