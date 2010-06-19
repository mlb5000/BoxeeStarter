using System;
using System.Threading;
using BoxeeStarter.Model;
using BoxeeStarter.Utilities.Directories;
using BoxeeStarter.Utilities.Logging;
using BoxeeStarter.Utilities.Network;
using BoxeeStarter.Utilities.Processes;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace BoxeeStarter.Tests.Unit.Model
{
    [TestFixture]
    public class BoxeeRemoteListenerTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Mocks = new MockRepository();

            Logger = MockRepository.GenerateStub<ILogger>();
            ProcFinder = Mocks.StrictMock<ProcessFinder>();
            UdpListener = Mocks.StrictMock<UdpListener>();
            ProcStarter = Mocks.StrictMock<ProcessStarter>();
            DirHelper = Mocks.StrictMock<DirectoryHelper>();
            ProcNotifier = Mocks.StrictMock<IProcessNotifier>();

            Listener = new BoxeeRemoteListener
                           {
                               Logger = Logger,
                               ProcFinder = ProcFinder,
                               Listener = UdpListener,
                               ProcStarter = ProcStarter,
                               DirHelper = DirHelper,
                               ProcNotifier = ProcNotifier
                           };

            Mocks.BackToRecordAll();
        }

        [TearDown]
        public void TearDown()
        {
            Mocks.VerifyAll();
        }

        #endregion

        private const string RemoteDiscoverXML =
            "<bdp1 cmd=\"discover\" application=\"iphone_remote\" version=\"1\" challenge=\"alohathere!\" signature=\"51d3feb33925ce8aa108ec95d8a841ae\"/>";

        private const string RemoteNotDiscoverXml =
            "<bdp1 cmd=\"notdiscover\" application=\"iphone_remote\" version=\"1\" challenge=\"alohathere!\" signature=\"51d3feb33925ce8aa108ec95d8a841ae\"/>";

        private const string RemoteNotiPhoneXml =
            "<bdp1 cmd=\"discover\" application=\"android_remote\" version=\"1\" challenge=\"alohathere!\" signature=\"51d3feb33925ce8aa108ec95d8a841ae\"/>";

        private const string RemoteWrongChallengeXml =
            "<bdp1 cmd=\"discover\" application=\"iphone_remote\" version=\"1\" challenge=\"chao!\" signature=\"51d3feb33925ce8aa108ec95d8a841ae\"/>";

        private const string ValidXmlNoBdp1Node =
            "<bdp2 cmd=\"discover\" application=\"iphone_remote\" version=\"1\" challenge=\"alohathere!\" signature=\"51d3feb33925ce8aa108ec95d8a841ae\"/>";

        private const string NotXml = "Hello world!";

        protected ILogger Logger { get; set; }
        protected MockRepository Mocks { get; set; }
        protected BoxeeRemoteListener Listener { get; set; }
        protected ProcessFinder ProcFinder { get; set; }
        protected UdpListener UdpListener { get; set; }
        protected ProcessStarter ProcStarter { get; set; }
        protected DirectoryHelper DirHelper { get; set; }
        protected IProcessNotifier ProcNotifier { get; set; }

        public void StartListening()
        {
            var t = new Thread(ThreadProc);
            t.Start();
            Thread.Sleep(500);
        }

        public void ThreadProc()
        {
            Listener.Listen();
        }

        [Test]
        public void Listen_BoxeeAlreadyRunning_BlocksUntilBoxeeStops()
        {
            Expect.Call(ProcFinder.ProcessAlreadyStarted("BOXEE")).Return(true);
            UdpListener.ListenForUdpPacket(2562);
            UdpListener.Message = RemoteDiscoverXML;
            Expect.Call(DirHelper.GetProgramDirFor("Boxee")).Return("Something");
            ProcStarter.StartProcess(null);
            LastCall.IgnoreArguments();
            ProcNotifier.NotifyProcessStarted += null;
            LastCall.IgnoreArguments().GetEventRaiser();
            ProcNotifier.NotifyProcessStopped += null;
            IEventRaiser stopRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            ProcNotifier.NotifyProcessStopped -= null;
            LastCall.IgnoreArguments();
            ProcNotifier.Start();

            Mocks.ReplayAll();
            StartListening();
            stopRaiser.Raise(this, EventArgs.Empty);
            UdpListener.PacketReceived.Set();
            Thread.Sleep(500);
        }

        [Test]
        public void Listen_BoxeeNotRunning_ReceivesGoodPacket_StartsBoxee()
        {
            Expect.Call(ProcFinder.ProcessAlreadyStarted("BOXEE")).Return(false);
            UdpListener.ListenForUdpPacket(2562);
            UdpListener.Message = RemoteDiscoverXML;
            Expect.Call(DirHelper.GetProgramDirFor("Boxee")).Return("Something");
            ProcStarter.StartProcess(null);
            LastCall.IgnoreArguments();
            ProcNotifier.NotifyProcessStarted += null;
            LastCall.IgnoreArguments();
            ProcNotifier.Start();

            Mocks.ReplayAll();
            StartListening();
            UdpListener.PacketReceived.Set();
        }

        [Test]
        public void Listen_BoxeeNotRunning_CommandNotDiscover_Returns()
        {
            Expect.Call(ProcFinder.ProcessAlreadyStarted("BOXEE")).Return(false);
            UdpListener.ListenForUdpPacket(2562);
            UdpListener.Message = RemoteNotDiscoverXml;
            ProcNotifier.NotifyProcessStarted += null;
            LastCall.IgnoreArguments();
            ProcNotifier.Start();

            Mocks.ReplayAll();
            StartListening();
            UdpListener.PacketReceived.Set();
        }
        
        [Test]
        public void Listen_BoxeeNotRunning_CommandDidNotComeFromiPhone_Returns()
        {
            Expect.Call(ProcFinder.ProcessAlreadyStarted("BOXEE")).Return(false);
            UdpListener.ListenForUdpPacket(2562);
            UdpListener.Message = RemoteNotiPhoneXml;
            ProcNotifier.NotifyProcessStarted += null;
            LastCall.IgnoreArguments();
            ProcNotifier.Start();

            Mocks.ReplayAll();
            StartListening();
            UdpListener.PacketReceived.Set();
        }

        [Test]
        public void Listen_BoxeeNotRunning_CommandNotXml_Returns()
        {
            Expect.Call(ProcFinder.ProcessAlreadyStarted("BOXEE")).Return(false);
            UdpListener.ListenForUdpPacket(2562);
            UdpListener.Message = NotXml;
            ProcNotifier.NotifyProcessStarted += null;
            LastCall.IgnoreArguments();
            ProcNotifier.Start();

            Mocks.ReplayAll();
            StartListening();
            UdpListener.PacketReceived.Set();
        }

        [Test]
        public void Listen_BoxeeNotRunning_CommandWrongChallenge_Returns()
        {
            Expect.Call(ProcFinder.ProcessAlreadyStarted("BOXEE")).Return(false);
            UdpListener.ListenForUdpPacket(2562);
            UdpListener.Message = RemoteWrongChallengeXml;
            ProcNotifier.NotifyProcessStarted += null;
            LastCall.IgnoreArguments();
            ProcNotifier.Start();

            Mocks.ReplayAll();
            StartListening();
            UdpListener.PacketReceived.Set();
        }

        [Test]
        public void Listen_BoxeeNotRunning_CommandMissingBdp1Node_Returns()
        {
            Expect.Call(ProcFinder.ProcessAlreadyStarted("BOXEE")).Return(false);
            UdpListener.ListenForUdpPacket(2562);
            UdpListener.Message = ValidXmlNoBdp1Node;
            ProcNotifier.NotifyProcessStarted += null;
            LastCall.IgnoreArguments();
            ProcNotifier.Start();

            Mocks.ReplayAll();
            StartListening();
            UdpListener.PacketReceived.Set();
        }

        [Test]
        public void ListenForRemote_BoxeeNotInstalled_Returns()
        {
            Expect.Call(ProcFinder.ProcessAlreadyStarted("BOXEE")).Return(false);
            UdpListener.ListenForUdpPacket(2562);
            UdpListener.Message = RemoteDiscoverXML;
            Expect.Call(DirHelper.GetProgramDirFor("Boxee")).Return(null);
            ProcNotifier.NotifyProcessStarted += null;
            LastCall.IgnoreArguments();
            ProcNotifier.Start();

            Mocks.ReplayAll();

            StartListening();
            UdpListener.PacketReceived.Set();
        }

        [Test]
        public void ListenForRemote_BoxeeStartsInBackground_StopsListening()
        {
            Expect.Call(ProcFinder.ProcessAlreadyStarted("BOXEE")).Return(false);
            UdpListener.ListenForUdpPacket(2562);
            UdpListener.Message = String.Empty;
            ProcNotifier.NotifyProcessStarted += null;
            IEventRaiser raiser = LastCall.IgnoreArguments().GetEventRaiser();
            ProcNotifier.Start();
            UdpListener.InterruptClient();
            ProcNotifier.NotifyProcessStarted -= null;
            LastCall.IgnoreArguments();

            Mocks.ReplayAll();
            StartListening();
            raiser.Raise(this, EventArgs.Empty);
            UdpListener.PacketReceived.Set();
            Thread.Sleep(500);
        }

        [Test]
        public void TestStopListening_StopsUdpListenerAndProcessListener()
        {
            UdpListener.InterruptClient();
            LastCall.IgnoreArguments();

            ProcNotifier.Stop();
            LastCall.IgnoreArguments();

            Mocks.ReplayAll();

            Listener.StopListening(this, EventArgs.Empty);
        }
    }
}