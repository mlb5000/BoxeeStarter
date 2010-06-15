using System.Diagnostics;

namespace BoxeeStarter.Utilities.Processes
{
    public class ProcessStarter
    {
        public virtual void StartProcess(ProcessStartInfo info)
        {
            Process.Start(info);
        }
    }
}