using System;
using BoxeeStarter.Utilities.Async;

namespace BoxeeStarter.Utilities.Processes
{
    public class ProcessNotifier : AsyncWorkerBase, IAsyncNotifier
    {
        private ProcessFinder _finder;

        public ProcessNotifier()
        {
        }

        public ProcessNotifier(string procName)
        {
            ProcName = procName;
        }

        public string ProcName { get; set; }

        public ProcessFinder Finder
        {
            get
            {
                if (_finder == null)
                    _finder = new ProcessFinder();

                return _finder;
            }
            set { _finder = value; }
        }

        #region IAsyncNotifier Members

        public event EventHandler NotifyMe;

        #endregion

        public override void DoWork()
        {
            if (!Finder.ProcessAlreadyStarted(ProcName))
                return;

            if (NotifyMe != null)
                NotifyMe(this, EventArgs.Empty);
        }
    }
}