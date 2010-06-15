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

        protected string ProcName { get; set; }

        protected ProcessFinder Finder
        {
            get
            {
                if (_finder == null)
                    _finder = new ProcessFinder();

                return _finder;
            }
            set { _finder = value; }
        }

        #region INotifier Members

        public event EventHandler NotifyMe;

        #endregion

        public override void DoWork()
        {
            if (!Finder.ProcessAlreadyStarted("BOXEE"))
                return;

            if (NotifyMe != null)
                NotifyMe(this, EventArgs.Empty);
        }
    }
}