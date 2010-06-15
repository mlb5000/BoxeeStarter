using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Xml;
using BoxeeStarter.Utilities;
using BoxeeStarter.Utilities.Directories;
using BoxeeStarter.Utilities.Logging;
using BoxeeStarter.Utilities.Processes;

namespace BoxeeStarter.Model
{
    public class BoxeeRemoteListener : IPortListener
    {
        private const int DefaultPort = 2562;
        private bool _shutdown;

        public BoxeeRemoteListener()
            : this(DefaultPort)
        {
        }

        public BoxeeRemoteListener(int portNum)
        {
            PortNumber = portNum;
        }

        public int PortNumber { get; set; }
        public ProcessFinder ProcFinder { get; set; }
        public UdpListener Listener { get; set; }
        public ILogger Logger { get; set; }
        public ProcessStarter ProcStarter { get; set; }
        public DirectoryHelper DirHelper { get; set; }

        #region IPortListener Members

        public Thread WorkerThread { get; set; }

        public void Start()
        {
            var start = new ParameterizedThreadStart(ThreadProc);
            WorkerThread = new Thread(start);
            WorkerThread.Start(this);
        }

        public void Stop()
        {
            StopListening();
        }

        public void Listen()
        {
            while (!_shutdown)
            {
                try
                {
                    ListenForRemote();
                    Thread.Sleep(2000);
                }
                catch (Exception)
                {
                    break;   
                }
            }

            Logger.Log("Leaving receive loop.");
        }

        #endregion

        private static void ThreadProc(object param)
        {
            ((IPortListener) param).Listen();
        }

        public void ListenForRemote()
        {
            if (ProcFinder.ProcessAlreadyStarted("BOXEE"))
            {
                //Logger.Log("Boxee is already running!");
                return;
            }

            Logger.Log(String.Format("Now waiting for a connection on port {0}", PortNumber));
            string message = Listener.ListenForUdpPacket(PortNumber);

            Logger.Log(String.Format("Received Message: {0}", message));

            if (!MessageIsFromIphoneRemote(message))
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

        private bool MessageIsFromIphoneRemote(string message)
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
                    return false;
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

        public void StopListening()
        {
            _shutdown = true;
            Listener.InterruptClient();
        }
    }
}