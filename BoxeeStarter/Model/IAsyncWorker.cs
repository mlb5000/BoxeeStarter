using System.Threading;

namespace BoxeeStarter.Model
{
    public interface IAsyncWorker
    {
        Thread WorkerThread { get; set; }

        void Start();
        void Stop();
    }
}