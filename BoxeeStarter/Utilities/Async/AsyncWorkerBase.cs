using System;
using System.Threading;

namespace BoxeeStarter.Utilities.Async
{
    public abstract class AsyncWorkerBase : IAsyncWorker
    {
        private bool _stop;
        public event EventHandler OnStop;

        #region IAsyncWorker Members

        public Thread WorkerThread { get; set; }

        public void Start()
        {
            _stop = false;
            WorkerThread = new Thread(ThreadProc);
            WorkerThread.Start(this);
        }

        public void Stop()
        {
            if (OnStop != null)
                OnStop(this, EventArgs.Empty);

            _stop = true;
        }

        #endregion

        public abstract void DoWork();

        public virtual void ThreadProc(object param)
        {
            while (!_stop)
            {
                DoWork();
            }
        }
    }
}