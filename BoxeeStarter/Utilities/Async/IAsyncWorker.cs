using System.Threading;

namespace BoxeeStarter.Utilities.Async
{
    public interface IAsyncWorker
    {
        Thread WorkerThread { get; set; }

        void Start();
        void Stop();
    }
}