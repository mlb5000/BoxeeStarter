using System.Diagnostics;

namespace BoxeeStarter.Utilities.Processes
{
    public class ProcessFinder : IProcessFinder
    {
        public virtual bool ProcessAlreadyStarted(string procname)
        {
            Process[] processes = Process.GetProcessesByName(procname);

            return processes.Length > 0;
        }
    }

    public interface IProcessFinder
    {
        bool ProcessAlreadyStarted(string procname);
    }
}