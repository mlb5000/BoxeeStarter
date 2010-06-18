using System;
using BoxeeStarter.Model;
using BoxeeStarter.Presenter;
using BoxeeStarter.Utilities.Registry;
using BoxeeStarter.View;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace BoxeeStarter.Tests.Unit.Presenter
{
    [TestFixture]
    public class SettingsPresenterTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Mocks = new MockRepository();
            FakeView = Mocks.StrictMock<ISettingsView>();
            FakeListener = Mocks.StrictMock<IPortListener>();
            TestPresenter = new SettingsPresenter(FakeView, FakeListener);

            var registry = Mocks.Stub<IWinRegistry>();
            FakeRegistryHelper = Mocks.StrictMock<RegistryHelper>(registry);
            TestPresenter.RegistryHelper = FakeRegistryHelper;

            Mocks.BackToRecordAll();
        }

        [TearDown]
        public void TearDown()
        {
            Mocks.VerifyAll();
        }

        #endregion

        protected IPortListener FakeListener { get; set; }
        protected ISettingsView FakeView { get; set; }
        protected SettingsPresenter TestPresenter { get; set; }
        protected MockRepository Mocks { get; set; }
        protected RegistryHelper FakeRegistryHelper { get; set; }

        protected IEventRaiser OnExitRaiser { get; set; }
        protected IEventRaiser OnLoadRaiser { get; set; }
        protected IEventRaiser OnMinRaiser { get; set; }
        protected IEventRaiser OnTrayShowRaiser { get; set; }
        protected IEventRaiser OnTrayLoadStartupRaiser { get; set; }

        public void SetViewEventExpectations()
        {
            FakeView.OnFormExit += null;
            OnExitRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            FakeView.OnFormLoad += null;
            OnLoadRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            FakeView.OnMinimized += null;
            OnMinRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            FakeView.OnTrayShowWindow += null;
            OnTrayShowRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            FakeView.OnTrayLoadStartup += null;
            OnTrayLoadStartupRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            FakeView.OnRightClickMenuOpened += null;
            OnRightClickMenuOpenedRaiser = LastCall.IgnoreArguments().GetEventRaiser();
        }

        protected IEventRaiser OnRightClickMenuOpenedRaiser { get; set; }

        public void SetInitializeExpectations()
        {
            SetViewEventExpectations();
            FakeListener.Start();
        }

        public void InitializeAndRaiseEvent(IEventRaiser raiser)
        {
            TestPresenter.Initialize();
            raiser.Raise(this, EventArgs.Empty);
        }

        [Test]
        public void Initialize_LinksUpToCorrectMethods()
        {
            SetInitializeExpectations();

            Mocks.ReplayAll();

            TestPresenter.Initialize();
        }

        [Test]
        public void OnTrayLoadStartup_RunAtStartupNotSelected_RemovesStartupFromRegistry()
        {
            SetInitializeExpectations();
            Expect.Call(FakeView.RunAtStartupSelected).Return(false);
            FakeRegistryHelper.RemoveProgramFromStartup("BoxeeStarter");

            Mocks.ReplayAll();

            InitializeAndRaiseEvent(OnTrayLoadStartupRaiser);
        }

        [Test]
        public void OnTrayLoadStartup_RunAtStartupSelected_AddsStartupToRegistry()
        {
            SetInitializeExpectations();
            Expect.Call(FakeView.RunAtStartupSelected).Return(true);
            Expect.Call(FakeView.ApplicationPath).Return("C:\\Application.exe");
            FakeRegistryHelper.RunProgramAtStartup("BoxeeStarter", "C:\\Application.exe");

            Mocks.ReplayAll();

            InitializeAndRaiseEvent(OnTrayLoadStartupRaiser);
        }

        [Test]
        public void OnTrayLoadStartupRaiser_ProgramNotSetToRunAtStartup_SetsViewToFalse()
        {
            SetInitializeExpectations();
            Expect.Call(FakeView.ApplicationPath).Return("Some Path");
            Expect.Call(FakeRegistryHelper.ProgramRunningAtStartup("BoxeeStarter", "Some Path")).Return(false);
            FakeView.RunAtStartupSelected = false;

            Mocks.ReplayAll();

            InitializeAndRaiseEvent(OnRightClickMenuOpenedRaiser);
        }

        [Test]
        public void OnTrayLoadStartupRaiser_ProgramSetToRunAtStartup_SetsViewToTrue()
        {
            SetInitializeExpectations();
            Expect.Call(FakeView.ApplicationPath).Return("Some Path");
            Expect.Call(FakeRegistryHelper.ProgramRunningAtStartup("BoxeeStarter", "Some Path")).Return(true);
            FakeView.RunAtStartupSelected = true;

            Mocks.ReplayAll();

            InitializeAndRaiseEvent(OnRightClickMenuOpenedRaiser);
        }

        [Test]
        public void TestOnFormLoad()
        {
            SetInitializeExpectations();

            Mocks.ReplayAll();

            InitializeAndRaiseEvent(OnLoadRaiser);
        }

        [Test]
        public void TestOnTrayExit()
        {
            SetInitializeExpectations();
            FakeListener.Stop();

            Mocks.ReplayAll();

            InitializeAndRaiseEvent(OnExitRaiser);
        }

        [Test]
        public void TestOnTrayShowWindow()
        {
            SetInitializeExpectations();
            //FakeView.ShowWindow();
            //FakeView.FocusOnWindow();

            Mocks.ReplayAll();

            InitializeAndRaiseEvent(OnTrayShowRaiser);
        }

        [Test]
        public void TestOnViewMinimized()
        {
            SetInitializeExpectations();
            FakeView.HideWindow();

            Mocks.ReplayAll();

            InitializeAndRaiseEvent(OnMinRaiser);
        }
    }
}