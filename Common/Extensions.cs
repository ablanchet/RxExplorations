using System;
using System.Reactive.Disposables;

namespace Common
{
    public static class Extensions
    {
        public static void WithDisposable(this IDisposable disposable, CompositeDisposable composite)
        {
            composite.Add(disposable);
        }
    }
}
