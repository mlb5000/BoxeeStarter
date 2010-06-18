using System.Diagnostics;

namespace BoxeeStarter.Utilities.Processes
{
    public class ProcessFinder : IProcessFinder
    {
        #region IProcessFinder Members

        public virtual bool ProcessAlreadyStarted(string procname)
        {
            Process[] processes = Process.GetProcessesByName(procname);

            return processes.Length > 0;
        }

        #endregion
    }

    public interface IProcessFinder
    {
        bool ProcessAlreadyStarted(string procname);
    }
}