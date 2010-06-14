namespace BoxeeStarter.Model
{
    public interface IPortListener : IAsyncWorker
    {
        void Listen();
    }
}