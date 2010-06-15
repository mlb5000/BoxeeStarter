using BoxeeStarter.Utilities.Async;

namespace BoxeeStarter.Model
{
    public interface IPortListener : IAsyncWorker
    {
        void Listen();
    }
}