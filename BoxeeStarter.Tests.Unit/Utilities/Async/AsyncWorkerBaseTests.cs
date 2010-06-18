using System;
using System.Threading;
using BoxeeStarter.Utilities.Async;
using NUnit.Framework;
using Rhino.Mocks;

namespace BoxeeStarter.Tests.Unit.Utilities.Async
{
    [TestFixture]
    public class AsyncWorkerBaseTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Mocks = new MockRepository();
            AsyncWorker = Mocks.PartialMock<AsyncWorkerBase>();

            Mocks.BackToRecordAll();
        }

        [TearDown]
        public void TearDown()
        {
            Mocks.VerifyAll();
        }

        #endregion

        protected AsyncWorkerBase AsyncWorker { get; set; }
        protected MockRepository Mocks { get; set; }

        private void StopTestDelegate(object sender, EventArgs e)
        {
            StopDelegateCalled = true;
        }

        protected bool StopDelegateCalled { get; set; }

        [Test]
        public void Start_CallsDoWork_UntilStopCalled()
        {
            AsyncWorker.DoWork();
            LastCall.IgnoreArguments().Repeat.Any();

            Mocks.ReplayAll();

            AsyncWorker.Start();
            Thread.Sleep(250);
            AsyncWorker.Stop();
        }

        [Test]
        public void Stop_CallsOnStopDelegate()
        {
            AsyncWorker.DoWork();
            LastCall.IgnoreArguments().Repeat.Any();

            Mocks.ReplayAll();

            AsyncWorker.OnStop += StopTestDelegate;
            AsyncWorker.Start();
            Thread.Sleep(250);
            AsyncWorker.Stop();
            Assert.IsTrue(StopDelegateCalled);
        }
    }
}