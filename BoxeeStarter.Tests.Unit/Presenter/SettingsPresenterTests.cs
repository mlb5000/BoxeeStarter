using System;
using BoxeeStarter.Model;
using BoxeeStarter.Presenter;
using BoxeeStarter.View;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace BoxeeStarter.Tests.Unit.Presenter
{
    [TestFixture]
    public class SettingsPresenterTests
    {
        [SetUp]
        public void SetUp()
        {
            Mocks = new MockRepository();
            FakeView = Mocks.StrictMock<ISettingsView>();
            FakeListener = Mocks.StrictMock<IPortListener>();
            TestPresenter = new SettingsPresenter(FakeView, FakeListener);

            Mocks.BackToRecordAll();
        }

        protected IPortListener FakeListener { get; set; }
        protected ISettingsView FakeView { get; set; }
        protected SettingsPresenter TestPresenter { get; set; }
        protected MockRepository Mocks { get; set; }

        [TearDown]
        public void TearDown()
        {
            Mocks.VerifyAll();
        }

        [Test]
        public void Initialize_LinksUpToCorrectMethods()
        {
            FakeView.OnFormExit += null;
            LastCall.IgnoreArguments();

            FakeView.OnFormLoad += null;
            LastCall.IgnoreArguments();

            FakeView.OnMinimized += null;
            LastCall.IgnoreArguments();

            FakeView.OnTrayShowWindow += null;
            LastCall.IgnoreArguments();

            FakeListener.Start();

            Mocks.ReplayAll();

            TestPresenter.Initialize();
        }

        [Test]
        public void OnViewMinimized_HidesForm()
        {
            FakeView.OnFormExit += null;
            LastCall.IgnoreArguments();

            FakeView.OnFormLoad += null;
            LastCall.IgnoreArguments();

            FakeView.OnMinimized += null;
            IEventRaiser raiser = LastCall.IgnoreArguments().GetEventRaiser();

            FakeView.OnTrayShowWindow += null;
            LastCall.IgnoreArguments();

            FakeListener.Start();

            FakeView.HideWindow();

            Mocks.ReplayAll();

            TestPresenter.Initialize();
            raiser.Raise(this, EventArgs.Empty);
        }

        [Test]
        public void OnTrayExit_StopsListener()
        {
            FakeView.OnFormExit += null;
            IEventRaiser raiser = LastCall.IgnoreArguments().GetEventRaiser();

            FakeView.OnFormLoad += null;
            LastCall.IgnoreArguments();

            FakeView.OnMinimized += null;
            LastCall.IgnoreArguments();

            FakeView.OnTrayShowWindow += null;
            LastCall.IgnoreArguments();

            FakeListener.Start();

            FakeListener.Stop();

            Mocks.ReplayAll();

            TestPresenter.Initialize();
            raiser.Raise(this, EventArgs.Empty);
        }

        [Test]
        public void OnTrayShowWindow_StopsListener()
        {
            FakeView.OnFormExit += null;
            LastCall.IgnoreArguments();

            FakeView.OnFormLoad += null;
            LastCall.IgnoreArguments();

            FakeView.OnMinimized += null;
            LastCall.IgnoreArguments();

            FakeView.OnTrayShowWindow += null;
            IEventRaiser raiser = LastCall.IgnoreArguments().GetEventRaiser();

            FakeListener.Start();

            FakeView.ShowWindow();
            FakeView.FocusOnWindow();

            Mocks.ReplayAll();

            TestPresenter.Initialize();
            raiser.Raise(this, EventArgs.Empty);
        }

        [Test]
        public void OnFormLoad_StopsListener()
        {
            FakeView.OnFormExit += null;
            LastCall.IgnoreArguments();

            FakeView.OnFormLoad += null;
            IEventRaiser raiser = LastCall.IgnoreArguments().GetEventRaiser();

            FakeView.OnMinimized += null;
            LastCall.IgnoreArguments();

            FakeView.OnTrayShowWindow += null;
            LastCall.IgnoreArguments();

            FakeListener.Start();

            Mocks.ReplayAll();

            TestPresenter.Initialize();
            raiser.Raise(this, EventArgs.Empty);
        }

    }
}