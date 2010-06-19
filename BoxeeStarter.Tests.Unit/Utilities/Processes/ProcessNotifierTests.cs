using System;
using BoxeeStarter.Utilities.Processes;
using NUnit.Framework;

namespace BoxeeStarter.Tests.Unit.Utilities.Processes
{
    [TestFixture]
    public class ProcessNotifierTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Notifier = new ProcessNotifier(ProcessName);
            StartNotified = false;
            StopNotified = false;
            Notifier.NotifyProcessStarted += StartDelegate;
            Notifier.NotifyProcessStopped += StopDelegate;
        }

        #endregion

        private void StopDelegate(object sender, EventArgs e)
        {
            StopNotified = true;
        }

        protected const string ProcessName = "ProcName.exe";

        private void StartDelegate(object sender, EventArgs e)
        {
            StartNotified = true;
        }

        protected ProcessNotifier Notifier { get; set; }
        protected bool StartNotified { get; set; }
        protected bool StopNotified { get; set; }

        [Test]
        public void ProcessStarted_IsNotWhatWeCareAbout_DoesNotCallDelegate()
        {
            Notifier.ProcessStarted("lkjdsflkj");

            Assert.IsFalse(StartNotified);
        }

        [Test]
        public void ProcessStarted_IsWhatWeCareAbout_CallsDelegate()
        {
            Notifier.ProcessStarted(ProcessName);

            Assert.IsTrue(StartNotified);
        }

        [Test]
        public void ProcessStopped_IsNotWhatWeCareAbout_DoesNotCallDelegate()
        {
            Notifier.ProcessStopped("ljdsfj");

            Assert.IsFalse(StopNotified);
        }

        [Test]
        public void ProcessStopped_IsWhatWeCareAbout_DoesNotCallDelegate()
        {
            Notifier.ProcessStopped(ProcessName);

            Assert.IsTrue(StopNotified);
        }
    }
}