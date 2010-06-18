using BoxeeStarter.Model;
using BoxeeStarter.View;
using NUnit.Framework;
using Rhino.Mocks;

namespace BoxeeStarter.Tests.Unit.View
{
    [TestFixture]
    public class ViewObserverTests
    {
        private MockRepository Mocks { get; set; }

        protected IPortListener FakeListener { get; set; }
        protected ISettingsView FakeView { get; set; }

        [Test]
        public void Observe_SettingsViewAndListener_NoExceptions()
        {
            Mocks = new MockRepository();

            FakeView = Mocks.StrictMock<ISettingsView>();
            FakeListener = Mocks.StrictMock<IPortListener>();

            FakeView.OnFormExit += null;
            LastCall.IgnoreArguments();

            FakeView.OnFormLoad += null;
            LastCall.IgnoreArguments();

            FakeView.OnMinimized += null;
            LastCall.IgnoreArguments();

            FakeView.OnTrayShowWindow += null;
            LastCall.IgnoreArguments();

            FakeView.OnTrayLoadStartup += null;
            LastCall.IgnoreArguments();

            FakeView.OnRightClickMenuOpened += null;
            LastCall.IgnoreArguments();

            FakeListener.Start();

            Mocks.ReplayAll();

            ViewObserver.Observe(FakeView, FakeListener);
        }
    }
}