using System;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using BoxeeStarter.Utilities.Async;
using BoxeeStarter.Utilities.Directories;
using BoxeeStarter.Utilities.Logging;
using BoxeeStarter.Utilities.Network;
using BoxeeStarter.Utilities.Processes;

namespace BoxeeStarter.Model
{
    public class BoxeeRemoteListener : AsyncWorkerBase, IPortListener
    {
        private const int DefaultPort = 2562;

        public BoxeeRemoteListener()
            : this(DefaultPort)
        {
            OnStop += StopListening;
        }

        public BoxeeRemoteListener(int portNum)
        {
            PortNumber = portNum;
            BoxeeStoppedEvent = new ManualResetEvent(false);
        }

        private ManualResetEvent BoxeeStoppedEvent { get; set; }

        public int PortNumber { get; set; }
        public ProcessFinder ProcFinder { get; set; }
        public UdpListener Listener { get; set; }
        public ILogger Logger { get; set; }
        public ProcessStarter ProcStarter { get; set; }
        public DirectoryHelper DirHelper { get; set; }
        public IProcessNotifier ProcNotifier { get; set; }

        #region IPortListener Members

        #endregion

        public override void DoWork()
        {
            Listen();
        }

        public void ListenForRemote()
        {
            ProcNotifier.NotifyProcessStarted += BoxeeProcessStarted;
            Logger.Log(String.Format("Now waiting for a connection on port {0}", PortNumber));
            Listener.ListenForUdpPacket(PortNumber);
            //block until the packet is received
            Listener.PacketReceived.WaitOne();
            Listener.PacketReceived.Reset();

            string message = Listener.Message;
            Logger.Log(String.Format("Received Message: {0}", message));

            if (String.IsNullOrEmpty(message) || !MessageIsBoxeeRemoteDiscover(message))
            {
                return;
            }

            string boxeeDir = DirHelper.GetProgramDirFor("Boxee");
            if (boxeeDir == null)
            {
                Logger.Log(String.Format("Boxee directory not found in the default location: {0}", boxeeDir));
                return;
            }

            StartWindowedProcess(boxeeDir + "\\BOXEE.exe", "-p", boxeeDir);
        }

        private void BoxeeProcessStarted(object sender, EventArgs e)
        {
            ProcNotifier.NotifyProcessStarted -= BoxeeProcessStarted;
            Logger.Log("Detected Boxee starting in the background. Stopping the UDP listener.");
            Listener.InterruptClient();
        }

        private void BoxeeProcessStopped(object sender, EventArgs e)
        {
            BoxeeStoppedEvent.Set();
        }

        private bool MessageIsBoxeeRemoteDiscover(string message)
        {
            try
            {
                var xml = new XmlDocument();
                xml.LoadXml(message);

                XmlNodeList list = xml.GetElementsByTagName("bdp1");
                if (list.Count == 0)
                {
                    Logger.Log("BDP1 element missing from XML document.");
                    return false;
                }

                XmlNode node = list[0];

                XmlAttribute command = node.Attributes["cmd"];
                XmlAttribute app = node.Attributes["application"];
                XmlAttribute challenge = node.Attributes["challenge"];

                if (command.Value != "discover" || app.Value != "iphone_remote" || challenge.Value != "alohathere!")
                {
                    Logger.Log("Message is either not a discover message, or not from a Boxee mobile remote.");
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void StartWindowedProcess(string fileName, string arguments, string workingDir)
        {
            var info = new ProcessStartInfo(fileName, arguments)
                           {
                               WindowStyle = ProcessWindowStyle.Normal,
                               WorkingDirectory = workingDir,
                               UseShellExecute = false
                           };

            ProcStarter.StartProcess(info);
        }

        public void StopListening(object sender, EventArgs e)
        {
            Listener.InterruptClient();
            ProcNotifier.Stop();
        }

        public void Listen()
        {
            ProcNotifier.Start();
            if (ProcFinder.ProcessAlreadyStarted("BOXEE"))
            {
                Logger.Log("Boxee is already running!");
                ProcNotifier.NotifyProcessStopped += BoxeeProcessStopped;
                BoxeeStoppedEvent.WaitOne();
                BoxeeStoppedEvent.Reset();
                ProcNotifier.NotifyProcessStopped -= BoxeeProcessStopped;
            }

            if (_stop)
                return;

            ListenForRemote();
        }
    }
}