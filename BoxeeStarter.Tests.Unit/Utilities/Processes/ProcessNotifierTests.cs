using System;
using BoxeeStarter.Utilities.Processes;
using NUnit.Framework;
using Rhino.Mocks;

namespace BoxeeStarter.Tests.Unit.Utilities.Processes
{
    [TestFixture]
    public class ProcessNotifierTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Mocks = new MockRepository();
            Finder = Mocks.StrictMock<ProcessFinder>();
            Notifier = new ProcessNotifier {Finder = Finder};
            Notifier.NotifyMe += NotifyDelegate;
            Notified = false;
            Notifier.ProcName = ProcessName;

            Mocks.BackToRecordAll();
        }

        [TearDown]
        public void TearDown()
        {
            Mocks.VerifyAll();
        }

        #endregion

        protected const string ProcessName = "ProcName";

        private void NotifyDelegate(object sender, EventArgs e)
        {
            Notified = true;
        }

        protected ProcessNotifier Notifier { get; set; }
        protected ProcessFinder Finder { get; set; }
        protected MockRepository Mocks { get; set; }
        protected bool Notified { get; set; }

        [Test]
        public void DoWork_ProcessIsNotRunning_DoesNotCallDelegate()
        {
            Expect.Call(Finder.ProcessAlreadyStarted(ProcessName)).Return(false);

            Mocks.ReplayAll();
            Notifier.DoWork();

            Assert.IsFalse(Notified);
        }

        [Test]
        public void DoWork_ProcessIsRunning_CallsDelegate()
        {
            Expect.Call(Finder.ProcessAlreadyStarted(ProcessName)).Return(true);

            Mocks.ReplayAll();
            Notifier.DoWork();

            Assert.IsTrue(Notified);
        }

        [Test]
        public void DoWork_ProductionConstructorProcessExists_CallsDelegate()
        {
            Notifier = new ProcessNotifier("services");
            Notifier.Finder = Finder;
            Notifier.NotifyMe += NotifyDelegate;
            Expect.Call(Finder.ProcessAlreadyStarted("services")).Return(true);

            Mocks.ReplayAll();
            Notifier.DoWork();

            Assert.IsTrue(Notified);
        }

        [Test]
        public void DoWork_ProductionConstructorAndFinderProcessExists_CallsDelegate()
        {
            Notifier = new ProcessNotifier("services");
            Notifier.NotifyMe += NotifyDelegate;

            Mocks.ReplayAll();
            Notifier.DoWork();

            Assert.IsTrue(Notified);
        }
    }
}