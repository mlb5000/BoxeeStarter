using System;
using BoxeeStarter.Utilities.Async;

namespace BoxeeStarter.Utilities.Processes
{
    public interface IProcessNotifier : IAsyncWorker
    {
        event EventHandler NotifyProcessStarted;
        event EventHandler NotifyProcessStopped;
    }
}