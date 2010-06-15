using System;
using BoxeeStarter.Model;
using BoxeeStarter.Utilities;
using BoxeeStarter.Utilities.Directories;
using BoxeeStarter.Utilities.Logging;
using BoxeeStarter.Utilities.Processes;
using NUnit.Framework;
using Rhino.Mocks;

namespace BoxeeStarter.Tests.Unit.Model
{
    [TestFixture]
    public class BoxeeRemoteListenerTests
    {
        private const string RemoteDiscoverXML = "<bdp1 cmd=\"discover\" application=\"iphone_remote\" version=\"1\" challenge=\"alohathere!\" signature=\"51d3feb33925ce8aa108ec95d8a841ae\"/>";
        private const string NotXml = "Hello world!";

        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Mocks = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            Mocks.VerifyAll();
        }

        #endregion

        protected ILogger Logger { get; set; }
        protected MockRepository Mocks { get; set; }
        protected BoxeeRemoteListener Listener { get; set; }
        protected ProcessFinder ProcFinder { get; set; }
        protected UdpListener UdpListener { get; set; }
        protected ProcessStarter ProcStarter { get; set; }
        protected DirectoryHelper DirHelper { get; set; }

        [Test]
        public void ListenForRemote_BoxeeIsRunning_Returns()
        {
            ProcFinder = Mocks.StrictMock<ProcessFinder>();
            Logger = MockRepository.GenerateStub<ILogger>();
            Listener = new BoxeeRemoteListener { ProcFinder = ProcFinder, Logger = Logger };

            Mocks.BackToRecordAll();
            Expect.Call(ProcFinder.ProcessAlreadyStarted("BOXEE")).Return(true);

            Mocks.ReplayAll();
            Listener.ListenForRemote();
        }
        
        [Test]
        public void ListenForRemote_BoxeeIsNotRunning_ListensForRemote()
        {
            Logger = MockRepository.GenerateStub<ILogger>();
            ProcFinder = Mocks.StrictMock<ProcessFinder>();
            UdpListener = Mocks.StrictMock<UdpListener>();
            ProcStarter = MockRepository.GenerateStub<ProcessStarter>();
            DirHelper = MockRepository.GenerateStub<DirectoryHelper>();
            
            Listener = new BoxeeRemoteListener
                           {
                               Logger = Logger,
                               ProcFinder = ProcFinder,
                               Listener = UdpListener,
                               ProcStarter = ProcStarter,
                               DirHelper = DirHelper
                           };

            Mocks.BackToRecordAll();
            Expect.Call(ProcFinder.ProcessAlreadyStarted("BOXEE")).Return(false);
            Expect.Call(UdpListener.ListenForUdpPacket(2562)).Return(RemoteDiscoverXML).Repeat.Any();

            Mocks.ReplayAll();
            Listener.ListenForRemote();
        }

        [Test]
        public void ListenForRemote_BoxeeNotRunningValidRemoteDiscover_StartsBoxee()
        {
            Logger = MockRepository.GenerateStub<ILogger>();
            ProcFinder = Mocks.StrictMock<ProcessFinder>();
            UdpListener = Mocks.StrictMock<UdpListener>();
            ProcStarter = Mocks.StrictMock<ProcessStarter>();
            DirHelper = Mocks.StrictMock<DirectoryHelper>();

            Listener = new BoxeeRemoteListener
                           {
                               Logger = Logger,
                               ProcFinder = ProcFinder,
                               Listener = UdpListener,
                               ProcStarter = ProcStarter,
                               DirHelper = DirHelper
                           };

            Mocks.BackToRecordAll();
            Expect.Call(ProcFinder.ProcessAlreadyStarted("BOXEE")).Return(false);
            Expect.Call(UdpListener.ListenForUdpPacket(2562)).Return(RemoteDiscoverXML).Repeat.Any();
            Expect.Call(DirHelper.GetProgramDirFor("Boxee")).Return("Something");
            ProcStarter.StartProcess(null);
            LastCall.IgnoreArguments();

            Mocks.ReplayAll();
            Listener.ListenForRemote();
        }

        [Test]
        public void ListenForRemote_BoxeeNotRunningNotXml_Returns()
        {
            Logger = MockRepository.GenerateStub<ILogger>();
            ProcFinder = Mocks.StrictMock<ProcessFinder>();
            UdpListener = Mocks.StrictMock<UdpListener>();
            ProcStarter = Mocks.StrictMock<ProcessStarter>();
            DirHelper = MockRepository.GenerateStub<DirectoryHelper>();

            Listener = new BoxeeRemoteListener
                           {
                               Logger = Logger,
                               ProcFinder = ProcFinder,
                               Listener = UdpListener,
                               ProcStarter = ProcStarter,
                               DirHelper = DirHelper
                           };

            Mocks.BackToRecordAll();
            Expect.Call(ProcFinder.ProcessAlreadyStarted("BOXEE")).Return(false);
            Expect.Call(UdpListener.ListenForUdpPacket(2562)).Return(NotXml).Repeat.Any();

            Mocks.ReplayAll();
            Listener.ListenForRemote();
        }

        [Test]
        public void ListenForRemote_BoxeeNotInstalled_Returns()
        {
            Logger = MockRepository.GenerateStub<ILogger>();
            ProcFinder = Mocks.StrictMock<ProcessFinder>();
            UdpListener = Mocks.StrictMock<UdpListener>();
            ProcStarter = Mocks.StrictMock<ProcessStarter>();
            DirHelper = Mocks.StrictMock<DirectoryHelper>();

            Listener = new BoxeeRemoteListener
                           {
                               Logger = Logger,
                               ProcFinder = ProcFinder,
                               Listener = UdpListener,
                               ProcStarter = ProcStarter,
                               DirHelper = DirHelper
                           };

            Mocks.BackToRecordAll();
            Expect.Call(ProcFinder.ProcessAlreadyStarted("BOXEE")).Return(false);
            Expect.Call(UdpListener.ListenForUdpPacket(2562)).Return(RemoteDiscoverXML).Repeat.Any();
            Expect.Call(DirHelper.GetProgramDirFor("Boxee")).Return(null);

            Mocks.ReplayAll();
            Listener.ListenForRemote();
        }
    }
}