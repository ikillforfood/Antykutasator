using System;

namespace Antykutasator.Services
{
    public interface IDispatcherService
    {
        void Invoke(Action action);
        event EventHandler<Exception> UnhandledException;
    }
}
