using System;
using System.Management;
using System.Threading;
using BoxeeStarter.Utilities.Logging;

namespace BoxeeStarter.Utilities.Processes
{
    public class ProcessNotifier : IProcessNotifier
    {
        public ProcessNotifier(string procName)
        {
            ProcName = procName;
            StartWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            StartWatcher.EventArrived += StartEventArrived;
            StopWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            StopWatcher.EventArrived += StopEventArrived;
        }

        public string ProcName { get; set; }
        private ManagementEventWatcher StartWatcher { get; set; }
        private ManagementEventWatcher StopWatcher { get; set; }

        #region IProcessNotifier Members

        public event EventHandler NotifyProcessStarted;

        public Thread WorkerThread
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void Start()
        {
            StartWatcher.Start();
            StopWatcher.Start();
        }

        public void Stop()
        {
            StartWatcher.Stop();
            StopWatcher.Stop();
        }

        #endregion

        private void StopEventArrived(object sender, EventArrivedEventArgs e)
        {
            var procName = ((string) e.NewEvent.Properties["ProcessName"].Value);
            new EventLogger().Log(String.Format("Process Stopped: {0}", procName));
            ProcessStopped(procName);
        }

        public void ProcessStopped(string name)
        {
            if (NotifyProcessStopped == null)
                return;

            if (name.Equals(ProcName, StringComparison.InvariantCultureIgnoreCase))
            {
                NotifyProcessStopped(this, EventArgs.Empty);
            }
        }

        public event EventHandler NotifyProcessStopped;

        private void StartEventArrived(object sender, EventArrivedEventArgs e)
        {
            var procName = ((string) e.NewEvent.Properties["ProcessName"].Value);
            new EventLogger().Log(String.Format("Process Started: {0}", procName));
            ProcessStarted(procName);
        }

        public void ProcessStarted(string name)
        {
            if (NotifyProcessStarted == null)
                return;

            if (name.Equals(ProcName, StringComparison.InvariantCultureIgnoreCase))
            {
                NotifyProcessStarted(this, EventArgs.Empty);
            }
        }
    }
}